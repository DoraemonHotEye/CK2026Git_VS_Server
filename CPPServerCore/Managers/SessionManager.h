#pragma once
#include <iostream>
#include <list>
#include <queue>
#include <memory>
#include <mutex>
#include <functional>

class SessionManager
{
private:
    int32_t sessionCount = 0;

    static SessionManager* inst;

    std::list<std::unique_ptr<class CSession>> sessions;
    std::queue<std::unique_ptr<class CSession>> sessionPool;
    std::mutex mtx;

    SessionManager() = default;

public:
    static SessionManager& Instance();
    void AddSession(std::unique_ptr<class CSession> session);
    void RemoveSession(class CSession* session);

    class CSession* Get(int SessionID);
    std::vector<std::string> GetEndPointsOrderBy(int step = 0, int step2 = 0);
    std::vector<std::string> GetEndPointCount(int c1 = 0, int c2 = 0);

    std::unique_ptr<CSession> AcquireSession(std::function<std::unique_ptr<CSession>()> creator)
    {
        std::lock_guard<std::mutex> lock(mtx);

        if (sessionPool.empty())
        {
            // creator 함수로 구체 클래스 생성
            return creator();
        }
        else
        {
            auto session = std::move(sessionPool.front());
            sessionPool.pop();
            return session;
        }
    }

    template <typename Func>
    void ForEach(Func func)
    {
        std::lock_guard<std::mutex> lock(mtx);
        for (auto& session : sessions)
        {
            func(session.get());
        }
    }

    size_t PoolSize()
    {
        std::lock_guard<std::mutex> lock(mtx);
        return sessionPool.size();
    }
};

