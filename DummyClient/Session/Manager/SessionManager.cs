using DummyClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Manager
{
    public class ServerSessionManager
    {
        private static ServerSessionManager Inst;
        public static ServerSessionManager Instance => Inst ?? (Inst = new ServerSessionManager());

        private Dictionary<int, ServerSession> Sessions = new Dictionary<int, ServerSession>();

        int SessionCount = 0;

        object _lock = new object();

        public ServerSession GetFirst()
        {
            if (Sessions.Count < 1)
                return null;

            return Sessions[0];
        }

        public ServerSession Generate(Socket socket)
        {
            lock(_lock)
            {
                ServerSession session = new ServerSession();
                session.SessionID = SessionCount;
                session.Start(socket);

                Sessions.Add(SessionCount, session);

                SessionCount++;

                return session;
            }
        }


        public ServerSession Get(int SessionID)
        {
            lock (_lock)
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

        public bool Remove(ServerSession session)
        {
            lock (_lock)
            {
                return Sessions.Remove(session.SessionID);
            }
        }
    }
}
