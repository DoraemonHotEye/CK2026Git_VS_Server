#include "PacketHandler.h"

#include <iostream>
#include <sstream>

#include "../Session/ClientSession.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../Game/Room/CRoomManager.h"
#include "../Game/Room/CRoom.h"
#include "../Game/Player/CPlayer.h"

void PacketHandler::GAMECLIENT_CreateRoomCallHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	GAMECLIENT_CreateRoomCall* _packet = dynamic_cast<GAMECLIENT_CreateRoomCall*>(message);

	if (_session != nullptr && _packet != nullptr) 
	{
		if (_session->player == nullptr) 
		{
			LOG("[Server] Player Null REF (Create Room Packet Call)");
			return;
		}
		LOG("[Server] Create Room Packet :: session id :: -> " + std::to_string(_session->sessionID));
		RoomManager::GetInstance().GenerateAndJoin(_session->player, 3);
	}
}

void PacketHandler::GAMECLIENT_RoomSendCallHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	GAMECLIENT_RoomSendCall* _packet = dynamic_cast<GAMECLIENT_RoomSendCall*>(message);

	if (_session != nullptr && _packet != nullptr) 
	{
		std::stringstream ss;
		ss << "SessionID: " << _session->sessionID << "	||	GAMECLIENT_RoomSendCallPacket	||	timeOut: " << _packet->timeout;

		PrintLogThread::Instance().Log(ss.str());

		auto hashes = RoomManager::GetInstance().GetRoomHashList();

		if (hashes.size() > 0)
		{
			MAINSERVER_SendRoomList roomList = MAINSERVER_SendRoomList();

			for (int i = 0; i < hashes.size(); ++i)
			{
				roomList.RoomHashDatas.Enter(hashes[i]);
			}

			_session->Send(roomList);
		}
	}
}

void PacketHandler::GAMECLIENT_JoinRoomCallHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	GAMECLIENT_JoinRoomCall* _packet = dynamic_cast<GAMECLIENT_JoinRoomCall*>(message);

	if (_session != nullptr && _packet != nullptr)
	{
		auto room = RoomManager::GetInstance().GetRoom(_packet->RoomHashID);
		room->Enter(_session->player);
	}
}

void PacketHandler::GAMECLIENT_RoomExitCallHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);

	if (_session != nullptr)
	{
		if (_session->player != nullptr)
		{
			if (_session->player->CurrentRoom != nullptr)
			{
				_session->player->CurrentRoom->Remove(_session->player);
			}
		}
	}
}

void PacketHandler::GAMECLIENT_GameStartHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);

	if (_session != nullptr) 
	{
		if (_session->player != nullptr)
		{
			if (_session->player->CurrentRoom != nullptr) 
			{
				_session->player->CurrentRoom->GameStart();
			}
		}
	}
}

void PacketHandler::GAMECLIENT_GameStartReadyHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);

	if (_session != nullptr)
	{
		if (_session->player != nullptr)
		{
			if (_session->player->CurrentRoom != nullptr)
			{
				_session->player->CurrentRoom->Ready();
			}
		}
	}
}

void PacketHandler::GAMECLIENT_SendLocalEndPointHandler(CPacketSession* session, PacketInterface* message)
{
	ClientSession* _session = dynamic_cast<ClientSession*>(session);
	GAMECLIENT_SendLocalEndPoint* endpoint = dynamic_cast<GAMECLIENT_SendLocalEndPoint*>(message);

	if (_session != nullptr)
	{
		if (_session->player != nullptr && endpoint != nullptr)
		{
			_session->player->_localAddress = endpoint->LocalAddress;
			LOG("SET Local Address :: SessionID (" + std::to_string(_session->sessionID) + ")" + " :: Local IP: " + endpoint->LocalAddress + " ::");
		}
	}
}
