using MakePacketTemplate.Packets;
using Server.Session;
using ServerCore;
using ServerCore.DebugPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketHandler
{
    public static void GAMECLIENT_CreateRoomCallHandler(PacketSession session, PacketInterface message)
    {
        ClientSession _session = session as ClientSession;

        if (_session != null) 
        {
            Console.WriteLine($"[Server]: 방 생성 요청 : {session.SessionID}");

            RoomManager.Instance.GenerateAndJoin(_session.player, 3);
        }
    }
}

