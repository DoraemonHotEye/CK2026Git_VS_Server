#include "SessionManager.h"
#include "../Session/CSession.h"

SessionManager* SessionManager::inst;

SessionManager& SessionManager::Instance()
{
    if (inst == nullptr)
        inst = new SessionManager();

    return *inst;
}

void SessionManager::AddSession(std::unique_ptr<CSession> session)
{
    std::lock_guard<std::mutex> lock(mtx);
    sessions.push_back(std::move(session));
}

void SessionManager::RemoveSession(CSession* session)
{
    std::lock_guard<std::mutex> lock(mtx);
    auto it = std::find_if(sessions.begin(), sessions.end(),
        [session](const std::unique_ptr<CSession>& ptr) { return ptr.get() == session; });

    if (it != sessions.end())
    {
        // 세션 풀로 이동
        sessionPool.push(std::move(*it));
        sessions.erase(it);
    }
}
