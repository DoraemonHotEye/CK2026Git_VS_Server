#pragma once
#include "CUSession.h"
#include "../../Managers/UIPEndPointManager.h"
#include "../../ServerSupports/PrintLogThread.h"
#include "../../Headers/Packets.h"


class CUPacketSession : public CUSession
{
protected:
    void SendPeerPing(SOCKADDR_IN& addr)
    {
        DGRAM_SERVER_Ping ping = DGRAM_SERVER_Ping();
        Send(ping, addr);
    }

public:
    static const int HeaderSize = 2;

	// CUSession을(를) 통해 상속됨
	virtual void OnSend(int len, SOCKADDR_IN& addr) = 0;
	virtual void OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr) = 0;

    virtual void RegisterUpdateMethod() override
    {
        UIPEndPointManager::Instance().RegisterSendCallback([this](SOCKADDR_IN& addr)
            {
                SendPeerPing(addr);
                return nullptr; // 반환형 맞추기
            });
    }

	// CUSession을(를) 통해 상속됨
	int OnRecv(char* buffer, int len, SOCKADDR_IN& addr) override
    {
        int processLen = 0;

        OnRecvPacket(buffer, len, addr);

        return processLen;

    }
};

