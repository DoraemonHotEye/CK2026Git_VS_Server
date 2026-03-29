#pragma once
#include <minwinbase.h>
#include <WinSock2.h>

class IocpObject;

struct IOContext
{
    OVERLAPPED overlapped;
    IocpObject* icopObj;

    enum class Type
    {
        Recv,
        Send,
        Accept,
        Connect
    } type;
};

struct AcceptContext : public IOContext
{
    SOCKET acceptSocket;
    char buffer[(sizeof(SOCKADDR_IN) + 16) * 2];

    AcceptContext()
    {
        ZeroMemory(buffer, sizeof(buffer));
        type = IOContext::Type::Accept;
    }
};

struct RecvContext : public IOContext
{
    WSABUF wsaBuf;
    char buffer[65535];

    RecvContext()
    {
        ZeroMemory(&overlapped, sizeof(OVERLAPPED));
        type = Type::Recv;
        wsaBuf.buf = buffer;
        wsaBuf.len = sizeof(buffer);
    }
};

struct SendContext : public IOContext
{
    WSABUF wsaBuf;

    SendContext()
    {
        ZeroMemory(&overlapped, sizeof(OVERLAPPED));
        type = Type::Send;
    }
};

struct ConnectContext : public IOContext
{
    SOCKET socket;

    ConnectContext()
    {
        ZeroMemory(&overlapped, sizeof(OVERLAPPED));
        type = Type::Connect;
        socket = INVALID_SOCKET;
    }
};

class IocpObject
{
public:
    virtual void Dispatch(IOContext* context, int bytes) = 0;
};