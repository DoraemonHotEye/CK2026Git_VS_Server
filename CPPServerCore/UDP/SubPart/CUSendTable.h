#pragma once

#include <unordered_map>
#include <vector>
#include <mutex>
#include <chrono>
#include <memory>
#include <cstdint>
#include <winsock2.h>

#include "IRdupCallback.h"

// PacketInterface 전방 선언
class PacketInterface;

class RudpSendTable
{
private:
    struct SendEntry
    {
        uint32_t seqNum = 0;
        std::vector<uint8_t> buffer;  // 패킷 데이터 복사본
        SOCKADDR_IN addr{};
        int64_t lastSendTimeMs = 0;
        int retryCount = 0;
        int retryIntervalMs = 0;
        IRudpCallback* callback = nullptr;

        SendEntry() = default;

        SendEntry(uint32_t s,
            const std::vector<uint8_t>& buf,
            const SOCKADDR_IN& a,
            int64_t now,
            int interval,
            IRudpCallback* cb)
            : seqNum(s),
            buffer(buf),
            addr(a),
            lastSendTimeMs(now),
            retryCount(0),
            retryIntervalMs(interval),
            callback(cb)
        {
        }
    };

    std::unordered_map<uint32_t, SendEntry> sendTable;
    std::mutex mtx;
    std::chrono::steady_clock::time_point startTime;

    int64_t NowMs() const;

public:
    RudpSendTable();
    ~RudpSendTable() = default;

    void Add(uint32_t seqNum,
        const std::vector<uint8_t>& buffer,
        const SOCKADDR_IN& addr,
        IRudpCallback* callback,
        int retryIntervalMs = 500);

    bool Ack(uint32_t seqNum);

    void Update();
};