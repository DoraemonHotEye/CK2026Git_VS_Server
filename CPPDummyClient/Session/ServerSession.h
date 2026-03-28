#pragma once
#include "../../CPPServerCore/Session/CPacketSession.h"

class ServerSession : public CPacketSession
{
public:
	void OnConnected(SOCKADDR_IN addr) override;
	void OnSend(int len) override;
	void OnDisconnected() override;
	void OnRecvPacket(char* buffer, int len) override;
};

