#pragma once
#include <unordered_map>
#include <functional>
#include <memory>
#include <cstdint>
#include "../../CPPServerCore/Headers/Packets.h"

class PacketManager
{
public:
    // ½Ì±ÛÅæ
    static PacketManager& Instance()
    {
        static PacketManager instance;
        return instance;
    }

    PacketManager(const PacketManager&) = delete;
    PacketManager& operator=(const PacketManager&) = delete;

private:
    PacketManager()
    {
        Register();
    }

public:
    using MakeFunc = std::function<std::unique_ptr<PacketInterface>(class CPacketSession*, const std::vector<uint8_t>&)>;
    using HandlerFunc = std::function<void(class CPacketSession*, PacketInterface*)>;

    void Register();

    void OnRecvPacket(class CPacketSession* session, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback = nullptr);

    template<typename T>
    static std::unique_ptr<PacketInterface> MakePacket(class CPacketSession* session, const std::vector<uint8_t>& buffer)
    {
        auto pkt = std::make_unique<T>();
        pkt->Deserialize(buffer);
        return pkt;
    }

    inline void HandlePacket(class CPacketSession* session, PacketInterface* packet, uint16_t id)
    {
        auto it = _handler.find(id);
        if (it != _handler.end())
            it->second(session, packet);
    }

    // ÇÚµé·¯ µî·Ï
    void RegisterHandler(uint16_t id, HandlerFunc func);

private:
    std::unordered_map<uint16_t, MakeFunc> _makeFunc;
    std::unordered_map<uint16_t, HandlerFunc> _handler;
};