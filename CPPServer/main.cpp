#include <iostream>
#include <thread>
#include <winsock2.h>

#include "Session/ClientSession.h"

#include "../CPPServerCore/Listener/CListener.h"
#include "../CPPServerCore/Headers/CommonTypes.h"
#include "../CPPServerCore/Managers/SessionManager.h"
#include "../CPPServerCore/Headers/StaticMethods.h"
#include "../CPPServerCore/ServerSupports/PrintLogThread.h"

#pragma comment(lib, "ws2_32.lib")

int main()
{
    //Logger Start
    PrintLogThread::Instance().Start();

    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    // 1. IOCP 생성
    HANDLE iocp = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);

    // 2. WorkerThread 실행
    for (int i = 0; i < 4; i++)
    {
        std::thread(workerThread, iocp).detach();
    }

    // 3. Listener 생성
    Listener listener;

    // 4. 콜백 설정 (Session 생성)
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

    

    // 5. 주소 설정
    SOCKADDR_IN addr;
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    addr.sin_port = htons(7777);

    // 6. Listener 시작
    listener.Start(addr, iocp, createSession);

    std::cout << "Server Started (Port: 7777)\n";

    // 서버 유지
    while (true)
    {
        Sleep(1000);
    }

    WSACleanup();

    PrintLogThread::Instance().Stop();

    return 0;
}