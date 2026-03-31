#pragma once
#include "../../CPPServerCore/UDP/Session/CUPacketSession.h"

class UServerSession : public CUPacketSession
{
public:
	// CUPacketSession을(를) 통해 상속됨
	void OnSend(int len) override;
	void OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr) override;
};

