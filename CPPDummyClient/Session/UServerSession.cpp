#include "UServerSession.h"

#include <iostream>
#include <string>

#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"

void UServerSession::OnSend(int len)
{
}

void UServerSession::OnRecvPacket(char* buffer, int len, SOCKADDR_IN& addr)
{
	std::string log;
	log += std::to_string(len);

	LOG(log);
}
