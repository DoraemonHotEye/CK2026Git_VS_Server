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

    public static void GAMECLIENT_RoomSendCallHandler(PacketSession session, PacketInterface message)
    {
        ClientSession _session = session as ClientSession;

        if (_session != null)
        {
            if (RoomManager.Instance.Count > 0)
            {
                MAINSERVER_SendRoomList _roomList = new MAINSERVER_SendRoomList();
                _roomList.RoomHashDatas = new PacketConvertList<string>();

                var HashList = RoomManager.Instance.GetRoomHashList();

                foreach (var Hash in HashList)
                {
                    _roomList.RoomHashDatas.Enter(Hash);
                }

                _session.Send(_roomList.Serialize());
            }
        }
    }

    public static void GAMECLIENT_JoinRoomCallHandler(PacketSession session, PacketInterface message)
    {
        ClientSession _session = session as ClientSession;

        if (_session == null)
        {
            Console.WriteLine($"[Server]: 현재 세션 상태가 연결이 끊어져 있습니다.");
            return;
        }

        if (_session.player == null)
        {
            Console.WriteLine($"[Server]: 현재 플레이어 연결 상태가 없습니다 {_session.SessionID}");
            return;
        }

        GAMECLIENT_JoinRoomCall _packet = message as GAMECLIENT_JoinRoomCall;
        Room FindRoom = RoomManager.Instance.GetRoom(_packet.RoomHashID);

        if (FindRoom == null)
        {
            Console.WriteLine($"[Server]: 현재 Room이 없습니다. {_packet.RoomHashID}");
            return;
        }

        if (FindRoom != null)
        {
            FindRoom.Enter(_session.player);
        }
    }

    public static void GAMECLIENT_RoomExitCallHandler(PacketSession session, PacketInterface message)
    {
        ClientSession _session = session as ClientSession;

        if (_session != null)
        {
            if (_session.player != null)
            {
                if (_session.player.CurrentRoom != null)
                {
                    _session.player.CurrentRoom.Remove(_session.player);

                    Console.WriteLine($"[Room]: 플레이어 퇴장 처리 {_session.SessionID}");
                }
            }
        }
    }
}

