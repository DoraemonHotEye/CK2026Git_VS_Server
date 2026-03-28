#pragma once
#include <winsock2.h>
#include <mswsock.h>
#include <functional>
#include "../Headers/CommonTypes.h"
#include "../Headers/ServerStaticHeaders.h"
#include <iostream>

class Listener : public IocpObject
{
public:
    void Dispatch(IOContext* context, int bytes)
    {
        std::cout << "[Listener Dispatch]\n";
        if (context->type == IOContext::Type::Accept)
        {
            ProcessAccept(static_cast<AcceptContext*>(context));
        }
    }

public:
    bool Start(SOCKADDR_IN addr, HANDLE iocp, OnAcceptCallback callback);
    void RegisterAccept();

public:
    void ProcessAccept(struct AcceptContext* context);

private:
    SOCKET _listenSocket = INVALID_SOCKET;
    HANDLE _iocpHandle = INVALID_HANDLE_VALUE;

    LPFN_ACCEPTEX _acceptEx = nullptr;
    LPFN_GETACCEPTEXSOCKADDRS _getAcceptExSockAddrs = nullptr;

    OnAcceptCallback _callback;
};