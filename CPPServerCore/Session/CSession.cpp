#include "CSession.h"

void CSession::Start(SOCKET socket, HANDLE iocp)
{
    _socket = socket;
    _iocp = iocp;

    CreateIoCompletionPort((HANDLE)_socket, iocp, (ULONG_PTR)this, 0);

    _recvContext.type = IOContext::Type::Recv;
    _recvContext.wsaBuf.buf = _recvContext.buffer;
    _recvContext.wsaBuf.len = sizeof(_recvContext.buffer);

    RegisterRecv();
}

void CSession::Send(std::vector<char> data)
{
    if (_disconnected) return;

    std::lock_guard<std::mutex> lock(_sendLock);

    _sendQueue.push(std::move(data));

    if (_pendingBuffer.empty())
    {
        RegisterSend();
    }
}

void CSession::Disconnect()
{
    if (_disconnected.exchange(1))
        return;

    shutdown(_socket, SD_BOTH);
    closesocket(_socket);

    OnDisconnected();
}

void CSession::RegisterRecv()
{if (_disconnected) return;

    DWORD recvBytes = 0;
    DWORD flags = 0;

    ZeroMemory(&_recvContext.overlapped, sizeof(OVERLAPPED));

    int ret = WSARecv(
        _socket,
        &_recvContext.wsaBuf,
        1,
        &recvBytes,
        &flags,
        &_recvContext.overlapped,
        NULL
    );

    if (ret == SOCKET_ERROR)
    {
        if (WSAGetLastError() != WSA_IO_PENDING)
            Disconnect();
    }
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

    _sendContext.type = IOContext::Type::Send;
    _sendContext.wsaBuf.buf = _pendingBuffer.data();
    _sendContext.wsaBuf.len = (ULONG)_pendingBuffer.size();

    DWORD sendBytes = 0;

    ZeroMemory(&_sendContext.overlapped, sizeof(OVERLAPPED));

    int ret = WSASend(
        _socket,
        &_sendContext.wsaBuf,
        1,
        &sendBytes,
        0,
        &_sendContext.overlapped,
        NULL
    );

    if (ret == SOCKET_ERROR)
    {
        if (WSAGetLastError() != WSA_IO_PENDING)
            Disconnect();
    }
}

void CSession::ProcessRecv(int bytes)
{
    int processLen = OnRecv(_recvContext.buffer, bytes);

    if (processLen <= 0)
    {
        Disconnect();
        return;
    }

    RegisterRecv();
}

void CSession::ProcessSend(int bytes)
{
    OnSend(bytes);

    _pendingBuffer.clear();

    if (!_sendQueue.empty())
        RegisterSend();
}