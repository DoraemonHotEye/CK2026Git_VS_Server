using MakePacketTemplate.Packets;
using Server.Session.Manager;
using ServerCore;
using ServerCore.DebugPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UDgramCore;
using UnitySupportLibrary.Game.Managers;
using UnitySupportLibrary.Game.Objects.Childs;
using static System.Collections.Specialized.BitVector32;

namespace Server.Session
{
    public class ClientSession : PacketSession
    {
        public Player player { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            DebugConsole.PrintWrite($"접속: {endPoint}");

            PlayerManager.Instance.Generate(this);

            MAINSERVER_SendSessionID _SessionID = new MAINSERVER_SendSessionID();
            _SessionID.SessionID = SessionID;

            Send(_SessionID.Serialize());
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            DebugConsole.PrintWrite($"접속 종료: {endPoint}");

            if (player != null)
                if (player.CurrentRoom != null)
                    player.CurrentRoom.Remove(player);


            if (player != null)
            {
                UIPEndPointContainer.Instance.RemovePlayer(player.PeerEndPoint);
                PlayerManager.Instance.Remove(this);
            }
            ClientSessionManager.Instance.Remove(this);
            UDPPlayerManager.Instance.Exit(SessionID);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int byteSended)
        {
            
        }
    }
}
