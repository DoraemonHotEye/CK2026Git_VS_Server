#pragma once
#include "../../CPPServerCore/Session/CPacketSession.h"

class ClientSession : public CPacketSession, public std::enable_shared_from_this<ClientSession>
{
public:
	class Player* player;  // 소유권 공유

public:
	// PacketSession을(를) 통해 상속됨
	void OnConnected(SOCKADDR_IN addr) override;
	void OnSend(int len) override;
	void OnDisconnected(SOCKADDR_IN addr) override;
	void OnRecvPacket(char* buffer, int len) override;
};

