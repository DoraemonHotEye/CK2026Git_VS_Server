
#pragma once

class PacketHandler 
{
public:
	static void MAINSERVER_SendSessionIDHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_OtherPlayerRoomJoinHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_SendRoomListHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_JoinRoomSuccHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_RoomMasterChangedHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_RoomExitSuccHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_OtherPlayerRoomExitHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_NoRoomCallbackHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_SendPeerUserHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_GameStartSuccHandler(class CPacketSession* session, class PacketInterface* message);
    static void MAINSERVER_SendPbEndPointHandler(class CPacketSession* session, class PacketInterface* message);
    
};