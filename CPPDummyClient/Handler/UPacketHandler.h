
#pragma once
#include <WinSock2.h>

class UPacketHandler
{
public:
    static void DGRAM_SERVER_PingHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    static void DGRAM_SERVER_AckSeqHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    static void DGRAM_SERVER_RequestAckHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    

};