using DummyClient.Session;
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

namespace DummyClient
{
    public class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            DebugConsole.PrintWrite($"접속: {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            DebugConsole.PrintWrite($"접속 종료: {endPoint}");

            ServerSessionManager.Instance.Remove(this);
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
