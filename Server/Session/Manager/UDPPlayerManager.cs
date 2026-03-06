using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Manager
{
    public class UDPPlayerManager
    {
        private static UDPPlayerManager inst = null;

        public static UDPPlayerManager Instance => inst ?? (inst = new UDPPlayerManager());

        private Dictionary<int, IPEndPoint> _PlayerEndPoints = new Dictionary<int, IPEndPoint>();

        private object _lock = new object();

        public void Enter(int SessionID, IPEndPoint iPEndPoint)
        {
            lock (_lock)
            {
                if(!_PlayerEndPoints.ContainsKey(SessionID))
                {
                    Console.WriteLine($"[UDP PlayerManager] == 등록 완료 UDP_IPEndPoint: {iPEndPoint}::ID::{SessionID}");
                    _PlayerEndPoints.Add(SessionID, iPEndPoint);
                }
            }
        }

        public bool Exit(int SessionID) 
        {
            lock (_lock)
            {
                bool succ = _PlayerEndPoints.Remove(SessionID);
                Console.WriteLine($"[UDP PlayerManager] == 삭제 완료 UDP::ID::{SessionID}");
                return succ;
            }
        }
    }
}
