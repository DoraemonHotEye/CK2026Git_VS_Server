
#include "CPacketManager.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/Session/CPacketSession.h"
#include "../Handler/PacketHandler.h"


void PacketManager::Register()
{
    _makeFunc[(uint16_t)MsgID::MAINSERVER_SENDSESSIONID] = MakePacket<MAINSERVER_SendSessionID>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_SENDSESSIONID, PacketHandler::MAINSERVER_SendSessionIDHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_OTHERPLAYERROOMJOIN] = MakePacket<MAINSERVER_OtherPlayerRoomJoin>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_OTHERPLAYERROOMJOIN, PacketHandler::MAINSERVER_OtherPlayerRoomJoinHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_SENDROOMLIST] = MakePacket<MAINSERVER_SendRoomList>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_SENDROOMLIST, PacketHandler::MAINSERVER_SendRoomListHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_JOINROOMSUCC] = MakePacket<MAINSERVER_JoinRoomSucc>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_JOINROOMSUCC, PacketHandler::MAINSERVER_JoinRoomSuccHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_ROOMMASTERCHANGED] = MakePacket<MAINSERVER_RoomMasterChanged>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_ROOMMASTERCHANGED, PacketHandler::MAINSERVER_RoomMasterChangedHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_ROOMEXITSUCC] = MakePacket<MAINSERVER_RoomExitSucc>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_ROOMEXITSUCC, PacketHandler::MAINSERVER_RoomExitSuccHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_OTHERPLAYERROOMEXIT] = MakePacket<MAINSERVER_OtherPlayerRoomExit>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_OTHERPLAYERROOMEXIT, PacketHandler::MAINSERVER_OtherPlayerRoomExitHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_NOROOMCALLBACK] = MakePacket<MAINSERVER_NoRoomCallback>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_NOROOMCALLBACK, PacketHandler::MAINSERVER_NoRoomCallbackHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_SENDPEERUSER] = MakePacket<MAINSERVER_SendPeerUser>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_SENDPEERUSER, PacketHandler::MAINSERVER_SendPeerUserHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_GAMESTARTSUCC] = MakePacket<MAINSERVER_GameStartSucc>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_GAMESTARTSUCC, PacketHandler::MAINSERVER_GameStartSuccHandler);
    _makeFunc[(uint16_t)MsgID::MAINSERVER_SENDPBENDPOINT] = MakePacket<MAINSERVER_SendPbEndPoint>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_SENDPBENDPOINT, PacketHandler::MAINSERVER_SendPbEndPointHandler);
    
}

void PacketManager::OnRecvPacket(CPacketSession* session, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback)
{
    size_t offset = 0;

    if (buffer.size() < 4) return;

    uint16_t size = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    uint16_t id = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    auto it = _makeFunc.find(id);
    if (it != _makeFunc.end())
    {
        std::unique_ptr<PacketInterface> packet = it->second(session, buffer);

        if (onRecvCallback)
            onRecvCallback(session, packet.get());
        else
            HandlePacket(session, packet.get(), id);
    }
}

void PacketManager::RegisterHandler(uint16_t id, HandlerFunc func)
{
    _handler[id] = func;
}