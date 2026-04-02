#pragma once
#include <unordered_map>
#include <mutex>
#include <iostream>

class Player;
class ClientSession;

class PlayerManager {
public:
    static PlayerManager& GetInstance() {
        static PlayerManager instance;
        return instance;
    }

    // 포인터 기반 생성 및 제거
    Player* Generate(class ClientSession* session);
    bool Remove(class ClientSession* session);

private:
    PlayerManager() = default;
    std::mutex _lock;
    // 키와 값 모두 생포인터로 관리
    std::unordered_map<ClientSession*, Player*> _playerBinary;
};