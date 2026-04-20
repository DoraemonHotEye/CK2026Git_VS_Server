
#include "CUPacketManager.h"

#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/UDP/Session/CUPacketSession.h"

#include "../Handler/UPacketHandler.h"

void UPacketManager::Register()
{
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_PING] = MakePacket<DGRAM_SERVER_Ping>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_PING, UPacketHandler::DGRAM_SERVER_PingHandler);
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_ACKSEQ] = MakePacket<DGRAM_SERVER_AckSeq>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_ACKSEQ, UPacketHandler::DGRAM_SERVER_AckSeqHandler);
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_REQUESTACK] = MakePacket<DGRAM_SERVER_RequestAck>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_REQUESTACK, UPacketHandler::DGRAM_SERVER_RequestAckHandler);
    
}

void UPacketManager::OnRecvPacket(CUPacketSession* session, SOCKADDR_IN& addr, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback)
{
    size_t offset = 0;

    if (buffer.size() < 4) return;

    uint16_t size = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    uint16_t id = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    uint16_t seqNum = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    if (seqNum != 0 && id != static_cast<uint16_t>(MsgID::DGRAM_SERVER_ACKSEQ))
    {
        if (session->RequestTables.find(addr) == session->RequestTables.end()) {
            session->RequestTables[addr] = std::make_shared<RudpRequestTable>();
        }
        bool succ = session->RequestTables[addr]->TryAdd(seqNum);
        if (succ)
        {
            DGRAM_SERVER_AckSeq ack = DGRAM_SERVER_AckSeq();
            ack.SeqNum = seqNum;
            session->Send(ack, addr);
        }
        else
        {
            return;
        }
    }

    auto it = _makeFunc.find(id);
    if (it != _makeFunc.end())
    {
        std::unique_ptr<PacketInterface> packet = it->second(session, buffer);

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