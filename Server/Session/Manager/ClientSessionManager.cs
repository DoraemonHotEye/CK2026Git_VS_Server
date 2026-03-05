using MakePacketTemplate.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Manager
{
    public class ClientSessionManager
    {
        private static ClientSessionManager Inst;
        public static ClientSessionManager Instance => Inst ?? (Inst = new ClientSessionManager());

        private Dictionary<int, ClientSession> Sessions = new Dictionary<int, ClientSession>();

        int SessionCount = 0;

        object _lock = new object();

        public ClientSession Generate(Socket socket)
        {
            lock(_lock)
            {
                ClientSession session = new ClientSession();
                session.SessionID = SessionCount;
                session.Start(socket);
                Sessions.Add(SessionCount, session);

                SessionCount++;

                return session;
            }
        }

        public ClientSession Get(int SessionID)
        {
            lock(_lock)
            {
                if (!Sessions.ContainsKey(SessionID))
                    return null;

                return Sessions[SessionID];
            }
        }

        public bool Remove(int id)
        {
            lock (_lock)
            {
                return Sessions.Remove(id);
            }
        }

        public bool Remove(ClientSession session)
        {
            lock (_lock)
            {
                return Sessions.Remove(session.SessionID);
            }
        }
    }
}
