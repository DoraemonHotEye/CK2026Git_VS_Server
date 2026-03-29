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

    MAINSERVER_JoinRoomSucc succ = MAINSERVER_JoinRoomSucc();
    succ.MasterPlayer = PlayerData();
    succ.MasterPlayer.SessionID = 1001;

    succ.RoomHashData = "202021323#@41SDA";

    PlayerData data = PlayerData();
    data.SessionID = 1002;
    succ.OtherPlayers.Enter(data);

    PlayerData data2 = PlayerData();
    data2.SessionID = 1003;
    succ.OtherPlayers.Enter(data2);

    PlayerData data3 = PlayerData();
    data3.SessionID = 1004;
    succ.OtherPlayers.Enter(data3);

    PlayerData data4 = PlayerData();
    data4.SessionID = 1005;
    succ.OtherPlayers.Enter(data4);

    PlayerData data5 = PlayerData();
    data5.SessionID = 1006;
    succ.OtherPlayers.Enter(data5);

    succ.MaxPlayerCount = succ.OtherPlayers.GetCount();

    Send(succ);
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