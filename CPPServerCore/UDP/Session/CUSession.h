#pragma once
#include <winsock2.h>
#include <windows.h>
#include <thread>
#include <atomic>
#include <mutex>
#include <unordered_map>
#include "../SubPart/CUSendTable.h"
#include "../SubPart/CURequestTable.h"
#include "../../Headers/ServerStaticHeaders.h"
// ... (기타 include 동일)

class CUSession : public IRudpCallback
{
public:
    CUSession() : _socket(INVALID_SOCKET), _stopThread(false) {}
    virtual ~CUSession() { Stop(); }

    // RUDP 재전송 콜백
    virtual void OnResend(const std::vector<uint8_t>& buffer, uint32_t seqNum, SOCKADDR_IN& addr) override
    {
        SendTo(addr, reinterpret_cast<const char*>(buffer.data()), (int)buffer.size());
    }

    // 소켓 시작 및 수신 스레드 실행
    void Start(u_short port);
    void StartClient(const std::string& serverIP, u_short port);
    void Stop();

    virtual void OnSend(int len, SOCKADDR_IN& addr) = 0;
    virtual int OnRecv(char* buffer, int len, SOCKADDR_IN& addr) = 0;

    void SendTo(SOCKADDR_IN& addr, const char* data, int len);
    void Send(class PacketInterface& packet, SOCKADDR_IN& addr, bool isUseRUDP = false);
    void Update();

protected:
    virtual void RegisterUpdateMethod() { }
    SOCKADDR_IN _serverAddr;

protected:
    SOCKET _socket;
    std::thread _recvThread;
    std::atomic<bool> _stopThread;
    std::atomic<uint16_t> _sendSeq{ 0 };

    // 멀티스레드 보호용 뮤텍스
    std::mutex _tableMutex;

public:
    RudpSendTable sendTable;
    std::unordered_map<SOCKADDR_IN, std::shared_ptr<RudpRequestTable>, SockAddrHash, SockAddrEqual> RequestTables;

private:
    void ReceiveLoop(); // 수신 전용 스레드 함수
};