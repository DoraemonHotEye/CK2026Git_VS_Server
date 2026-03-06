using MakePacketTemplate.Packets;
using Server.Session;
using Server.Session.Manager;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UDgramCore;

public class UPacketHandler
{
    public static void DGRAM_SERVER_PingHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        Console.WriteLine($"[Ping Packet] {Sender}");
    }

    public static void DGRAM_SERVER_ConnectPacketHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        Console.WriteLine($"[DGRAM_SERVER]: {Sender}");
    }

    public static void GAMECLIENT_DisconnectUDPHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        Console.WriteLine($"[DGRAM_SERVER]: Disconnecrt 감지 {Sender}");
        UPlayerManager.Instance.RemovePlayer(Sender);
    }
}