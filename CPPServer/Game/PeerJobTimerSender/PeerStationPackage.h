#pragma once
#include <string>
#include <vector>
#include <queue>
#include <memory>
#include <mutex>
#include <optional>
#include <cstdint>
#include "../../../CPPServerCore/Headers/Packets.h"

class Player;

struct PeerDataPack
{
    std::string PublicAddress;
    std::string LocalAddress;
    int SessionID = -1;
    uint16_t Port = 0;
};

class PeerStationPackage
{
public:
    std::string HashRoomID;

    PeerStationPackage() = default;
    ~PeerStationPackage() = default;

    void EnterPlayers(const std::vector<Player*>& players);
    void EnterPeers(const std::vector<PeerDataPack>& peers);

    // Lockless 호출 가능
    bool BroadCast();

private:
    void BroadCastMessage(PacketInterface& message);

    std::vector<Player*> BroadCastList;
    std::queue<PeerDataPack> SendIPEndPointContainer;
    mutable std::mutex mtx; // 내부 동기화
};