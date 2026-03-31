#pragma once
#include "CUSession.h"

class CUPacketSession : public CUSession
{
public:
    static const int HeaderSize = 2;

	// CUSession을(를) 통해 상속됨
	virtual void OnSend(int len) = 0;
	virtual void OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr) = 0;

	// CUSession을(를) 통해 상속됨
	int OnRecv(char* buffer, int len, SOCKADDR_IN& addr) override
    {
        int processLen = 0;

        OnRecvPacket(buffer, len, addr);

        return processLen;

    }
};

