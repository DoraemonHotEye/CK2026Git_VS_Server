#include "CUPacketManager.h"

void UPacketManager::Register()
{

}

void UPacketManager::OnRecvPacket(UPacketSession* session, SOCKADDR_IN& addr, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback)
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
        std::unique_ptr<PacketInterface> packet = it->second(session, addr, buffer);

        if (onRecvCallback)
            onRecvCallback(session, addr, packet.get());
        else
            HandlePacket(session, addr, packet.get(), id);
    }
}

void UPacketManager::RegisterHandler(uint16_t id, HandlerFunc func)
{
    _handler[id] = func;
}