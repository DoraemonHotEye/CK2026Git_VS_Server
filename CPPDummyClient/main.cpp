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
#include "CmdHeader/CmdHeader.h"
#include "../CPPServerCore/JobTimer/CJobTimer.h"
#include "../CPPServerCore/ServerSupports/Configuration/CIpConfigManager.h"
#pragma comment(lib, "ws2_32.lib")

using namespace std;

UServerSession cusession;
Connector connector;

void FlushTimer()
{
    cusession.Update();
    UIPEndPointManager::Instance().Update();
    JobTimer::Instance().Push(FlushTimer, 1000);
}

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

    // 4. Session 생성 콜백
    OnAcceptCallback createSession = [](SOCKET socket) -> CSession*
        {
            // 풀에서 ClientSession 가져오기
            auto session = SessionManager::Instance().AcquireSession([]() {
                    auto ses = std::make_unique<ServerSession>();
                    ses.get()->usession = &cusession;
                return ses; // 생성 콜백
                });

            // 활성 세션 리스트에 등록
            CSession* rawPtr = session.get();
            SessionManager::Instance().AddSession(std::move(session));

            return rawPtr; // IOCP에서 raw pointer 사용
        };

    SOCKADDR_IN addru;
    addru.sin_family = AF_INET;
    inet_pton(
        AF_INET,
        IpConfigManager::Instance().ipConfig.UDPIPEndPoint.IPAddress.c_str(), // 수정된 부분
        &addru.sin_addr
    );
    addru.sin_port = htons(IpConfigManager::Instance().ipConfig.UDPIPEndPoint.Port);

    UIPEndPointManager::Instance().Add(addru);

    //GAMECLIENT_USessionIDMatch disconnect = GAMECLIENT_USessionIDMatch();
    //disconnect.SessionID = 241;
    //
    //for(int i = 0; i < 1; ++i)
    //    cusession.Send(disconnect, addru, true);
    cusession.StartClient(IpConfigManager::Instance().ipConfig.UDPIPEndPoint.IPAddress, IpConfigManager::Instance().ipConfig.UDPIPEndPoint.Port);

    // 5. 서버 주소 설정
    SOCKADDR_IN addr;
    addr.sin_family = AF_INET;
    inet_pton(
        AF_INET,
        IpConfigManager::Instance().ipConfig.TCPIPEndPoint.IPAddress.c_str(), // 수정된 부분
        &addr.sin_addr
    );
    addr.sin_port = htons(IpConfigManager::Instance().ipConfig.TCPIPEndPoint.Port);


    // 6. 연결 시도
    connector.Start(addr, iocp, createSession, 425);

    cout << "Client Connecting...\n";

    RegisterCommand();

    JobTimer::Instance().Push(FlushTimer, 200);
    // 유지
    while (true)
    {
        JobTimer::Instance().Flush();
    }

    WSACleanup();

    PrintLogThread::Instance().Stop();

    return 0;
}