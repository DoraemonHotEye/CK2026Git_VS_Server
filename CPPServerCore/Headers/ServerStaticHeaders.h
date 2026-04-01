#pragma once
#include <minwinbase.h>
#include <unordered_map>
#include <WinSock2.h>
#include <ws2tcpip.h>
#include <memory>

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

struct URecvContext : public IOContext
{
    WSABUF wsaBuf;
    char buffer[1024];
    SOCKADDR_IN addr;
    int addrLen = sizeof(addr);

    URecvContext()
    {
        ZeroMemory(&overlapped, sizeof(OVERLAPPED));
        type = Type::Recv;
        wsaBuf.buf = buffer;
        wsaBuf.len = sizeof(buffer);
    }
};

struct USendContext : public IOContext
{
    WSABUF wsaBuf;
    SOCKADDR_IN addr;
    int addrLen = sizeof(addr);

    USendContext()
    {
        ZeroMemory(&overlapped, sizeof(OVERLAPPED));
        type = Type::Send;
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

struct SockAddrHash {
    size_t operator()(const SOCKADDR_IN& addr) const {
        return std::hash<uint32_t>()(addr.sin_addr.S_un.S_addr) ^ std::hash<uint16_t>()(addr.sin_port);
    }
};

struct SockAddrEqual {
    bool operator()(const SOCKADDR_IN& lhs, const SOCKADDR_IN& rhs) const {
        return lhs.sin_addr.S_un.S_addr == rhs.sin_addr.S_un.S_addr &&
            lhs.sin_port == rhs.sin_port;
    }
};