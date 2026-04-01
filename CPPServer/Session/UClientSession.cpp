#include "UClientSession.h"

#include <iostream>
#include <string>

#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../Manager/CUPacketManager.h"


void UClientSession::OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr)
{
	std::vector<uint8_t> vecBuffer(buffer, buffer + len);

	UPacketManager::Instance().OnRecvPacket(this, addr, vecBuffer);
}

void UClientSession::OnSend(int len, SOCKADDR_IN& addr)
{
}
