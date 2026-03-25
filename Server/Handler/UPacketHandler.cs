using MakePacketTemplate.Packets;
using Server;
using Server.Session;
using Server.Session.Manager;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UDgramCore;

public class UPacketHandler
{
    public static void DGRAM_SERVER_PingHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        Console.WriteLine($"[Ping Packet] {Sender}");
    }

    public static void DGRAM_SERVER_AckSeqHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        DGRAM_SERVER_AckSeq _ack = message as DGRAM_SERVER_AckSeq;

        if (_ack != null)
        {
            bool ackremove = session.sendTable.Ack(_ack.SeqNum);
        
            if (ackremove == true)
            {
                DGRAM_SERVER_RequestAck requestAck = new DGRAM_SERVER_RequestAck();
                requestAck.SeqNum = _ack.SeqNum;

                session.SendServer.BeginSend(requestAck.Serialize().Array, requestAck.GetSize(), Sender, session.udpSendCallback, session.SendServer);
            }
        }
    }

    public static void DGRAM_SERVER_RequestAckHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        DGRAM_SERVER_RequestAck _req = message as DGRAM_SERVER_RequestAck;

        if (_req != null)
        {
            session.PeerRequestTable[Sender.ToString()].Remove(_req.SeqNum);
        }
    }

    public static void DGRAM_SERVER_ConnectPacketHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        Console.WriteLine($"[DGRAM_SERVER]: {Sender}");
    }

    public static void GAMECLIENT_DisconnectUDPHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        Console.WriteLine($"[DGRAM_SERVER]: Disconnecrt 감지 {Sender}");
        UIPEndPointContainer.Instance.RemovePlayer(Sender);
    }

    public static void GAMECLIENT_USessionIDMatchHandler(IPEndPoint Sender, USession session, PacketInterface message)
    {
        GAMECLIENT_USessionIDMatch UMatch = message as GAMECLIENT_USessionIDMatch;
        Console.WriteLine($"[DGRAM_SERVER]: {UMatch.SessionID}");

        UIPEndPointContainer.Instance.RegisterPlayer(Sender);
        UDPPlayerManager.Instance.Enter(UMatch.SessionID, Sender);

        var _session = ClientSessionManager.Instance.Get(UMatch.SessionID);

        if (_session != null)
        {
            if (_session.player != null)
            {
                _session.player.PeerEndPoint = Sender;
            }
        }
    }
}