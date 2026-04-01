#include "CURequestTable.h"
#include "../../ServerSupports/PrintLogThread.h"

RudpRequestTable::RequestEntry::RequestEntry(uint32_t seqNum, std::chrono::steady_clock::time_point receivedTime)
    : SeqNum(seqNum), ReceivedTime(receivedTime) {
}

RudpRequestTable::RudpRequestTable(int keepAliveMs)
    : _keepAliveMs(keepAliveMs) {
}

bool RudpRequestTable::TryAdd(uint32_t seqNum) {
    auto now = std::chrono::steady_clock::now();

    // 10ms만 기다려보고 안 되면 일단 포기 (Hard Deadlock 방지)
    std::unique_lock<std::recursive_timed_mutex> lock(_mutex, std::defer_lock);
    if (!lock.try_lock_for(std::chrono::milliseconds(10))) {
        return false;
    }

    if (_requestTable.find(seqNum) != _requestTable.end()) return false;
    _requestTable.emplace(seqNum, RequestEntry(seqNum, now));
    return true;
}

bool RudpRequestTable::Contains(uint32_t seqNum) {
    std::unique_lock<std::recursive_timed_mutex> lock(_mutex, std::defer_lock);

    // 조회 작업은 매우 빨라야 하므로 대기 시간을 짧게 잡거나 try_lock 사용 권장
    if (!lock.try_lock_for(std::chrono::milliseconds(1))) {
        return false; // 잠금을 못 얻으면 일단 없다고 판단 (재시도 로직 필요)
    }

    return _requestTable.find(seqNum) != _requestTable.end();
}

bool RudpRequestTable::Remove(uint32_t seqNum) {
    std::unique_lock<std::recursive_timed_mutex> lock(_mutex, std::defer_lock);
    if (!lock.try_lock_for(std::chrono::milliseconds(10))) {
        return false;
    }
    std::string logMsg = "리퀘스트 삭제: SeqNum: ";
    logMsg += std::to_string(seqNum);  // ipEndPoint가 ToString() 제공해야 함
    LOG(logMsg);
    return _requestTable.erase(seqNum) > 0;
}

void RudpRequestTable::Update() {
    auto now = std::chrono::steady_clock::now();

    std::unique_lock<std::recursive_timed_mutex> lock(_mutex, std::defer_lock);

    if (!lock.try_lock_for(std::chrono::milliseconds(10))) {
        return;
    }

    auto it = _requestTable.begin();
    while (it != _requestTable.end()) {
        if ((now - it->second.ReceivedTime) >= _keepAliveMs) {
            // 삭제 전 로그
            std::string logMsg = "리퀘스트 삭제: SeqNum: ";
            logMsg += std::to_string(it->second.SeqNum);  // ipEndPoint가 ToString() 제공해야 함
            LOG(logMsg);

            it = _requestTable.erase(it);
        }
        else {
            ++it;
        }
    }
}

void RudpRequestTable::Clear() {
    std::unique_lock<std::recursive_timed_mutex> lock(_mutex, std::defer_lock);
    if (!lock.try_lock_for(std::chrono::milliseconds(10))) {
        return;
    }
    _requestTable.clear();
}