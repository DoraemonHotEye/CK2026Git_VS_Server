using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class Room
{
    private object _lock = new object();

    public string MapHashID = "NONE_HASH_#ERROR_CODE";

    public ushort _MaxPlayerCount = 4;

    public Room(ushort MaxPlayerCount, string InitMapHashID)
    {
        _MaxPlayerCount = MaxPlayerCount;
        MapHashID = InitMapHashID;
    }

    private List<Player> playerList = new List<Player>();

    public Room Enter(Player player)
    {
        lock (_lock)
        {
            if (player == null)
            {
                Console.WriteLine($"[Room]:: 유저 접속 중간에 player == null 감지");
                return null;
            }

            if (player.session == null)
            {
                Console.WriteLine($"[Room]:: 유저 접속 중간에 player의 session == null 감지");
            }

            if (playerList.Count >= _MaxPlayerCount)
            {
                Console.WriteLine($"[Room]:: 현재 방이 꽉차 플레이어를 입장 시킬 수 없습니다. {MapHashID}");
                return null;
            }

            player.CurrentRoom = this;
            playerList.Add(player);

            Console.WriteLine($"[Room]: 방접속 완료 {_MaxPlayerCount}");

            return this;
        }
    }

    public bool Remove(Player player)
    {
        lock (_lock)
        {
            bool succ = playerList.Remove(player);

            if (playerList.Count <= 0)  // 유저가 없으니 그 방의 RoomManager(그 방의 관리자)를 삭제
            {
                //유저가 전부 나감
                RoomManager.Instance.Remove(this);
            }

            return succ;
        }
    }
}
