#pragma once
#include <vector>
#include <memory>
#include <mutex>

class PeerStationPackage;

class PeerJobTimerSender
{
public:
    static PeerJobTimerSender& Instance();

    void RegisterSender(std::shared_ptr<PeerStationPackage> sender);
    void BroadCast();

private:
    PeerJobTimerSender() = default;
    ~PeerJobTimerSender() = default;

    std::vector<std::shared_ptr<PeerStationPackage>> PeerPackages;
    std::mutex mtx;
};