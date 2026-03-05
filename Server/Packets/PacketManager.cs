
using MakePacketTemplate.Packets;
using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, PacketInterface>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, PacketInterface>>();
    Dictionary<ushort, Action<PacketSession, PacketInterface>> _handler = new Dictionary<ushort, Action<PacketSession, PacketInterface>>();

    public void Register()
    {
        
    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, PacketInterface> onRecvCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func<PacketSession, ArraySegment<byte>, PacketInterface> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            PacketInterface packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet, id);
        }
    }

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : PacketInterface, new()
    {
        T pkt = new T();
        pkt.Desrialize(buffer);
        return pkt;
    }

    public void HandlePacket(PacketSession session, PacketInterface packet, ushort ID)
    {
        Action<PacketSession, PacketInterface> action = null;
        if (_handler.TryGetValue(ID, out action))
            action.Invoke(session, packet);
    }
}