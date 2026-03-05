using ServerCore;
using ServerCore.DebugPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    public class Program
    {
        static Listener _listener = new Listener();
        static Connector _connect = new Connector();

        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName(); // 내 컴퓨터 이름
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            IPEndPoint endPoint = new IPEndPoint(addresses[0], 5556);

            _listener.Start(endPoint, null);

            DebugConsole.PrintWrite("인증서버 - Listening...");

            while (true)
            {

            }
        }
    }
}
