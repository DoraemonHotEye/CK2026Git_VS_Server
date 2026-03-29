#include "PacketHandler.h"

#include <iostream>

#include "../Session/ClientSession.h"
#include "../../CPPServerCore/Headers/Packets.h"

void PacketHandler::DGRAM_SERVER_RequestPacketHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	DGRAM_SERVER_RequestAck* _packet = dynamic_cast<DGRAM_SERVER_RequestAck*>(message);

	std::cout << _packet->ReqSeqNum << std::endl;
	std::cout << _packet->SeqNum << std::endl;
}

void PacketHandler::MAIN_SERVER_SendRoomListPacketHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	MAINSERVER_SendRoomList* _packet = dynamic_cast<MAINSERVER_SendRoomList*>(message);

	std::cout << "RoomCount: " << _packet->RoomHashDatas.GetCount() << std::endl;

	for (auto room : _packet->RoomHashDatas.Values) 
	{
		std::cout << "RoomHash: " << room << std::endl;
	}
}

void PacketHandler::MAINSERVER_JoinRoomSuccPacketHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	MAINSERVER_JoinRoomSucc* _packet = dynamic_cast<MAINSERVER_JoinRoomSucc*>(message);
	
	std::cout << "MasterPlayer: " << _packet->MasterPlayer.SessionID << std::endl;
	std::cout << "MaxPlayerCount: " << _packet->MaxPlayerCount << std::endl;
	std::cout << "RoomHashData: " << _packet->RoomHashData << std::endl;
	std::cout << "OtherPlayerCount: " << _packet->OtherPlayers.GetCount() << std::endl;

	for (auto pl : _packet->OtherPlayers.Values)
	{
		std::cout << "OtherPlayer: " << pl.SessionID << std::endl;
	}
}
