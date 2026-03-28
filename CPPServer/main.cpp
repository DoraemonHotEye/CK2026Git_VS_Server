#include <iostream>
#include <thread>
#include <winsock2.h>
#include "Session/ClientSession.h"
#include "../CPPServerCore/Listener/CListener.h"
#include "../CPPServerCore/Headers/CommonTypes.h"

#pragma comment(lib, "ws2_32.lib")

void workerThread(HANDLE iocp)
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

        // ⭐ 타입 먼저 확인
        if (context->type == IOContext::Type::Accept)
        {
            // Accept는 bytes == 0 정상
            if (!ret)
                continue;

            obj->Dispatch(context, bytes);
            continue;
        }

        // ⭐ Session 계열 (Recv/Send)
        if (!ret || bytes == 0)
        {
            CSession* session = static_cast<CSession*>(obj);
            session->Disconnect();
            continue;
        }

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
        std::thread(workerThread, iocp).detach();
    }

    // 3. Listener 생성
    Listener listener;

    // 4. 콜백 설정 (Session 생성)
    OnAcceptCallback createSession = [](SOCKET socket)
        {
            return new ClientSession();
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
    return 0;
}