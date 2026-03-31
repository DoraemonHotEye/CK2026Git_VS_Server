
#pragma once
#include <WinSock2.h>

class UPacketHandler
{
public:
    static void GAMECLIENT_DisconnectUDPHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    

};