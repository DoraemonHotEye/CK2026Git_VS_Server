
using MakePacketTemplate.Packets;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using UDgramCore;

public class UPacketManager
{
    #region Singleton
    static UPacketManager _instance = new UPacketManager();
    public static UPacketManager Instance { get { return _instance; } }
    #endregion

    UPacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<IPEndPoint, USession, ArraySegment<byte>, PacketInterface>> _makeFunc = new Dictionary<ushort, Func<IPEndPoint, USession, ArraySegment<byte>, PacketInterface>>();
    Dictionary<ushort, Action<IPEndPoint, USession, PacketInterface>> _handler = new Dictionary<ushort, Action<IPEndPoint, USession, PacketInterface>>();

    public void Register()
    {
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_PING, MakePacket<DGRAM_SERVER_Ping>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_PING, UPacketHandler.DGRAM_SERVER_PingHandler);
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_ACKSEQ, MakePacket<DGRAM_SERVER_AckSeq>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_ACKSEQ, UPacketHandler.DGRAM_SERVER_AckSeqHandler);
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_REQUESTACK, MakePacket<DGRAM_SERVER_RequestAck>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_REQUESTACK, UPacketHandler.DGRAM_SERVER_RequestAckHandler);
	    _makeFunc.Add((ushort)MsgID.GAMECLIENT_DISCONNECTUDP, MakePacket<GAMECLIENT_DisconnectUDP>);
        _handler.Add((ushort)MsgID.GAMECLIENT_DISCONNECTUDP, UPacketHandler.GAMECLIENT_DisconnectUDPHandler);
        _makeFunc.Add((ushort)MsgID.GAMECLIENT_USESSIONIDMATCH, MakePacket<GAMECLIENT_USessionIDMatch>);
        _handler.Add((ushort)MsgID.GAMECLIENT_USESSIONIDMATCH, UPacketHandler.GAMECLIENT_USessionIDMatchHandler);
        
    }

    public void OnRecvPacket(IPEndPoint ipendPoint, USession usession, ArraySegment<byte> buffer, Action<IPEndPoint, USession, PacketInterface> onRecvCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;
        ushort SeqNum = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);

        if (SeqNum != 0)
        {
            if (!usession.PeerRequestTable.ContainsKey(ipendPoint.ToString()))
                usession.PeerRequestTable.Add(ipendPoint.ToString(), new RudpRequestTable());

            bool succ = usession.PeerRequestTable[ipendPoint.ToString()].TryAdd(SeqNum);

            if (succ == true)
            {
                //TODO ACK 처리
                DGRAM_SERVER_AckSeq ackSeq = new DGRAM_SERVER_AckSeq();
                ackSeq.SeqNum = SeqNum;
                usession._server.Send(ackSeq.Serialize().Array, ackSeq.GetSize(), ipendPoint);
            }
            else 
            {
                return;
            }
        }

        Func<IPEndPoint, USession, ArraySegment<byte>, PacketInterface> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            PacketInterface packet = func.Invoke(ipendPoint, usession, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(ipendPoint, usession, packet);
            else
                HandlePacket(ipendPoint, usession, packet, id);
        }
    }

    T MakePacket<T>(IPEndPoint ipendPoint, USession usession, ArraySegment<byte> buffer) where T : PacketInterface, new()
    {
        T pkt = new T();
        pkt.Desrialize(buffer);
        return pkt;
    }

    public void HandlePacket(IPEndPoint session, USession usession, PacketInterface packet, ushort ID)
    {
        Action<IPEndPoint, USession, PacketInterface> action = null;
        if (_handler.TryGetValue(ID, out action))
            action.Invoke(session, usession, packet);
    }
}