#pragma once
#include <unordered_map>
#include <vector>
#include <mutex>
#include <chrono>
#include <cstdint>

class RudpRequestTable {
private:
    struct RequestEntry {
        uint32_t SeqNum;
        std::chrono::steady_clock::time_point ReceivedTime;

        RequestEntry(uint32_t seqNum, std::chrono::steady_clock::time_point receivedTime);
    };

    std::unordered_map<uint32_t, RequestEntry> _requestTable;
    std::recursive_timed_mutex _mutex;
    std::chrono::milliseconds _keepAliveMs;

public:
    explicit RudpRequestTable(int keepAliveMs = 5000);

    bool TryAdd(uint32_t seqNum);
    bool Contains(uint32_t seqNum);
    bool Remove(uint32_t seqNum);
    void Update();
    void Clear();
};