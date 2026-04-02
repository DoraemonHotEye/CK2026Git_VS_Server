#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
#include <mutex>
#include <ctime>
#include <iomanip>
#include <sstream>

class Room;
class Player;

class RoomManager
{
public:
    static RoomManager& GetInstance() {
        static RoomManager instance;
        return instance;
    }

    // 방 제거 (데드락 방지)
    bool Remove(Room* room);

    // 방 생성 (참조만 반환)
    Room* GenerateSingle(unsigned short maxPlayerCount);

    // 방 생성 및 플레이어 입장 (데드락 방지)
    Room* GenerateAndJoin(Player* player, unsigned short maxPlayerCount);

    // 방 찾기
    Room* GetRoom(const std::string& hashID);

    // 해시 리스트 가져오기
    std::vector<std::string> GetRoomHashList();

    int GetCount();

    static std::string CreateDateHashTag();

private:
    RoomManager() = default;
    ~RoomManager() = default;

    std::mutex _lock;
    std::unordered_map<std::string, Room*> _roomList;
};