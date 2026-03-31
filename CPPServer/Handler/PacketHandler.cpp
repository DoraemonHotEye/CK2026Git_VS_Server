#include "PacketHandler.h"

#include <iostream>
#include <sstream>

#include "../Session/ClientSession.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"

void PacketHandler::GAMECLIENT_CreateRoomCallHandler(CPacketSession* session, PacketInterface* message)
{
	
}

void PacketHandler::GAMECLIENT_RoomSendCallHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	GAMECLIENT_RoomSendCall* _packet = dynamic_cast<GAMECLIENT_RoomSendCall*>(message);

	std::stringstream ss;
	ss << "SessionID: " << _session->sessionID << "	||	GAMECLIENT_RoomSendCallPacket	||	timeOut: " << _packet->timeout;

	PrintLogThread::Instance().Log(ss.str());
}

void PacketHandler::GAMECLIENT_JoinRoomCallHandler(CPacketSession* session, PacketInterface* message)
{

}

void PacketHandler::GAMECLIENT_RoomExitCallHandler(CPacketSession* session, PacketInterface* message)
{

}

void PacketHandler::GAMECLIENT_GameStartHandler(CPacketSession* session, PacketInterface* message)
{

}
