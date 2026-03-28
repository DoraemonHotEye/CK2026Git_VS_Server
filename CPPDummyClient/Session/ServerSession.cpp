#include "ServerSession.h"

#include <iostream>
#include <winsock2.h>
#include <WS2tcpip.h>

#pragma comment(lib, "ws2_32.lib")

void ServerSession::OnConnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::cout << "[Connected] "
        << ip << ":" << ntohs(addr.sin_port)
        << std::endl;
}

void ServerSession::OnSend(int len)
{
    std::cout << "[Send] Bytes: " << len << std::endl;
}

void ServerSession::OnDisconnected()
{
    std::cout << "[Disconnected]" << std::endl;
}

void ServerSession::OnRecvPacket(char* buffer, int len)
{
}