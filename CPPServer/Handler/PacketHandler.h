
#pragma once

class PacketHandler 
{
public:
	static void GAMECLIENT_CreateRoomCallHandler(class CPacketSession* session, class PacketInterface* message);
    static void GAMECLIENT_RoomSendCallHandler(class CPacketSession* session, class PacketInterface* message);
    
};