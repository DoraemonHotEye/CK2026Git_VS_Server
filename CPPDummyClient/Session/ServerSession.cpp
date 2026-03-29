#include "ServerSession.h"

#include <iostream>
#include <winsock2.h>
#include <WS2tcpip.h>

#include "../../CPPServerCore/Headers/Packets.h"

#pragma comment(lib, "ws2_32.lib")

void ServerSession::OnConnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::cout << "[Connected] "
        << ip << ":" << ntohs(addr.sin_port)
        << std::endl;

    //DGRAM_SERVER_RequestAck* _packet = new DGRAM_SERVER_RequestAck();
    //_packet->ReqSeqNum = 1;
    //_packet->SeqNum = 1;
    //Send(*_packet);
    //
    //delete _packet;
    //_packet = nullptr;
}

void ServerSession::OnSend(int len)
{
    std::cout << "[Send] Bytes: " << len << std::endl;
}

void ServerSession::OnDisconnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::cout << "[Disconnected] " 
        << ip << ":" << ntohs(addr.sin_port)
        << std::endl;
}

void ServerSession::OnRecvPacket(char* buffer, int len)
{
}