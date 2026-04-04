#include <iostream>
#include <thread>
#include <winsock2.h>

#include "Session/ClientSession.h"
#include "Session/UClientSession.h"

#include "../CPPServerCore/Listener/CListener.h"
#include "../CPPServerCore/Headers/CommonTypes.h"
#include "../CPPServerCore/Managers/SessionManager.h"
#include "../CPPServerCore/Headers/StaticMethods.h"
#include "../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../CPPServerCore/UDP/Session/CUSession.h"
#include "../CPPServerCore/JobTimer/CJobTimer.h"

#include "Game/PeerJobTimerSender/PeerJobTimerSender.h"

#pragma comment(lib, "ws2_32.lib")


Listener listener;
UClientSession cusession;

void FlushTimer() 
{
    cusession.Update();
    UIPEndPointManager::Instance().Update();
    PeerJobTimerSender::Instance().BroadCast();
    JobTimer::Instance().Push(FlushTimer, 1000);
}

int main()
{
    PrintLogThread::Instance().Start();

    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    HANDLE iocp = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);

    for (int i = 0; i < 4; i++)
    {
        std::thread(workerThread, iocp).detach();
    }

    OnAcceptCallback createSession = [](SOCKET socket) -> CSession*
        {
            // 풀에서 ClientSession 가져오기
            auto session = SessionManager::Instance().AcquireSession([]() {
                return std::make_unique<ClientSession>(); // 생성 콜백
                });
    
            // 활성 세션 리스트에 등록
            CSession* rawPtr = session.get();
            SessionManager::Instance().AddSession(std::move(session));
    
            return rawPtr; // IOCP에서 raw pointer 사용
        };

    SOCKADDR_IN addr;
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    addr.sin_port = htons(7777);

    // 6. Listener 시작
    cusession.Start(5556);
    listener.Start(addr, iocp, createSession);

    std::cout << "Server Started (Port: 7777)\n";
    JobTimer::Instance().Push(FlushTimer, 200);

    // 서버 유지
    while (true)
    {
        JobTimer::Instance().Flush();
    }

    WSACleanup();

    PrintLogThread::Instance().Stop();

    return 0;
}