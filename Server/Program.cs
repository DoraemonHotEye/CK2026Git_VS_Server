using Server.Session;
using Server.Session.Manager;
using Server.Session.UDP;
using ServerCore;
using ServerCore.DebugPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitySupportLibrary.Debug;
using UnitySupportLibrary.Game.Managers;

namespace Server
{
    public class Program
    {
        static Listener _listener = new Listener();
        static UClientSession UClientSession = new UClientSession();
        static void FlushPingUDP()
        {
            JobTimer.Instance.Push(FlushPingUDP, 1000);
        }

        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName(); // 내 컴퓨터 이름
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            IPEndPoint endPoint = new IPEndPoint(addresses[1], 5555);
            IPEndPoint UendPoint = new IPEndPoint(addresses[1], 5556);

            _listener.Start(endPoint, new Listener.OnListenerCallback(ClientSessionManager.Instance.Generate));
            UClientSession.Start(UendPoint);

            DebugConsole.PrintWrite("Listening...");

            JobTimer.Instance.Push(FlushPingUDP);

            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}
