#include "CConnector.h"
#include "../Session/CSession.h"
#include <iostream>

bool Connector::Start(SOCKADDR_IN addr, HANDLE iocp, OnConnectCallback callback, int count)
{
    _addr = addr;
    _iocpHandle = iocp;
    _callback = callback;

    // 더미 소켓 (ConnectEx 얻기용)
    SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);

    GUID guid = WSAID_CONNECTEX;
    DWORD bytes = 0;

    WSAIoctl(
        sock,
        SIO_GET_EXTENSION_FUNCTION_POINTER,
        &guid,
        sizeof(guid),
        &_connectEx,
        sizeof(_connectEx),
        &bytes,
        NULL,
        NULL
    );

    closesocket(sock);

    // 여러 개 연결 시도
    for (int i = 0; i < count; i++)
    {
        RegisterConnect();
    }

    return true;
}

void Connector::RegisterConnect()
{
    ConnectContext* context = new ConnectContext();
    context->icopObj = this;
    context->socket = socket(AF_INET, SOCK_STREAM, 0);

    // 반드시 bind 필요 (ConnectEx 규칙)
    SOCKADDR_IN localAddr = {};
    localAddr.sin_family = AF_INET;
    bind(context->socket, (SOCKADDR*)&localAddr, sizeof(localAddr));

    // IOCP 연결
    CreateIoCompletionPort((HANDLE)context->socket, _iocpHandle, (ULONG_PTR)this, 0);

    BOOL ret = _connectEx(
        context->socket,
        (SOCKADDR*)&_addr,
        sizeof(_addr),
        nullptr,
        0,
        nullptr,
        &context->overlapped
    );

    if (ret == FALSE)
    {
        if (WSAGetLastError() != ERROR_IO_PENDING)
        {
            std::cout << "ConnectEx failed\n";
            delete context;
        }
    }
}

void Connector::ProcessConnect(ConnectContext* context)
{
    SOCKET sock = context->socket;
    setsockopt(sock, SOL_SOCKET, SO_UPDATE_CONNECT_CONTEXT, NULL, 0);

    if (_callback)
    {
        CSession* session = _callback(sock);

        session->Start(sock, _iocpHandle);

        SOCKADDR_IN addr;
        int len = sizeof(addr);
        getpeername(sock, (SOCKADDR*)&addr, &len);

        session->OnConnected(addr);
    }

    delete context;
}
