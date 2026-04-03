#include "UPacketHandler.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../../CPPServerCore/Headers/Packets.h"

#include <string>
#include "../Session/UServerSession.h"

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
	UServerSession* ucsession = dynamic_cast<UServerSession*>(session);

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
		bool succ = session->RequestTables[addr].get()->Remove(ack->SeqNum);

		if (succ == true)
		{
			std::string msg;
			msg += "Request Packet: ";
			msg += std::to_string(ack->ReqSeqNum);

			LOG(msg);
		}
	}
}