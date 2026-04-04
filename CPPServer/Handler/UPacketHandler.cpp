#include "UPacketHandler.h"

#include <string>
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../../CPPServerCore/Managers/UIPEndPointManager.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../Session/UClientSession.h"
#include "../Game/Player/CPlayerManager.h"
#include "../Game/Player/CPlayer.h"
#include "../../CPPServerCore/Managers/SessionManager.h"
#include "../Session/ClientSession.h"

void UPacketHandler::DGRAM_SERVER_PingHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	char ip[INET_ADDRSTRLEN] = { 0 };
	inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));
	std::string msg;
	msg += ip;
	msg += ":";
	msg += std::to_string(ntohs(addr.sin_port));
	msg += " :: Ping Packet UDP Session :: ";

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
	GAMECLIENT_USessionIDMatch* _packet = dynamic_cast<GAMECLIENT_USessionIDMatch*>(message);

	bool succ = UIPEndPointManager::Instance().Add(addr);
	if (succ == true)
	{
		//TODO 로그 메세지
		auto csession_ = dynamic_cast<ClientSession*>(SessionManager::Instance().Get(_packet->SessionID));

		if (csession_ != nullptr)
			if (csession_->player != nullptr)
				csession_->player->_peerEndPoint = addr;
	}
}
