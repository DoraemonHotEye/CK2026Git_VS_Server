#include "SessionManager.h"
#include <algorithm>
#include <sstream>
#include <vector>
#include <string>
#include <map>
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
    session.get()->sessionID = sessionCount++;
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

std::vector<std::string> SessionManager::GetEndPointsOrderBy(int step, int step2)
{
    std::lock_guard<std::mutex> lock(mtx);
    std::vector<std::string> endpoints;

    // 1️ 전체 세션 복사
    std::vector<CSession*> tempSessions;
    for (auto& s : sessions)
        tempSessions.push_back(s.get());

    // 2정렬 처리 (step, step2 사용)
    // step = 1 → IP 오름차순, step = 2 → Port 오름차순
    if (step != 0 || step2 != 0)
    {
        std::sort(tempSessions.begin(), tempSessions.end(),
            [step, step2](CSession* a, CSession* b)
            {
                if (!a || !b) return false;
                // step 처리
                if (step == 1) // IP
                {
                    uint32_t ipA = ntohl(a->remoteAddr.sin_addr.S_un.S_addr);
                    uint32_t ipB = ntohl(b->remoteAddr.sin_addr.S_un.S_addr);
                    if (ipA != ipB) return ipA < ipB;
                }
                else if (step == 2) // Port
                {
                    uint16_t portA = ntohs(a->remoteAddr.sin_port);
                    uint16_t portB = ntohs(b->remoteAddr.sin_port);
                    if (portA != portB) return portA < portB;
                }

                // step2 처리
                if (step2 == 1) // IP
                {
                    uint32_t ipA = ntohl(a->remoteAddr.sin_addr.S_un.S_addr);
                    uint32_t ipB = ntohl(b->remoteAddr.sin_addr.S_un.S_addr);
                    if (ipA != ipB) return ipA < ipB;
                }
                else if (step2 == 2) // Port
                {
                    uint16_t portA = ntohs(a->remoteAddr.sin_port);
                    uint16_t portB = ntohs(b->remoteAddr.sin_port);
                    if (portA != portB) return portA < portB;
                }

                return false;
            });
    }

    // 3️문자열로 변환
    for (auto s : tempSessions)
    {
        char ip[INET_ADDRSTRLEN];
        inet_ntop(AF_INET, &s->remoteAddr.sin_addr, ip, sizeof(ip));
        uint16_t port = ntohs(s->remoteAddr.sin_port);

        std::ostringstream oss;
        oss << ip << ":" << port;
        endpoints.push_back(oss.str());
    }

    return endpoints;
}

CSession* SessionManager::Get(int sessionID)
{
    std::lock_guard<std::mutex> lock(mtx);

    for (auto& session : sessions)
    {
        if (session->sessionID == sessionID)
            return session.get(); // unique_ptr에서 raw pointer 반환
    }

    return nullptr; // 해당 ID 없으면 nullptr
}

std::vector<std::string> SessionManager::GetEndPointCount(int startIndex, int count)
{
    std::lock_guard<std::mutex> lock(mtx);
    std::vector<std::string> result;

    if (sessions.empty())
        return result;

    // 리스트 크기 계산
    int size = static_cast<int>(sessions.size());
    if (startIndex >= size)
        return result; // 범위 벗어나면 빈 벡터

    int endIndex = startIndex + count;
    if (endIndex > size)
        endIndex = size;

    // iterator 이동
    auto it = sessions.begin();
    std::advance(it, startIndex);

    for (int i = startIndex; i < endIndex; ++i, ++it)
    {
        CSession* s = it->get();

        char ip[INET_ADDRSTRLEN];
        inet_ntop(AF_INET, &s->remoteAddr.sin_addr, ip, sizeof(ip));
        uint16_t port = ntohs(s->remoteAddr.sin_port);

        std::ostringstream oss;
        oss << ip << ":" << port;

        result.push_back(oss.str());
    }

    return result;
}
