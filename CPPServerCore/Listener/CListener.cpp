#include "CListener.h"
#include "../Session/CSession.h"
#include <iostream>
#include <thread>

bool Listener::Start(SOCKADDR_IN addr, HANDLE iocp, OnAcceptCallback callback)
{
    _callback = callback;
    _iocpHandle = iocp;

    _listenSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);

    bind(_listenSocket, (SOCKADDR*)&addr, sizeof(addr));
    listen(_listenSocket, SOMAXCONN);

    CreateIoCompletionPort((HANDLE)_listenSocket, _iocpHandle, (ULONG_PTR)this, 0);

    // AcceptEx
    GUID guidAcceptEx = WSAID_ACCEPTEX;
    DWORD bytes = 0;
    WSAIoctl(_listenSocket, SIO_GET_EXTENSION_FUNCTION_POINTER,
        &guidAcceptEx, sizeof(guidAcceptEx),
        &_acceptEx, sizeof(_acceptEx),
        &bytes, NULL, NULL);

    // GetAcceptExSockAddrs ⭐ 반드시 필요
    GUID guidAddr = WSAID_GETACCEPTEXSOCKADDRS;
    WSAIoctl(_listenSocket, SIO_GET_EXTENSION_FUNCTION_POINTER,
        &guidAddr, sizeof(guidAddr),
        &_getAcceptExSockAddrs, sizeof(_getAcceptExSockAddrs),
        &bytes, NULL, NULL);

    for (int i = 0; i < 10; i++)
        RegisterAccept();

    return true;
}

void Listener::RegisterAccept()
{
    AcceptContext* context = new AcceptContext();

    ZeroMemory(&context->overlapped, sizeof(OVERLAPPED));

    context->acceptSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);

    DWORD bytesReceived = 0;

    BOOL ret = _acceptEx(
        _listenSocket,
        context->acceptSocket,
        context->buffer,
        0,
        sizeof(SOCKADDR_IN) + 16,
        sizeof(SOCKADDR_IN) + 16,
        &bytesReceived,
        &context->overlapped
    );

    if (ret == FALSE)
    {
        int err = WSAGetLastError();
        if (err != ERROR_IO_PENDING)
        {
            std::cout << "AcceptEx failed: " << err << std::endl;
            delete context;
        }
    }
}

void Listener::ProcessAccept(AcceptContext* context)
{
    SOCKET clientSocket = context->acceptSocket;

    //필수
    setsockopt(clientSocket, SOL_SOCKET, SO_UPDATE_ACCEPT_CONTEXT,
        (char*)&_listenSocket, sizeof(_listenSocket));

    if (_callback)
    {
        CSession* session = _callback(clientSocket);

        session->Start(clientSocket, _iocpHandle);

        SOCKADDR* localAddr = nullptr;
        SOCKADDR* remoteAddr = nullptr;
        int localLen = 0;
        int remoteLen = 0;

        _getAcceptExSockAddrs(
            context->buffer,
            0,
            sizeof(SOCKADDR_IN) + 16,
            sizeof(SOCKADDR_IN) + 16,
            &localAddr,
            &localLen,
            &remoteAddr,
            &remoteLen
        );

        SOCKADDR_IN* clientAddr = reinterpret_cast<SOCKADDR_IN*>(remoteAddr);

        session->OnConnected(*clientAddr);
    }

    //여기서 삭제 (WorkerThread에서 삭제하면 안됨)
    delete context;

    //다음 Accept
    RegisterAccept();
}