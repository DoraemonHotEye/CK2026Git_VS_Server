#pragma once
#include <mutex>
#include <list>
#include <winsock2.h>
#include <windows.h>
#include "../Headers/CommonTypes.h"

class UIPEndPointManager
{
private:
    static UIPEndPointManager* inst;

private:
    std::mutex mtx;
    std::list<class IPEndPointEntry> endpoints;

private:
    OnUDPSendCallback OnPingSendCallback;

public:
    uint32_t GetEndPointCount();

    static UIPEndPointManager& Instance();

    void RegisterSendCallback(OnUDPSendCallback callbackEvent);

    bool Add(const SOCKADDR_IN& addr);
    bool Remove(const SOCKADDR_IN& addr);

    void OnPingResponse(const SOCKADDR_IN& addr);

    // 주기적 업데이트: Ping 호출, Timeout 제거
    void Update(double pingIntervalSec = 5.0, double timeoutSec = 10.0);
};