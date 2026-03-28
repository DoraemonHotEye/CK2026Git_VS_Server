#pragma once
#include "../../CPPServerCore/Session/CPacketSession.h"

class ClientSession : public CPacketSession
{
public:
	// PacketSession을(를) 통해 상속됨
	void OnConnected(SOCKADDR_IN addr) override;
	void OnSend(int len) override;
	void OnDisconnected() override;
	void OnRecvPacket(char* buffer, int len) override;
};

