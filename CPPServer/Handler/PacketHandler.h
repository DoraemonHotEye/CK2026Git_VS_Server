#pragma once

class PacketHandler 
{
public:
	static void DGRAM_SERVER_RequestPacketHandler(class CPacketSession* session, class PacketInterface* message);
	static void MAIN_SERVER_SendRoomListPacketHandler(class CPacketSession* session, class PacketInterface* message);
	static void MAINSERVER_JoinRoomSuccPacketHandler(class CPacketSession* session, class PacketInterface* message);
};