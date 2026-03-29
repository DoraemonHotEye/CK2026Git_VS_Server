#pragma once
#include "CSession.h"
#include "../Headers/Packets.h"
#include <memory>
#include <queue>
#include <vector>
#include <mutex>
#include <atomic>
#include <winsock2.h>
#include <windows.h>
#include "../Managers/SessionManager.h"

void CSession::Start(SOCKET socket, HANDLE iocp)
{
    _socket = socket;
    _iocp = iocp;

    CreateIoCompletionPort((HANDLE)_socket, iocp, (ULONG_PTR)this, 0);

    // 풀에서 나온 세션이면 Reset() 호출 후 다시 초기화
    Reset();

    _recvContext = std::make_unique<RecvContext>();
    _recvContext->icopObj = this;
    _recvContext->wsaBuf.buf = _recvContext->buffer;
    _recvContext->wsaBuf.len = sizeof(_recvContext->buffer);

    _sendContext = std::make_unique<SendContext>();

    RegisterRecv();
}

void CSession::Reset()
{
    // 1. 남은 I/O 취소
    CancelIoEx((HANDLE)_socket, nullptr);

    // 2. 플래그 초기화
    _disconnected.store(0);

    // 3. 큐 및 버퍼 초기화
    {
        std::lock_guard<std::mutex> lock(_sendLock);
        std::queue<std::vector<uint8_t>> emptyQueue;
        std::swap(_sendQueue, emptyQueue);
    }
    _pendingBuffer.clear();

    // 4. 기존 컨텍스트 삭제
    _recvContext.reset();
    _sendContext.reset();
}

void CSession::Send(PacketInterface& packet)
{
    if (_disconnected) return;

    std::vector<uint8_t> serialized = packet.Serialize();

    std::lock_guard<std::mutex> lock(_sendLock);
    _sendQueue.push(std::move(serialized));

    if (_pendingBuffer.empty())
        RegisterSend();
}

void CSession::Disconnect()
{
    if (_disconnected.exchange(1))
        return;

    SOCKADDR_IN clientAddr = {};
    int addrLen = sizeof(clientAddr);
    if (getpeername(_socket, (SOCKADDR*)&clientAddr, &addrLen) == SOCKET_ERROR)
    {
        ZeroMemory(&clientAddr, sizeof(clientAddr));
        printf("getpeername failed: %d\n", WSAGetLastError());
    }

    shutdown(_socket, SD_BOTH);
    closesocket(_socket);

    Reset(); // 안전하게 초기화

    SessionManager::Instance().RemoveSession(this);

    OnDisconnected(clientAddr);
}

void CSession::RegisterRecv()
{
    if (_disconnected) return;

    DWORD recvBytes = 0;
    DWORD flags = 0;

    ZeroMemory(&_recvContext->overlapped, sizeof(OVERLAPPED));

    _recvContext->wsaBuf.buf = reinterpret_cast<char*>(_recvBuffer.WritePtr());
    _recvContext->wsaBuf.len = (ULONG)_recvBuffer.FreeSize();

    int ret = WSARecv(
        _socket,
        &_recvContext->wsaBuf,
        1,
        &recvBytes,
        &flags,
        &_recvContext->overlapped,
        NULL
    );

    if (ret == SOCKET_ERROR && WSAGetLastError() != WSA_IO_PENDING)
        Disconnect();
}

void CSession::RegisterSend()
{
    if (_disconnected) return;

    if (_pendingBuffer.empty())
    {
        while (!_sendQueue.empty())
        {
            auto& data = _sendQueue.front();
            _pendingBuffer.insert(_pendingBuffer.end(), data.begin(), data.end());
            _sendQueue.pop();
        }
    }

    if (_pendingBuffer.empty())
        return;

    _sendContext->type = IOContext::Type::Send;
    _sendContext->icopObj = this;
    _sendContext->wsaBuf.buf = reinterpret_cast<char*>(_pendingBuffer.data());
    _sendContext->wsaBuf.len = static_cast<ULONG>(_pendingBuffer.size());

    DWORD sendBytes = 0;
    ZeroMemory(&_sendContext->overlapped, sizeof(OVERLAPPED));

    int ret = WSASend(
        _socket,
        &_sendContext->wsaBuf,
        1,
        &sendBytes,
        0,
        &_sendContext->overlapped,
        NULL
    );

    if (ret == SOCKET_ERROR && WSAGetLastError() != WSA_IO_PENDING)
        Disconnect();
}

void CSession::ProcessRecv(int bytes)
{
    // 1. Write 커서 이동
    if (_recvBuffer.OnWrite(bytes) == false)
    {
        Disconnect();
        return;
    }

    // 2. RecvBuffer 기준으로 처리
    int processLen = OnRecv(
        reinterpret_cast<char*>(_recvBuffer.ReadPtr()),
        (int)_recvBuffer.DataSize()
    );

    // 3. 처리 검증
    if (processLen < 0 || _recvBuffer.DataSize() < (size_t)processLen)
    {
        Disconnect();
        return;
    }

    // 4. Read 커서 이동
    if (_recvBuffer.OnRead(processLen) == false)
    {
        Disconnect();
        return;
    }

    // 5. 버퍼 정리
    _recvBuffer.Clean();

    // 6. 다시 recv 등록
    RegisterRecv();
}

void CSession::ProcessSend(int bytes)
{
    OnSend(bytes);

    _pendingBuffer.clear();

    if (!_sendQueue.empty())
        RegisterSend();
}
