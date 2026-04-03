#pragma once
#include <string>
#include <vector>
#include <mutex>
#include <iostream>
#include "../../../CPPServerCore/Headers/Packets.h"

class Player;

class Room
{
public:
    Room(unsigned short maxPlayerCount, std::string initHashID);
    ~Room() = default;

    // ÇÙ½É ±â´É
    void BroadCast(PacketInterface& message);
    void Ready();
    void GameStart();
    Room* Enter(class Player* player);
    bool Remove(class Player* player);

    const int GetMasterSessionID();

public:
    std::string RoomHashData;
    unsigned short _MaxPlayerCount;
    int ReadyCount = 0;

private:
    class Player* RoomMaster = nullptr;
    std::vector<class Player*> playerList;
    std::mutex _lock;
};