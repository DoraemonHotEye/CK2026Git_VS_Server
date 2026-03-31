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
    virtual void OnSend(int len) = 0;
    virtual int OnRecv(char* buffer, int len, SOCKADDR_IN& addr) = 0;

    void Start(u_short port, HANDLE Iocp);
    void StartClient(HANDLE Iocp);

private:
    void RegisterRecv();
    void ProcessRecv(int bytes);
    
public:
    void SendTo(SOCKADDR_IN& addr, char* data, int len);

protected:
    RecvBuffer _recvBuffer{ 8192 }; // 사이즈 적당히

private:
    std::unique_ptr<class URecvContext> _recvContext;
};