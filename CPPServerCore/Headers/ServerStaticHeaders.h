#pragma once
#include <minwinbase.h>
#include <WinSock2.h>

struct IOContext
{
    OVERLAPPED overlapped;

    enum class Type
    {
        Recv,
        Send,
        Accept
    } type;
};

struct AcceptContext : public IOContext
{
    SOCKET acceptSocket;
    char buffer[(sizeof(SOCKADDR_IN) + 16) * 2];

    AcceptContext()
    {
        type = IOContext::Type::Accept;
        ZeroMemory(buffer, sizeof(buffer));
    }
};

struct RecvContext : public IOContext
{
    WSABUF wsaBuf;
    char buffer[65535];

    RecvContext()
    {
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
        type = Type::Send;
    }
};

struct ConnectContext
{
    OVERLAPPED overlapped;
    SOCKET socket;

    ConnectContext()
    {
        ZeroMemory(this, sizeof(ConnectContext));
    }
};

class IocpObject
{
public:
    virtual void Dispatch(IOContext* context, int bytes) = 0;
};