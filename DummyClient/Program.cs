using DummyClient;
using DummyClient.Session;
using MakePacketTemplate.Packets;
using Server.Session.Manager;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    public class Program
    {
        static Connector _Connector = new Connector();
        
        static void FlushPingUDP()
        {
            JobTimer.Instance.Push(FlushPingUDP, 3000);
        }

        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName(); // 내 컴퓨터 이름
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            IPEndPoint endPoint = new IPEndPoint(addresses[1], 5555);
            IPEndPoint endPoint2 = new IPEndPoint(addresses[1], 5556);

            _Connector.Connect(endPoint, new Connector.OnConnectorCallback(ServerSessionManager.Instance.Generate), 4);
            
            JobTimer.Instance.Push(FlushPingUDP);

            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}
