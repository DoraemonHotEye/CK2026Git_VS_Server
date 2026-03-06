
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
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_CONNECTPACKET, MakePacket<DGRAM_SERVER_ConnectPacket>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_CONNECTPACKET, UPacketHandler.DGRAM_SERVER_ConnectPacketHandler);
	    _makeFunc.Add((ushort)MsgID.GAMECLIENT_DISCONNECTUDP, MakePacket<GAMECLIENT_DisconnectUDP>);
        _handler.Add((ushort)MsgID.GAMECLIENT_DISCONNECTUDP, UPacketHandler.GAMECLIENT_DisconnectUDPHandler);
        
    }

    public void OnRecvPacket(IPEndPoint ipendPoint, USession usession, ArraySegment<byte> buffer, Action<IPEndPoint, USession, PacketInterface> onRecvCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

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