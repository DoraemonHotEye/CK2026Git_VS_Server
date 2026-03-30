#include "ServerSession.h"

#include <iostream>
#include <winsock2.h>
#include <WS2tcpip.h>

#include "../../CPPServerCore/Headers/Packets.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"

#pragma comment(lib, "ws2_32.lib")

void ServerSession::OnConnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::string log = "[Connected] ";
    log += ip;
    log += ":";
    log += std::to_string(ntohs(addr.sin_port));

    PrintLogThread::Instance().Log(log);

    GAMECLIENT_RoomSendCall call = GAMECLIENT_RoomSendCall();
    call.timeout = 40;
    Send(call);
}

void ServerSession::OnSend(int len)
{
    //std::cout << "[Send] Bytes: " << len << std::endl;
}

void ServerSession::OnDisconnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::string log = "[Disconnected] ";
    log += ip;
    log += ":";
    log += std::to_string(ntohs(addr.sin_port));

    PrintLogThread::Instance().Log(log);
}

void ServerSession::OnRecvPacket(char* buffer, int len)
{
}