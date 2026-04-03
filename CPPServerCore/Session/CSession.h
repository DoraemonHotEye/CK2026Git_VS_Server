#include <memory>  // unique_ptr
#include <queue>
#include <vector>
#include <mutex>
#include <atomic>
#include <winsock2.h>
#include <windows.h>
#include "../Headers/ServerStaticHeaders.h"
#include "../RecvBuffer/RecvBuffer.h"

// Forward declaration
class PacketInterface;

class CSession : public IocpObject
{
public:
    virtual void Dispatch(IOContext* context, int bytes) override
    {
        if (bytes == 0)
        {
            Disconnect();
            return;
        }

        if (context->type == IOContext::Type::Recv)
            ProcessRecv(bytes);
        else if (context->type == IOContext::Type::Send)
            ProcessSend(bytes);
    }

public:
    int32_t sessionID = -1;

    virtual ~CSession() {}

    virtual void OnConnected(SOCKADDR_IN addr) = 0;
    virtual int OnRecv(char* buffer, int len) = 0;
    virtual void OnSend(int len) = 0;
    virtual void OnDisconnected(SOCKADDR_IN addr) = 0;

    void Start(SOCKET socket, HANDLE iocp);
    void Send(PacketInterface& packet);
    void Disconnect();

    void RegisterRecv();
    void RegisterSend();

    void ProcessRecv(int bytes);
    void ProcessSend(int bytes);

    void Reset();

    std::string GetEndPoint() const;

    SOCKADDR_IN remoteAddr;

protected:
    RecvBuffer _recvBuffer{ 8192 }; // 사이즈 적당히

private:
    SOCKET _socket;
    HANDLE _iocp;

    // 이제 RecvContext, SendContext를 unique_ptr로 관리
    std::unique_ptr<class RecvContext> _recvContext;
    std::unique_ptr<class SendContext> _sendContext;

    // send queue는 vector<char> 그대로
    std::queue<std::vector<uint8_t>> _sendQueue;
    std::vector<char> _pendingBuffer;

    std::atomic<int> _disconnected{ 0 };
    std::mutex _sendLock;
};