using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


public class PlayerManager
{
    private static PlayerManager inst = null;
    public static PlayerManager Instance => inst ?? (inst = new PlayerManager());

    object _lock = new object();

    public Dictionary<ClientSession, Player> playerBinary = new Dictionary<ClientSession, Player>();

    public Player Generate(ClientSession session)
    {
        lock (_lock)
        {
            Player player = new Player();
            player.session = session;
            session.player = player;

            playerBinary.Add(session, player);

            Console.WriteLine($"[PlayerManager] == {session.SessionID}::Player생성 성공::");

            return player;
        }
    }

    public bool Remove(ClientSession session)
    {
        lock (_lock)
        {
            bool succ = playerBinary.Remove(session);
            
            if (succ == true)
                Console.WriteLine($"[PlayerManager] == {session.SessionID}::Player삭제 성공::");

            return succ;
        }
    }
}

