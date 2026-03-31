#include "CUSession.h"
#include <string>
#include <WinSock2.h>
#include <WS2tcpip.h>

#include "../../ServerSupports/PrintLogThread.h"

#pragma comment(lib, "ws2_32.lib")

void CUSession::Start(u_short port, HANDLE Iocp)
{
    _iocp = Iocp;

    _socket = WSASocket(
        AF_INET,
        SOCK_DGRAM,
        IPPROTO_UDP,
        NULL,
        0,
        WSA_FLAG_OVERLAPPED
    );

    if (_socket == INVALID_SOCKET)
    {
        LOG("Socket creation failed");
        return;
    }

    SOCKADDR_IN addr = {};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    addr.sin_port = htons(port);

    if (bind(_socket, (SOCKADDR*)&addr, sizeof(addr)) == SOCKET_ERROR)
    {
        LOG("Bind failed");
        closesocket(_socket);
        return;
    }

    // 4. 소켓을 IOCP에 연결
    if (CreateIoCompletionPort((HANDLE)_socket, _iocp, (ULONG_PTR)this, 0) == NULL)
    {
        LOG("IOCP bind failed");
        closesocket(_socket);
        return;
    }

    // 5. RecvContext 초기화
    _recvContext = std::make_unique<URecvContext>();

    ZeroMemory(&_recvContext->overlapped, sizeof(OVERLAPPED));
    _recvContext->icopObj = this;
    _recvContext->wsaBuf.buf = _recvContext->buffer;
    _recvContext->wsaBuf.len = sizeof(_recvContext->buffer);
    _recvContext->addrLen = sizeof(_recvContext->addr);

    // 6. 최초 Recv 등록
    RegisterRecv();

    std::string Log = "UDP Server started on port: ";
    Log += std::to_string(port);

    LOG(Log);
}

void CUSession::StartClient(HANDLE Iocp)
{
    _iocp = Iocp;

    _socket = WSASocket(
        AF_INET,
        SOCK_DGRAM,
        IPPROTO_UDP,
        NULL,
        0,
        WSA_FLAG_OVERLAPPED
    ); 
    
    SOCKADDR_IN addr = {};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    addr.sin_port = htons(0); // 자동 포트 할당

    if (bind(_socket, (SOCKADDR*)&addr, sizeof(addr)) == SOCKET_ERROR)
    {
        LOG("Client bind failed");
        closesocket(_socket);
        return;
    }

    if (_socket == INVALID_SOCKET)
    {
        LOG("Socket creation failed");
        return;
    }

    // 소켓을 IOCP에 연결
    if (CreateIoCompletionPort((HANDLE)_socket, _iocp, (ULONG_PTR)this, 0) == NULL)
    {
        LOG("IOCP bind failed");
        closesocket(_socket);
        return;
    }

    // RecvContext 초기화
    _recvContext = std::make_unique<URecvContext>();

    ZeroMemory(&_recvContext->overlapped, sizeof(OVERLAPPED));
    _recvContext->icopObj = this;
    _recvContext->wsaBuf.buf = _recvContext->buffer;
    _recvContext->wsaBuf.len = sizeof(_recvContext->buffer);
    _recvContext->addrLen = sizeof(_recvContext->addr);

    // 서버 응답 받을거면 등록
    RegisterRecv();

    LOG("UDP Client started");
}

void CUSession::RegisterRecv() 
{
    DWORD flags = 0;
    DWORD recvBytes = 0;

    ZeroMemory(&_recvContext->overlapped, sizeof(OVERLAPPED));
    _recvContext->icopObj = this;
    _recvContext->wsaBuf.buf = _recvContext->buffer;
    _recvContext->wsaBuf.len = sizeof(_recvContext->buffer);
    _recvContext->addrLen = sizeof(_recvContext->addr);

    int ret = WSARecvFrom(
        _socket,
        &_recvContext->wsaBuf,
        1,
        &recvBytes,
        &flags,
        (SOCKADDR*)&_recvContext->addr,
        &_recvContext->addrLen,
        &_recvContext->overlapped,
        NULL
    );

    if (ret == SOCKET_ERROR && WSAGetLastError() != WSA_IO_PENDING)
        LOG("RecvFrom Error");
}

void CUSession::ProcessRecv(int bytes)
{
    try
    {
        SOCKADDR_IN clientAddr = _recvContext->addr;

        // WSARecvFrom으로 받은 데이터 바로 처리
        OnRecv(_recvContext->buffer, bytes, clientAddr);

        // 다음 Recv 등록
        RegisterRecv();
    }
    catch (const std::exception& exec)
    {
        LOG(exec.what());
    }
}

void CUSession::SendTo(SOCKADDR_IN& addr, char* data, int len)
{
    USendContext* ctx = new USendContext();
    
    ctx->icopObj = this;
    ctx->wsaBuf.buf = new char[len];
    memcpy(ctx->wsaBuf.buf, data, len);
    ctx->wsaBuf.len = len;
    ctx->addr = addr;
    ctx->addrLen = sizeof(addr);
    
    ZeroMemory(&ctx->overlapped, sizeof(OVERLAPPED));
    
    DWORD sendBytes = 0;
    
    int ret = WSASendTo(
        _socket,
        &ctx->wsaBuf,
        1,
        &sendBytes,
        0,
        (SOCKADDR*)&ctx->addr,
        ctx->addrLen,
        &ctx->overlapped,
        NULL
    );
    
    if (ret == SOCKET_ERROR && WSAGetLastError() != WSA_IO_PENDING)
        LOG("SendTo Error\n");
}