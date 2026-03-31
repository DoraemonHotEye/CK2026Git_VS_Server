#pragma once
#include <WinSock2.h>

class UPacketHandler
{
public:
    static void GAMECLIENT_CreateRoomCallHandler(class CPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    static void GAMECLIENT_RoomSendCallHandler(class CPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    static void GAMECLIENT_JoinRoomCallHandler(class CPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);

};