using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDgramCore
{
    public class UIPEndPointContainer
    {
        private static UIPEndPointContainer inst = null;
        public static UIPEndPointContainer Instance => inst ?? (inst = new UIPEndPointContainer());

        private List<IPEndPoint> PlayerList = new List<IPEndPoint>();

        public List<IPEndPoint> IPEndPoints { get {  return PlayerList; } }

        object _lock = new object();

        public void RegisterPlayer(IPEndPoint endPoint)
        {
            lock (_lock)
            {
                if (!PlayerList.Contains(endPoint))
                {
                    PlayerList.Add(endPoint);
                }
            }
        }

        public bool RemovePlayer(IPEndPoint endPoint)
        {
            lock (_lock)
            {
                return PlayerList.Remove(endPoint);
            }
        }

        public static IPEndPoint StringToIPEndPoint(string endpoint)
        {
            string[] parts = endpoint.Split(':');

            IPAddress ip = IPAddress.Parse(parts[0]);
            int port = int.Parse(parts[1]);

            return new IPEndPoint(ip, port);
        }
    }
}
