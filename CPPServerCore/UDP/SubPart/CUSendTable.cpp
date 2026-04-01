#include "CUSendTable.h"
#include <algorithm>
#include "../../ServerSupports/PrintLogThread.h"

// ------------------- 생성자 -------------------
RudpSendTable::RudpSendTable()
{
    startTime = std::chrono::steady_clock::now();
}

// ------------------- 현재 시간(ms) 계산 -------------------
int64_t RudpSendTable::NowMs() const
{
    auto now = std::chrono::steady_clock::now();
    return std::chrono::duration_cast<std::chrono::milliseconds>(now - startTime).count();
}

// ------------------- Add -------------------
void RudpSendTable::Add(uint32_t seqNum,
    const std::vector<uint8_t>& buffer,
    const SOCKADDR_IN& addr,
    IRudpCallback* callback,
    int retryIntervalMs)
{
    std::lock_guard<std::mutex> lock(mtx);
    sendTable[seqNum] = SendEntry(seqNum, buffer, addr, NowMs(), retryIntervalMs, callback);
}

// ------------------- Ack -------------------
bool RudpSendTable::Ack(uint32_t seqNum)
{
    std::lock_guard<std::mutex> lock(mtx);
    
    bool succ = sendTable.erase(seqNum) > 0;
    
    if (succ == true)
    {
        std::string ss;
        ss += "ACK:: => ";
        ss += std::to_string(seqNum);

        LOG(ss);
    }
    return succ;
}

// ------------------- Update -------------------
void RudpSendTable::Update()
{
    std::vector<SendEntry> resendListCopy; // 포인터 대신 복사본 사용
    int64_t nowMs = NowMs();

    {
        std::lock_guard<std::mutex> lock(mtx);
        for (auto& kv : sendTable)
        {
            SendEntry& entry = kv.second;

            if (nowMs - entry.lastSendTimeMs >= entry.retryIntervalMs)
            {
                entry.lastSendTimeMs = nowMs;
                entry.retryCount++;

                // 포인터 대신 SendEntry 복사본을 저장
                resendListCopy.push_back(entry);
            }
        }
    }

    // lock 밖에서 콜백 호출
    for (auto& entry : resendListCopy)
    {
        if (entry.callback)
        {
            entry.callback->OnResend(
                entry.buffer,
                entry.seqNum,
                entry.addr
            );
        }
    }
}