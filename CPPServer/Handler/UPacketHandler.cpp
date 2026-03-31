#include "UPacketHandler.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../../CPPServerCore/Headers/Packets.h"

#include <string>

void UPacketHandler::GAMECLIENT_DisconnectUDPHandler(CUPacketSession* session, SOCKADDR_IN& addr, PacketInterface* message)
{
	GAMECLIENT_DisconnectUDP* _packet = dynamic_cast<GAMECLIENT_DisconnectUDP*>(message);

	std::string msg;

	msg += std::to_string(_packet->timeout);

	LOG(msg);
}
