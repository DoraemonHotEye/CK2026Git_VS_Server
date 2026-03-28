#include <iostream>
#include <thread>
#include <winsock2.h>
#include <WS2tcpip.h>

#include "Session/ServerSession.h"
#include "../CPPServerCore/Headers/CommonTypes.h"
#include "../CPPServerCore/Connector/CConnector.h"

#pragma comment(lib, "ws2_32.lib")

using namespace std;

// ⭐ IOCP WorkerThread
void WorkerThread(HANDLE iocp)
{
    while (true)
    {
        DWORD bytes = 0;
        ULONG_PTR key = 0;
        OVERLAPPED* overlapped = nullptr;

        BOOL ret = GetQueuedCompletionStatus(
            iocp,
            &bytes,
            &key,
            &overlapped,
            INFINITE
        );

        if (overlapped == nullptr)
            continue;

        IocpObject* obj = reinterpret_cast<IocpObject*>(key);
        IOContext* context = reinterpret_cast<IOContext*>(overlapped);

        obj->Dispatch(context, bytes);
    }
}

int main()
{
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    // 1. IOCP 생성
    HANDLE iocp = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);

    // 2. WorkerThread 실행
    for (int i = 0; i < 4; i++)
    {
        std::thread(WorkerThread, iocp).detach();
    }

    // 3. Connector 생성
    Connector connector;

    // 4. Session 생성 콜백
    OnConnectCallback createSession = [](SOCKET socket) -> CSession*
        {
            return new ServerSession();
        };

    // 5. 서버 주소 설정
    SOCKADDR_IN addr;
    addr.sin_family = AF_INET;
    inet_pton(AF_INET, "127.0.0.1", &addr.sin_addr); // 서버 IP
    addr.sin_port = htons(7777);                     // 서버 포트

    // 6. 연결 시도
    connector.Start(addr, iocp, createSession, 1);

    cout << "Client Connecting...\n";

    // 유지
    while (true)
    {
        Sleep(1000);
    }

    WSACleanup();
    return 0;
}