using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDgramCore
{
    public class UPlayerManager
    {
        private static UPlayerManager inst = null;
        public static UPlayerManager Instance => inst ?? (inst = new UPlayerManager());

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

        public bool RemoverPlayer(IPEndPoint endPoint)
        {
            lock (_lock)
            {
                return PlayerList.Remove(endPoint);
            }
        }
    }
}
