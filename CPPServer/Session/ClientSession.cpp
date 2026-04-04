#include "ClientSession.h"

#include <iostream>
#include <winsock2.h>
#include <WS2tcpip.h>
#include "../Manager/CPacketManager.h"
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../Game/Player/CPlayer.h"
#include "../Game/Player/CPlayerManager.h"
#include "../Game/Room/CRoom.h"

#pragma comment(lib, "ws2_32.lib")

void ClientSession::OnConnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::string log = "[Connected] ";
    log += ip;
    log += ":";
    log += std::to_string(ntohs(addr.sin_port));
    log += ":: Session ID: ";
    log += std::to_string(sessionID);

    LOG(log);

    PlayerManager::GetInstance().Generate(this);

    MAINSERVER_SendSessionID sessionid = MAINSERVER_SendSessionID();
    sessionid.SessionID = sessionID;
    Send(sessionid);
}

void ClientSession::OnSend(int len)
{
    //PrintLogThread::Instance().Log(
    //    "[Send] Bytes: " + std::to_string(len)
    //);
}

void ClientSession::OnDisconnected(SOCKADDR_IN addr)
{
    char ip[INET_ADDRSTRLEN] = { 0 };
    inet_ntop(AF_INET, &addr.sin_addr, ip, sizeof(ip));

    std::string log = "[Disconnected] ";
    log += ip;
    log += ":";
    log += std::to_string(ntohs(addr.sin_port));

    LOG(log);

    if (player != nullptr)
    {
        if (player->CurrentRoom != nullptr)
            player->CurrentRoom->Remove(player);

        PlayerManager::GetInstance().Remove(this);
    }
}

void ClientSession::OnRecvPacket(char* buffer, int len)
{
    // 필요하면 로그 추가 가능
    // PrintLogThread::Instance().Log("[Recv] Bytes: " + std::to_string(len));

    std::vector<uint8_t> vecBuffer(buffer, buffer + len);

    PacketManager::Instance().OnRecvPacket(this, vecBuffer);
}
