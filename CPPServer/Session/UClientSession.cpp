#include "UClientSession.h"

#include <iostream>
#include <string>

#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"


void UClientSession::OnSend(int len)
{
}

void UClientSession::OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr)
{
	std::string log;
	log += std::to_string(len);

	LOG(log);
}
