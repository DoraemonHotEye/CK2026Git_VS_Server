#include <iostream>
#include <thread>
#include <winsock2.h>
#include <WS2tcpip.h>

#include "Session/ServerSession.h"
#include "../CPPServerCore/Headers/CommonTypes.h"
#include "../CPPServerCore/Connector/CConnector.h"
#include "../CPPServerCore/Managers/SessionManager.h"
#include "../CPPServerCore/Headers/StaticMethods.h"
#include "../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../CPPServerCore/UDP/Session/CUSession.h"
#include "Session/UServerSession.h"
#include "../CPPServerCore/Headers/Packets.h"

#pragma comment(lib, "ws2_32.lib")

using namespace std;

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

    UServerSession cusession;

    // 3. Connector 생성
    Connector connector;

    // 4. Session 생성 콜백
    OnAcceptCallback createSession = [](SOCKET socket) -> CSession*
        {
            // 풀에서 ClientSession 가져오기
            auto session = SessionManager::Instance().AcquireSession([]() {
                return std::make_unique<ServerSession>(); // 생성 콜백
                });

            // 활성 세션 리스트에 등록
            CSession* rawPtr = session.get();
            SessionManager::Instance().AddSession(std::move(session));

            return rawPtr; // IOCP에서 raw pointer 사용
        };

    SOCKADDR_IN addru;
    addru.sin_family = AF_INET;
    inet_pton(AF_INET, "127.0.0.1", &addru.sin_addr); // 서버 IP
    addru.sin_port = htons(5556);

    cusession.StartClient("127.0.0.1", 5556);

    UIPEndPointManager::Instance().Add(addru);

    GAMECLIENT_USessionIDMatch disconnect = GAMECLIENT_USessionIDMatch();
    disconnect.SessionID = 241;

    for(int i = 0; i < 1; ++i)
        cusession.Send(disconnect, addru, true);

    // 5. 서버 주소 설정
    SOCKADDR_IN addr;
    addr.sin_family = AF_INET;
    inet_pton(AF_INET, "127.0.0.1", &addr.sin_addr); // 서버 IP
    addr.sin_port = htons(7777);                     // 서버 포트


    // 6. 연결 시도
    connector.Start(addr, iocp, createSession, 150);

    cout << "Client Connecting...\n";

    // 유지
    while (true)
    {
        cusession.Update();
        UIPEndPointManager::Instance().Update();
        std::this_thread::sleep_for(std::chrono::milliseconds(1));
    }

    WSACleanup();

    PrintLogThread::Instance().Stop();

    return 0;
}