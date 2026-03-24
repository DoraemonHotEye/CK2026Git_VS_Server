using MakePacketTemplate.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class RudpSendTable
{
    public delegate void ResendCallback(PacketInterface packet, uint seqNum);

    private class SendEntry
    {
        public uint SeqNum;
        public PacketInterface Packet;
        public long LastSendTimeMs;
        public int RetryCount;
        public int RetryIntervalMs;
        public ResendCallback Callback;

        public SendEntry(uint seqNum, PacketInterface packet, long nowMs, int retryIntervalMs, ResendCallback callback)
        {
            SeqNum = seqNum;
            Packet = packet;
            LastSendTimeMs = nowMs;
            RetryCount = 0;
            RetryIntervalMs = retryIntervalMs;
            Callback = callback;
        }
    }

    private readonly object _lock = new object();
    private readonly Dictionary<uint, SendEntry> _sendTable = new Dictionary<uint, SendEntry>();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public void Add(uint seqNum, PacketInterface packet, ResendCallback callback, int retryIntervalMs = 500)
    {
        lock (_lock)
        {
            _sendTable[seqNum] = new SendEntry(seqNum, packet, _stopwatch.ElapsedMilliseconds, retryIntervalMs, callback);
        }
    }

    public bool Ack(uint seqNum)
    {
        lock (_lock)
        {
            return _sendTable.Remove(seqNum);
        }
    }

    public void Update()
    {
        List<SendEntry> resendList = new List<SendEntry>();
        long nowMs = _stopwatch.ElapsedMilliseconds;

        lock (_lock)
        {
            foreach (var kv in _sendTable)
            {
                SendEntry entry = kv.Value;

                if (nowMs - entry.LastSendTimeMs >= entry.RetryIntervalMs)
                {
                    entry.LastSendTimeMs = nowMs;
                    entry.RetryCount++;
                    resendList.Add(entry);
                }
            }
        }

        foreach (var entry in resendList)
        {
            entry.Callback?.Invoke(entry.Packet, entry.SeqNum);
        }
    }
}