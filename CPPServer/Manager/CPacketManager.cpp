#include "CPacketManager.h"
#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/Session/CPacketSession.h"
#include "../Handler/PacketHandler.h"


void PacketManager::Register()
{
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_REQUESTACK] = MakePacket<DGRAM_SERVER_RequestAck>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_REQUESTACK, PacketHandler::DGRAM_SERVER_RequestPacketHandler);

    _makeFunc[(uint16_t)MsgID::MAINSERVER_SENDROOMLIST] = MakePacket<MAINSERVER_SendRoomList>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_SENDROOMLIST, PacketHandler::MAIN_SERVER_SendRoomListPacketHandler);

    _makeFunc[(uint16_t)MsgID::MAINSERVER_JOINROOMSUCC] = MakePacket<MAINSERVER_JoinRoomSucc>;
    RegisterHandler((uint16_t)MsgID::MAINSERVER_JOINROOMSUCC, PacketHandler::MAINSERVER_JoinRoomSuccPacketHandler);
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
