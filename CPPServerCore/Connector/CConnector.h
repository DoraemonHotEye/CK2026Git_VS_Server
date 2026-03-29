#pragma once
#include <winsock2.h>
#include <mswsock.h>
#include <functional>
#include "../Headers/CommonTypes.h"
#include "../Headers/ServerStaticHeaders.h"
#include <iostream>

class Connector : public IocpObject
{
public:
    void Dispatch(IOContext* context, int bytes)
    {
        if (context->type == IOContext::Type::Connect)
        {
            ProcessConnect(reinterpret_cast<ConnectContext*>(context));
        }
    }

public:
    bool Start(SOCKADDR_IN addr, HANDLE iocp, OnConnectCallback callback, int count = 1);

private:
    void RegisterConnect();
    void ProcessConnect(class ConnectContext* context);

private:
    HANDLE _iocpHandle = nullptr;
    SOCKADDR_IN _addr;
    OnConnectCallback _callback;

    LPFN_CONNECTEX _connectEx = nullptr;
};