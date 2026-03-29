#pragma once
#include <WinSock2.h>
#include "ServerStaticHeaders.h"

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

        if (context->icopObj != nullptr)
            context->icopObj->Dispatch(context, bytes);
    }
}