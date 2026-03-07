using MakePacketTemplate.Packets;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Room
{
    private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

    public string RoomHashData = "NONE_HASH_#ERROR_CODE";

    public ushort _MaxPlayerCount = 4;

    public Room(ushort MaxPlayerCount, string InitMapHashID)
    {
        _MaxPlayerCount = MaxPlayerCount;
        RoomHashData = InitMapHashID;
    }

    private List<Player> playerList = new List<Player>();

    public void BroadCastLock(PacketInterface message)
    {
        try
        {
            rwLock.EnterReadLock();

            foreach(var pl in playerList)
            {
                pl.session.Send(message.Serialize());
            }
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public void BroadCast(PacketInterface message)
    {
        foreach(var pl in playerList)
        {
            pl.session.Send(message.Serialize());
        }
    }

    public Room Enter(Player player)
    {
        try
        {
            rwLock.EnterWriteLock();

            if (player == null)
            {
                Console.WriteLine($"[Room]:: 유저 접속 중간에 player == null 감지");
                return null;
            }

            if (playerList.Contains(player))
            {
                Console.WriteLine("[Room] 이미 방에 있는 플레이어");
                return null;
            }

            if (player.session == null)
            {
                Console.WriteLine($"[Room]:: 유저 접속 중간에 player의 session == null 감지");
                return null;
            }

            if (playerList.Count >= _MaxPlayerCount)
            {
                Console.WriteLine($"[Room]:: 현재 방이 꽉차 플레이어를 입장 시킬 수 없습니다. {RoomHashData}");
                return null;
            }

            if (playerList.Count > 0)
            {
                MAINSERVER_OtherPlayerRoomJoin _OtherJoin = new MAINSERVER_OtherPlayerRoomJoin();
                _OtherJoin.SessionID = player.session.SessionID;
                _OtherJoin.PlayerList = new PacketConvertList<PlayerData>();

                foreach (var playerData in playerList) 
                {
                    PlayerData pl = new PlayerData();
                    pl.SessionID = playerData.session.SessionID;
                    _OtherJoin.PlayerList.Enter(pl);
                }

                BroadCast(_OtherJoin);
            }

            player.CurrentRoom = this;
            playerList.Add(player);

            MAINSERVER_CreateRoomSucc createRoomsucc = new MAINSERVER_CreateRoomSucc();
            createRoomsucc.timeout = 10;
            createRoomsucc.RoomHashData = RoomHashData;
            createRoomsucc.MaxPlayerCount = _MaxPlayerCount;
            

            player.session.Send(createRoomsucc.Serialize());

            Console.WriteLine($"[Room]: 방접속 완료 {player.session.SessionID} :-> MaxPlayerRoom {_MaxPlayerCount}");

            return this;
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
        
    }

    public bool Remove(Player player)
    {
        try
        {
            rwLock.EnterWriteLock();

            bool succ = playerList.Remove(player);

            if (succ == true)
            {
                player.CurrentRoom = null;
            }

            if (playerList.Count <= 0)  // 유저가 없으니 그 방의 RoomManager(그 방의 관리자)를 삭제
            {
                //유저가 전부 나감
                RoomManager.Instance.Remove(this);
            }

            return succ;
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
}
