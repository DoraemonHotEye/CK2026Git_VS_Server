#include "UIPEndPointManager.h"
#include <algorithm>
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h> 
#include "../IPEndPointEntry/IPEndPointEntry.h"
#include "../ServerSupports/PrintLogThread.h"

UIPEndPointManager* UIPEndPointManager::inst = nullptr;

UIPEndPointManager& UIPEndPointManager::Instance()
{
    if (!inst)
        inst = new UIPEndPointManager();
    return *inst;
}

void UIPEndPointManager::RegisterSendCallback(OnUDPSendCallback callbackEvent)
{
    OnPingSendCallback = callbackEvent;
}

bool UIPEndPointManager::Add(const SOCKADDR_IN& addr)
{
    std::lock_guard<std::mutex> lock(mtx);
    auto it = std::find_if(endpoints.begin(), endpoints.end(),
        [&addr](const IPEndPointEntry& e)
        {
            return e.Address.sin_addr.S_un.S_addr == addr.sin_addr.S_un.S_addr &&
                e.Address.sin_port == addr.sin_port;
        });

    if (it != endpoints.end())
        return false;

    endpoints.emplace_back(addr);
    return true;
}

bool UIPEndPointManager::Remove(const SOCKADDR_IN& addr)
{
    std::lock_guard<std::mutex> lock(mtx);

    auto it = std::find_if(endpoints.begin(), endpoints.end(),
        [&addr](const IPEndPointEntry& e)
        {
            return e.Address.sin_addr.S_un.S_addr == addr.sin_addr.S_un.S_addr &&
                e.Address.sin_port == addr.sin_port;
        });

    if (it != endpoints.end())
    {
        endpoints.erase(it);

        char ip[INET_ADDRSTRLEN] = { 0 };
        inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

        std::string logMsg = "UIPEndPoint 삭제: IP = ";
        logMsg += ip;
        logMsg += ", Port = ";
        logMsg += std::to_string(ntohs(it->Address.sin_port));

        LOG(logMsg); // PrintLogThread.h의 LOG 사용

        return true;
    }
    return false;
}

void UIPEndPointManager::OnPingResponse(const SOCKADDR_IN& addr)
{
    std::lock_guard<std::mutex> lock(mtx);

    for (auto& e : endpoints)
    {
        if (e.Address.sin_addr.S_un.S_addr == addr.sin_addr.S_un.S_addr &&
            e.Address.sin_port == addr.sin_port)
        {
            e.OnResponse();
            return;
        }
    }
}

void UIPEndPointManager::Update(double pingIntervalSec, double timeoutSec)
{
    std::vector<SOCKADDR_IN> toPing;

    {
        std::lock_guard<std::mutex> lock(mtx);

        for (auto it = endpoints.begin(); it != endpoints.end(); )
        {
            double sinceResponse = it->GetSinceLastResponse();
            double sincePing = it->GetSinceLastPing();

            if (sinceResponse >= timeoutSec)
            {
                char ip[INET_ADDRSTRLEN] = { 0 };
                inet_ntop(AF_INET, &it->Address.sin_addr, ip, sizeof(ip));

                std::string logMsg = "UIPEndPoint 삭제: IP = ";
                logMsg += ip;
                logMsg += ", Port = ";
                logMsg += std::to_string(ntohs(it->Address.sin_port));

                LOG(logMsg); // PrintLogThread.h의 LOG 사용

                it = endpoints.erase(it);
                continue;
            }

            if (sincePing >= pingIntervalSec && OnPingSendCallback)
            {
                toPing.push_back(it->Address);
                // 여기서는 락 안에서 호출하지 않고
                // Ping 상태 갱신만 수행
                it->OnPingSent(); // 예: 내부 시각만 갱신
            }

            ++it;
        }
    }

    // 락 해제 후 외부 콜백 호출
    for (auto& addr : toPing)
    {
        OnPingSendCallback(addr);
    }
}