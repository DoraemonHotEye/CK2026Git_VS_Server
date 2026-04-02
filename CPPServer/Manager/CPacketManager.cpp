
#include "CPacketManager.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/Session/CPacketSession.h"
#include "../Handler/PacketHandler.h"


void PacketManager::Register()
{
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_CREATEROOMCALL] = MakePacket<GAMECLIENT_CreateRoomCall>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_CREATEROOMCALL, PacketHandler::GAMECLIENT_CreateRoomCallHandler);
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_ROOMSENDCALL] = MakePacket<GAMECLIENT_RoomSendCall>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_ROOMSENDCALL, PacketHandler::GAMECLIENT_RoomSendCallHandler);
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_JOINROOMCALL] = MakePacket<GAMECLIENT_JoinRoomCall>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_JOINROOMCALL, PacketHandler::GAMECLIENT_JoinRoomCallHandler);
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_ROOMEXITCALL] = MakePacket<GAMECLIENT_RoomExitCall>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_ROOMEXITCALL, PacketHandler::GAMECLIENT_RoomExitCallHandler);
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_GAMESTART] = MakePacket<GAMECLIENT_GameStart>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_GAMESTART, PacketHandler::GAMECLIENT_GameStartHandler);
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_GAMESTARTREADY] = MakePacket<GAMECLIENT_GameStartReady>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_GAMESTARTREADY, PacketHandler::GAMECLIENT_GameStartReadyHandler);
    _makeFunc[(uint16_t)MsgID::GAMECLIENT_SENDLOCALENDPOINT] = MakePacket<GAMECLIENT_SendLocalEndPoint>;
    RegisterHandler((uint16_t)MsgID::GAMECLIENT_SENDLOCALENDPOINT, PacketHandler::GAMECLIENT_SendLocalEndPointHandler);
    
}

void PacketManager::OnRecvPacket(CPacketSession* session, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback)
{
    size_t offset = 0;

    if (buffer.size() < 4) return; // 최소 크기 체크

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