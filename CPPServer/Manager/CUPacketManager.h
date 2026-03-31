#pragma once
#include <unordered_map>
#include <functional>
#include <memory>
#include <cstdint>
#include <WinSock2.h>

#include "../../CPPServerCore/Headers/Packets.h"

class UPacketManager
{
public:
    // ½Ì±ÛÅæ
    static UPacketManager& Instance()
    {
        static UPacketManager instance;
        return instance;
    }

    UPacketManager(const UPacketManager&) = delete;
    UPacketManager& operator=(const UPacketManager&) = delete;

private:
    UPacketManager()
    {
        Register();
    }

public:
    using MakeFunc = 
        std::function<std::unique_ptr<PacketInterface>(class UPacketSession*, SOCKADDR_IN&, const std::vector<uint8_t>&)>;
    using HandlerFunc = std::function<void(class UPacketSession*, SOCKADDR_IN&, PacketInterface*)>;

    void Register();

    void OnRecvPacket(class UPacketSession* session, SOCKADDR_IN& addr, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback = nullptr);

    template<typename T>
    static std::unique_ptr<PacketInterface> MakePacket(class UPacketSession* session, const std::vector<uint8_t>& buffer)
    {
        auto pkt = std::make_unique<T>();
        pkt->Deserialize(buffer);
        return pkt;
    }

    void HandlePacket(class UPacketSession* session, SOCKADDR_IN& addr, PacketInterface* packet, uint16_t id)
    {
        auto it = _handler.find(id);
        if (it != _handler.end())
            it->second(session, addr, packet);
    }

    // ÇÚµé·¯ µî·Ï
    void RegisterHandler(uint16_t id, HandlerFunc func);

private:
    std::unordered_map<uint16_t, MakeFunc> _makeFunc;
    std::unordered_map<uint16_t, HandlerFunc> _handler;
};