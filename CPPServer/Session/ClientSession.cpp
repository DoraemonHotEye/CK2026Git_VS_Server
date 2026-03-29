#include "ClientSession.h"

#include <iostream>
#include <winsock2.h>
#include <WS2tcpip.h>
#include "../Manager/CPacketManager.h"

#pragma comment(lib, "ws2_32.lib")

void ClientSession::OnConnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::cout << "[Connected] "
        << ip << ":" << ntohs(addr.sin_port)
        << std::endl;
}

void ClientSession::OnSend(int len)
{
    std::cout << "[Send] Bytes: " << len << std::endl;
}

void ClientSession::OnDisconnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::cout << "[Disconnected] " 
        << ip << ":" << ntohs(addr.sin_port)
        << std::endl;
}

void ClientSession::OnRecvPacket(char* buffer, int len)
{
    // char* buffer를 std::vector<uint8_t>로 변환
    std::vector<uint8_t> vecBuffer(buffer, buffer + len);

    // PacketManager 호출
    PacketManager::Instance().OnRecvPacket(this, vecBuffer);
}
