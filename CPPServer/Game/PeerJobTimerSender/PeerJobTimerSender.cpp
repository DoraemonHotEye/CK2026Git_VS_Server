#include "PeerJobTimerSender.h"
#include "PeerStationPackage.h"

PeerJobTimerSender& PeerJobTimerSender::Instance()
{
    static PeerJobTimerSender inst;
    return inst;
}

void PeerJobTimerSender::RegisterSender(std::shared_ptr<PeerStationPackage> sender)
{
    std::lock_guard<std::mutex> lock(mtx);
    PeerPackages.push_back(sender);
}

void PeerJobTimerSender::BroadCast()
{
    std::vector<std::shared_ptr<PeerStationPackage>> destroyList;

    {
        std::lock_guard<std::mutex> lock(mtx);
        for (auto& peer : PeerPackages)
        {
            bool succ = peer->BroadCast();
            if (succ)
                destroyList.push_back(peer);
        }
    }

    // 락 밖에서 제거
    for (auto& peer : destroyList)
    {
        std::lock_guard<std::mutex> lock(mtx);
        auto it = std::find(PeerPackages.begin(), PeerPackages.end(), peer);
        if (it != PeerPackages.end())
            PeerPackages.erase(it);
    }
}