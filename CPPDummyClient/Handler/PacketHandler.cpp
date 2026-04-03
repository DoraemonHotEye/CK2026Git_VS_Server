#include "PacketHandler.h"

#include <iostream>
#include <sstream>

#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../Session/ServerSession.h"

void PacketHandler::MAINSERVER_SendSessionIDHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_OtherPlayerRoomJoinHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_SendRoomListHandler(CPacketSession* session, PacketInterface* message)
{
}

void PacketHandler::MAINSERVER_JoinRoomSuccHandler(CPacketSession* session, PacketInterface* message)
{
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
