#pragma once
#include <functional>
#include <mutex>
#include <chrono>
#include "../Algorithm/CPriorityQueue.h"

struct JobTimerElem
{
    int execTick; // 실행 시간 (밀리초)
    std::function<void()> action;

    // C# CompareTo 대신 operator>
    bool operator>(const JobTimerElem& other) const
    {
        // execTick이 작을수록 먼저 실행해야 하므로 반대로 비교
        return execTick < other.execTick;
    }
};

class JobTimer
{
private:
    PriorityQueue<JobTimerElem> _pq;
    std::mutex _lock;

    JobTimer() = default;

public:
    static JobTimer& Instance()
    {
        static JobTimer inst;
        return inst;
    }

    // 작업 등록, tickAfter는 밀리초
    void Push(const std::function<void()>& action, int tickAfter = 0);

    // 실행 가능한 작업 모두 수행
    void Flush();
};