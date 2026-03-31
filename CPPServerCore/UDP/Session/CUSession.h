#pragma once
#include <memory>  // unique_ptr
#include <queue>
#include <vector>
#include <mutex>
#include <atomic>
#include <winsock2.h>
#include <windows.h>
#include "../../Headers/ServerStaticHeaders.h"
#include "../../RecvBuffer/RecvBuffer.h"

class CUSession : public IocpObject
{
public:
    virtual void Dispatch(IOContext* context, int bytes) override
    {
        if (bytes == 0)
        {
            return;
        }

        if (context->type == IOContext::Type::Recv)
            ProcessRecv(bytes);
        //else if (context->type == IOContext::Type::Send)
        //    ProcessSend(bytes);
    }
    SOCKET _socket;
    HANDLE _iocp;

public:
    void Start(u_short port, HANDLE Iocp);
    void StartClient(HANDLE Iocp);
    void RegisterRecv();
    void ProcessRecv(int bytes);
    void SendTo(SOCKADDR_IN& addr, char* data, int len);
    void OnRecvFrom(char* buffer, int len, SOCKADDR_IN& addr);

private:
    std::unique_ptr<class URecvContext> _recvContext;
};