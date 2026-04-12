#include "CRoomManager.h"
#include "CRoom.h" // Room의 Enter 등을 호출하기 위해 필요

int RoomManager::GetCount() {
    std::lock_guard<std::mutex> lock(_lock);
    return static_cast<int>(_roomList.size());
}

std::vector<std::string> RoomManager::GetRoomHashList() {
    std::vector<std::string> hashList;

    {
        std::lock_guard<std::mutex> lock(_lock);
        // C++11/14 스타일: pair의 first가 key(hash), second가 value(room)입니다.
        for (auto it = _roomList.begin(); it != _roomList.end(); ++it) {
            hashList.push_back(it->first);
        }
    }

    return hashList;
}

bool RoomManager::Remove(Room* room) {
    if (room == nullptr) return false;

    std::string hash = room->RoomHashData;
    bool success = false;

    {
        std::lock_guard<std::mutex> lock(_lock);
        auto it = _roomList.find(hash);
        if (it != _roomList.end()) {
            _roomList.erase(it);
            success = true;
        }
    }

    if (success) {
        std::cout << "[RoomManager]:: " << hash << " Deleted From Memory" << std::endl;
        // 주의: Room의 소멸 책임이 Manager에게 있다면 여기서 delete 수행
        // 만약 다른 곳에서 관리한다면 delete 로직을 상황에 맞게 배치하세요.
        delete room;
    }

    return success;
}

Room* RoomManager::GenerateSingle(unsigned short maxPlayerCount) {
    // 1. 락 밖에서 객체 생성 (락 체류 시간 최소화)
    Room* newRoom = new Room(maxPlayerCount, CreateDateHashTag());
    std::string hash = newRoom->RoomHashData;

    {
        std::lock_guard<std::mutex> lock(_lock);
        _roomList[hash] = newRoom;
    }

    return newRoom;
}

Room* RoomManager::GenerateAndJoin(Player* player, unsigned short maxPlayerCount) {
    if (player == nullptr) return nullptr;

    // 1. 락 밖에서 방 생성
    Room* newRoom = new Room(maxPlayerCount, CreateDateHashTag());

    // 2. [데드락 방지] Manager 락을 잡기 전에 Room 조작을 먼저 함
    // (만약 Enter 내부에서 또 다른 락을 잡더라도 Manager 락과는 무관함)
    newRoom->Enter(player);

    {
        // 3. 최소한의 범위에서만 Manager 락 점유
        std::lock_guard<std::mutex> lock(_lock);
        _roomList[newRoom->RoomHashData] = newRoom;
    }

    return newRoom;
}

Room* RoomManager::GetRoom(const std::string& hashID) {
    std::lock_guard<std::mutex> lock(_lock);
    auto it = _roomList.find(hashID);
    if (it != _roomList.end()) {
        return it->second;
    }
    return nullptr;
}

std::string RoomManager::CreateDateHashTag() {
    auto now = std::time(nullptr);
    struct tm tm_struct; // 결과를 담을 구조체

    // Windows용 localtime_s (인자 순서: 결과주소, 시간주소)
    if (localtime_s(&tm_struct, &now) != 0) {
        return "#ERROR_TIME";
    }

    std::ostringstream oss;
    oss << "#" << std::put_time(&tm_struct, "%Y%m%d%H%M%S");
    return oss.str();
}