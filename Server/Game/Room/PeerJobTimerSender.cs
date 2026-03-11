using MakePacketTemplate.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public struct PeerDataPack
{
    public IPEndPoint endPoint;
    public string LocalAddress;
    public int SessionID;
}

public class PeerStationPackage
{
    public string HashRoomID;

    public List<Player> BroadCastList = new List<Player>();
    public Queue<PeerDataPack> SendIPEndPointContainer = new Queue<PeerDataPack>();

    public void EnterPlayers(List<Player> player)
    { 
        BroadCastList.AddRange(player); 
    }

    public void EnterPeers(List<PeerDataPack> peers) 
    {
        foreach(PeerDataPack peer in peers)
            SendIPEndPointContainer.Enqueue(peer);
    }

    private void BroadCastMessage(PacketInterface message)
    {
        foreach (var pl in BroadCastList)
        {
            pl.session.Send(message.Serialize());
        }
    }

    public bool BroadCast() //LockLess Zone
    {
        //TODO Queue에서 순차적 진행
        if (SendIPEndPointContainer.Count < 1)
            return true; //완료 더이상 보낼 필요 없음

        var EndPoint = SendIPEndPointContainer.Dequeue();

        if (EndPoint.endPoint == null)
            return false;

        MAINSERVER_SendPeerUser peer = new MAINSERVER_SendPeerUser();
        peer.PublicAddress = EndPoint.endPoint.Address.ToString();
        peer.LocalAddress = EndPoint.LocalAddress;
        peer.SessionID = EndPoint.SessionID;
        peer.Port = EndPoint.endPoint.Port;

        BroadCastMessage(peer);

        return false;
    }
}

public class PeerJobTimerSender
{
    private static PeerJobTimerSender Inst = null;
    public static PeerJobTimerSender Instance => Inst ?? (Inst = new PeerJobTimerSender());

    private object _lock = new object();

    private List<PeerStationPackage> PeerPackages = new List<PeerStationPackage>();

    public void RegisterSender(PeerStationPackage sender)
    {
        lock (_lock) //Locl Station #0941
        {
            PeerPackages.Add(sender);
        }
    }

    public void BroadCast()
    {
        lock (_lock) //Lock Station #0942
        {
            List<PeerStationPackage> DestroyStation = new List<PeerStationPackage>();

            foreach (var peer in PeerPackages)
            {
                bool succ = peer.BroadCast();

                if (succ == true)
                {
                    DestroyStation.Add(peer);
                }
            }

            if (DestroyStation.Count > 0)
            {
                foreach (var peer in DestroyStation)
                    PeerPackages.Remove(peer);
            }

        }
    }
}

