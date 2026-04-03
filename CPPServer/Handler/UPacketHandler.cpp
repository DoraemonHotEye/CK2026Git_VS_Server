#include "UPacketHandler.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../../CPPServerCore/Headers/Packets.h"

#include <string>
#include "../Session/UClientSession.h"
#include "../../CPPServerCore/Managers/UIPEndPointManager.h"

void UPacketHandler::DGRAM_SERVER_PingHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	char ip[INET_ADDRSTRLEN] = { 0 };
	inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));
	std::string msg;
	msg += ip;
	msg += ":";
	msg += std::to_string(ntohs(addr.sin_port));

	UIPEndPointManager::Instance().OnPingResponse(addr);

	LOG(msg);
}

void UPacketHandler::DGRAM_SERVER_AckSeqHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	DGRAM_SERVER_AckSeq* ack = dynamic_cast<DGRAM_SERVER_AckSeq*>(message);
	UClientSession* ucsession = dynamic_cast<UClientSession*>(session);

	if (ack != nullptr && ucsession != nullptr) 
	{
		bool succ = ucsession->sendTable.Ack(ack->SeqNum);

		if (succ == true) 
		{
			DGRAM_SERVER_RequestAck req = DGRAM_SERVER_RequestAck();
			req.ReqSeqNum = ack->SeqNum;
			session->Send(req, addr);
		}
	}		
}

void UPacketHandler::DGRAM_SERVER_RequestAckHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	DGRAM_SERVER_RequestAck* ack = dynamic_cast<DGRAM_SERVER_RequestAck*>(message);

	if (ack != nullptr) 
	{
		bool succ = session->RequestTables[addr].get()->Remove(ack->ReqSeqNum);

		if (succ == true)
		{
			std::string msg;
			msg += "Request Packet: ";
			msg += std::to_string(ack->ReqSeqNum);

			LOG(msg);
		}
	}
}

void UPacketHandler::GAMECLIENT_DisconnectUDPHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	GAMECLIENT_DisconnectUDP* _packet = dynamic_cast<GAMECLIENT_DisconnectUDP*>(message);

	std::string msg;

	msg += std::to_string(_packet->timeout);

	LOG(msg);
}

void UPacketHandler::GAMECLIENT_USessionIDMatchHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	bool succ = UIPEndPointManager::Instance().Add(addr);
	if (succ == true)
	{
		//TODO 로그 메세지

	}
}
