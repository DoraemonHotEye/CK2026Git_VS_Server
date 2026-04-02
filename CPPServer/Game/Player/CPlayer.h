#pragma once
#include <string>
#include <memory>
#include <WinSock2.h>

// 전방 선언 (Circular Dependency 방지)
class ClientSession;
class Room;

class Player {
public:
    Player() : _peerEndPoint({}) {}
    ~Player() = default;

public:
    SOCKADDR_IN _peerEndPoint;               // C#의 IPEndPoint 대응
    ClientSession* _session;  // 소유권 공유
    std::string _localAddress;
    Room* CurrentRoom;
};