#include "CPlayerManager.h"
#include "CPlayer.h"
#include "../../Session/ClientSession.h"

Player* PlayerManager::Generate(ClientSession* session) {
    if (session == nullptr) return nullptr;

    // 1. 객체 생성 (락 밖에서 수행하여 체류 시간 단축)
    Player* newPlayer = new Player();
    newPlayer->_session = session; // Player 내부도 ClientSession* 타입이어야 함

    {
        // 2. 맵 추가 시에만 짧게 락 점유
        std::lock_guard<std::mutex> lock(_lock);
        _playerBinary[session] = newPlayer;
    }

    // 3. 락 해제 후 세션 멤버 수정 (세션 내부 락과 데드락 방지)
    session->player = newPlayer;

    std::cout << "[PlayerManager] == SessionID: " << session->sessionID
        << " :: Player 생성 성공 ::" << std::endl;

    return newPlayer;
}

bool PlayerManager::Remove(ClientSession* session) {
    if (session == nullptr) return false;

    Player* targetPlayer = nullptr;

    {
        // 1. 맵에서 꺼내오는 동안만 락 점유
        std::lock_guard<std::mutex> lock(_lock);
        auto it = _playerBinary.find(session);
        if (it != _playerBinary.end()) {
            targetPlayer = it->second;
            _playerBinary.erase(it); // 맵에서 제거
        }
    }

    // 2. 매니저 락이 완전히 풀린 상태에서 정리 작업 진행
    if (targetPlayer) {
        // 세션과의 연결 고리 제거
        session->player = nullptr;

        std::cout << "[PlayerManager] == SessionID: " << session->sessionID
            << " :: Player 삭제 및 메모리 해제 ::" << std::endl;

        // 3. 실제 메모리 해제 (delete)
        // 여기서 delete를 수행해도 매니저 락을 잡고 있지 않으므로 데드락 안전
        delete targetPlayer;
        return true;
    }

    return false;
}