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
    public string RoomHashData = "NONE_HASH_#ERROR_CODE";

    public ushort _MaxPlayerCount = 4;

    Player RoomMaster = null;
    public Room(ushort MaxPlayerCount, string InitMapHashID)
    {
        _MaxPlayerCount = MaxPlayerCount;
        RoomHashData = InitMapHashID;
    }

    private List<Player> playerList = new List<Player>();

    private object _lock = new object();

    public void BroadCast(PacketInterface message)
    {
        Player[] snapshot;

        lock (_lock)
        {
            snapshot = playerList.ToArray();
        }

        var data = message.Serialize();

        foreach (var pl in snapshot)
        {
            pl.session.Send(data);
        }
    }

    public void GameStart()
    {
        Player[] snapshot;
        List<PeerDataPack> peers;

        lock (_lock)
        {
            snapshot = playerList.ToArray();

            peers = new List<PeerDataPack>();

            foreach (var p in snapshot)
            {
                PeerDataPack peer = new PeerDataPack();
                peer.endPoint = p.PeerEndPoint;
                peer.SessionID = p.session.SessionID;
                peers.Add(peer);
            }
        }

        // lock 밖
        PeerStationPackage package = new PeerStationPackage();
        package.EnterPlayers(new List<Player>(snapshot));
        package.EnterPeers(peers);

        PeerJobTimerSender.Instance.RegisterSender(package);

        MAINSERVER_GameStartSucc start = new MAINSERVER_GameStartSucc();
        start.timeout = 10;

        BroadCast(start);
    }

    public Room Enter(Player player)
    {
        Player[] snapshot;
        bool notifyJoin = false;

        lock (_lock)
        {
            if (player == null)
            {
                Console.WriteLine($"[Room]:: player == null");
                return null;
            }

            if (playerList.Contains(player))
            {
                Console.WriteLine("[Room] 이미 방에 있는 플레이어");
                return null;
            }

            if (player.session == null)
            {
                Console.WriteLine($"[Room]:: session == null");
                return null;
            }

            if (playerList.Count >= _MaxPlayerCount)
            {
                Console.WriteLine($"[Room] 방이 꽉참 {RoomHashData}");
                return null;
            }

            notifyJoin = playerList.Count > 0;

            player.CurrentRoom = this;
            playerList.Add(player);

            if (playerList.Count == 1)
                RoomMaster = player;

            snapshot = playerList.ToArray();
        }

        // lock 밖

        if (notifyJoin)
        {
            var join = new MAINSERVER_OtherPlayerRoomJoin();
            join.OtherPlayer = new PlayerData();
            join.OtherPlayer.SessionID = player.session.SessionID;

            BroadCast(join);
        }

        var succ = new MAINSERVER_JoinRoomSucc();
        succ.RoomHashData = RoomHashData;
        succ.MaxPlayerCount = _MaxPlayerCount;
        succ.MasterPlayer = new PlayerData();
        succ.MasterPlayer.SessionID = RoomMaster.session.SessionID;
        succ.OtherPlayers = new PacketConvertList<PlayerData>();

        foreach (var pl in snapshot)
        {
            PlayerData pd = new PlayerData();
            pd.SessionID = pl.session.SessionID;
            succ.OtherPlayers.Enter(pd);
        }

        player.session.Send(succ.Serialize());

        Console.WriteLine($"[Room]: 방접속 완료 {player.session.SessionID}");

        return this;
    }

    public bool Remove(Player player)
    {
        Player[] snapshot;
        bool masterChanged = false;
        Player newMaster = null;

        lock (_lock)
        {
            if (!playerList.Contains(player))
                return false;

            playerList.Remove(player);
            player.CurrentRoom = null;

            if (playerList.Count == 0)
            {
                RoomManager.Instance.Remove(this);
            }
            else if (player == RoomMaster)
            {
                RoomMaster = playerList[0];
                newMaster = RoomMaster;
                masterChanged = true;
            }

            snapshot = playerList.ToArray();
        }

        // lock 밖
        if (masterChanged)
        {
            var msg = new MAINSERVER_RoomMasterChanged();
            msg.RoomMasterData = new PlayerData();
            msg.RoomMasterData.SessionID = newMaster.session.SessionID;

            BroadCast(msg);
        }

        var exit = new MAINSERVER_OtherPlayerRoomExit();
        exit.LeavePlayerData = new PlayerData();
        exit.LeavePlayerData.SessionID = player.session.SessionID;

        BroadCast(exit);

        return true;
    }
}
