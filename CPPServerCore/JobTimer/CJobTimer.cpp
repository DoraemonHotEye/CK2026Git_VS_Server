#include "CJobTimer.h"
#include <chrono>

using namespace std::chrono;

void JobTimer::Push(const std::function<void()>& action, int tickAfter)
{
    JobTimerElem job;
    auto now = duration_cast<milliseconds>(system_clock::now().time_since_epoch()).count();
    job.execTick = static_cast<int>(now) + tickAfter;
    job.action = action;

    std::lock_guard<std::mutex> lock(_lock);
    _pq.Push(job);
}

void JobTimer::Flush()
{
    while (true)
    {
        JobTimerElem job;
        {
            std::lock_guard<std::mutex> lock(_lock);
            if (_pq.Empty())
                break;

            job = _pq.Peek();
            auto now = static_cast<int>(duration_cast<milliseconds>(system_clock::now().time_since_epoch()).count());

            if (job.execTick > now)
                break;

            _pq.Pop();
        }

        // lock 밖에서 실행
        job.action();
    }
}