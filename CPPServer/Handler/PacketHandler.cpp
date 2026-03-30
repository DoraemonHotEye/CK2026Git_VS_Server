#include "PacketHandler.h"

#include <iostream>
#include <sstream>

#include "../Session/ClientSession.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"

void PacketHandler::DGRAM_SERVER_RequestPacketHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	DGRAM_SERVER_RequestAck* _packet = dynamic_cast<DGRAM_SERVER_RequestAck*>(message);

	std::stringstream ss;
	ss << "[DGRAM_REQUEST] "
		<< "ReqSeqNum: " << _packet->ReqSeqNum
		<< ", SeqNum: " << _packet->SeqNum;

	PrintLogThread::Instance().Log(ss.str());
}

void PacketHandler::MAIN_SERVER_SendRoomListPacketHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	MAINSERVER_SendRoomList* _packet = dynamic_cast<MAINSERVER_SendRoomList*>(message);

	std::stringstream ss;

	ss << "[RoomList] RoomCount: "
		<< _packet->RoomHashDatas.GetCount();

	for (auto& room : _packet->RoomHashDatas.Values)
	{
		ss << " | RoomHash: " << room;
	}

	PrintLogThread::Instance().Log(ss.str());
}

void PacketHandler::MAINSERVER_JoinRoomSuccPacketHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	MAINSERVER_JoinRoomSucc* _packet = dynamic_cast<MAINSERVER_JoinRoomSucc*>(message);

	std::stringstream ss;

	ss << "[JoinRoomSucc] "
		<< "MasterPlayer: " << _packet->MasterPlayer.SessionID
		<< ", MaxPlayerCount: " << _packet->MaxPlayerCount
		<< ", RoomHashData: " << _packet->RoomHashData
		<< ", OtherPlayerCount: " << _packet->OtherPlayers.GetCount();

	for (auto& pl : _packet->OtherPlayers.Values)
	{
		ss << " | OtherPlayer: " << pl.SessionID;
	}

	PrintLogThread::Instance().Log(ss.str());
}
