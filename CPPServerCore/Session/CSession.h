#pragma once
#include <winsock2.h>
#include <mswsock.h>
#include <queue>
#include <vector>
#include <atomic>
#include <functional>
#include <mutex>
#include "../Headers/ServerStaticHeaders.h"

class CSession : public IocpObject
{
public:
    virtual void Dispatch(IOContext* context, int bytes) override
    {
        if (context->type == IOContext::Type::Recv)
            ProcessRecv(bytes);
        else
            ProcessSend(bytes);
    }

public:
    virtual ~CSession() {}

    virtual void OnConnected(SOCKADDR_IN addr) = 0;
    virtual int OnRecv(char* buffer, int len) = 0;
    virtual void OnSend(int len) = 0;
    virtual void OnDisconnected() = 0;

    void Start(SOCKET socket, HANDLE iocp);
    void Send(std::vector<char> data);
    void Disconnect();

    void RegisterRecv();
    void RegisterSend();

    void ProcessRecv(int bytes);
    void ProcessSend(int bytes);

private:
    SOCKET _socket;
    HANDLE _iocp;

    RecvContext _recvContext;
    SendContext _sendContext;

    std::queue<std::vector<char>> _sendQueue;
    std::vector<char> _pendingBuffer;

    std::atomic<int> _disconnected;
    std::mutex _sendLock;
};