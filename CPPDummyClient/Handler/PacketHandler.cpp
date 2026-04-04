#include "PacketHandler.h"

#include <iostream>
#include <sstream>

#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../Session/ServerSession.h"
#include "../Session/UServerSession.h"

void PacketHandler::MAINSERVER_SendSessionIDHandler(CPacketSession* session, PacketInterface* message)
{
	ServerSession* _session = dynamic_cast<ServerSession*>(session);
	MAINSERVER_SendSessionID* _packet = dynamic_cast<MAINSERVER_SendSessionID*>(message);

	if (_session != nullptr && _packet != nullptr)
	{
		GAMECLIENT_USessionIDMatch match = GAMECLIENT_USessionIDMatch();
		match.SessionID = _packet->SessionID;
		_session->usession->Send(match, _session->usession->_serverAddr, true);
	}
}

void PacketHandler::MAINSERVER_OtherPlayerRoomJoinHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_SendRoomListHandler(CPacketSession* session, PacketInterface* message)
{
	ServerSession* _session = dynamic_cast<ServerSession*>(session);
	MAINSERVER_SendRoomList* _packet = dynamic_cast<MAINSERVER_SendRoomList*>(message);

	if (_session != nullptr && _packet != nullptr)
	{
		LOG("[Server] : RoomList :");
		for (auto room : _packet->RoomHashDatas.Values) 
		{
			LOG(room);
		}
	}
}

void PacketHandler::MAINSERVER_JoinRoomSuccHandler(CPacketSession* session, PacketInterface* message)
{
	LOG("[Server]: Join Room Succ :: Session ID :: " + std::to_string(session->sessionID));
}

void PacketHandler::MAINSERVER_RoomMasterChangedHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_RoomExitSuccHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_OtherPlayerRoomExitHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_NoRoomCallbackHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_SendPeerUserHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_GameStartSuccHandler(CPacketSession* session, PacketInterface* message)
{
	
}

void PacketHandler::MAINSERVER_SendPbEndPointHandler(CPacketSession* session, PacketInterface* message)
{
}
