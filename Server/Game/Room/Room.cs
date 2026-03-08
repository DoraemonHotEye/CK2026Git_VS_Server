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

    Player RoomMaster = null;
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

    public void GameStart()
    {
        try
        {
            rwLock.EnterReadLock();

            PeerStationPackage package = new PeerStationPackage();
            package.EnterPlayers(playerList);

            List<PeerDataPack> Peers = new List<PeerDataPack>();
            foreach (var p in playerList) 
            {
                PeerDataPack peer = new PeerDataPack();
                peer.endPoint = p.PeerEndPoint;
                peer.SessionID = p.session.SessionID;
                Peers.Add(peer);
            }

            package.EnterPeers(Peers);

            PeerJobTimerSender.Instance.RegisterSender(package);

            MAINSERVER_GameStartSucc _gamestart = new MAINSERVER_GameStartSucc();
            _gamestart.timeout = 10;
            BroadCast(_gamestart);
        }
        finally
        {
            rwLock.ExitReadLock();
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
                _OtherJoin.OtherPlayer = new PlayerData();
                _OtherJoin.OtherPlayer.SessionID = player.session.SessionID;

                BroadCast(_OtherJoin);
            }

            player.CurrentRoom = this;
            playerList.Add(player);

            if (playerList.Count == 1)
            {
                RoomMaster = player; //방장 설정
            }

            MAINSERVER_JoinRoomSucc JoinRoomSucc = new MAINSERVER_JoinRoomSucc();
            JoinRoomSucc.RoomHashData = RoomHashData;
            JoinRoomSucc.MaxPlayerCount = _MaxPlayerCount;
            JoinRoomSucc.MasterPlayer = new PlayerData();
            JoinRoomSucc.MasterPlayer.SessionID = RoomMaster.session.SessionID;
            JoinRoomSucc.OtherPlayers = new PacketConvertList<PlayerData>();

            foreach (var pl in playerList)
            {
                PlayerData pld = new PlayerData();
                pld.SessionID = pl.session.SessionID;
                JoinRoomSucc.OtherPlayers.Enter(pld);
            }

            player.session.Send(JoinRoomSucc.Serialize());


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

            if (succ == false)
                return false;

            player.CurrentRoom = null;

            MAINSERVER_RoomExitSucc exitsucc = new MAINSERVER_RoomExitSucc();
            exitsucc.timeout = 10;
            player.session.Send(exitsucc.Serialize());

            if (playerList.Count <= 0)  // 유저가 없으니 그 방의 RoomManager(그 방의 관리자)를 삭제
            {
                //유저가 전부 나감
                RoomManager.Instance.Remove(this);
            }
            else
            {
                if (player == RoomMaster)
                {
                    RoomMaster = playerList[0]; //일단 제일 첫번쨰 친구를 방장으로 설정

                    MAINSERVER_RoomMasterChanged _changedRoomMaster = new MAINSERVER_RoomMasterChanged();
                    _changedRoomMaster.RoomMasterData = new PlayerData();
                    _changedRoomMaster.RoomMasterData.SessionID = RoomMaster.session.SessionID;

                    BroadCast(_changedRoomMaster);

                    Console.WriteLine($"[Room]: 방장 위임 완료 {RoomMaster.session.SessionID}");
                }
            }

            MAINSERVER_OtherPlayerRoomExit exitRoomOther = new MAINSERVER_OtherPlayerRoomExit();
            exitRoomOther.LeavePlayerData = new PlayerData();
            exitRoomOther.LeavePlayerData.SessionID = player.session.SessionID;

            BroadCast(exitRoomOther);

            Console.WriteLine($"[Room]: 퇴장 처리 완료 {player.session.SessionID}");

            return succ;
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
}
