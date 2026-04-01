#pragma once
#include "../../CPPServerCore/UDP/Session/CUPacketSession.h"

class UServerSession : public CUPacketSession
{
public:
	void OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr) override;
	void OnSend(int len, SOCKADDR_IN& addr) override;
};

