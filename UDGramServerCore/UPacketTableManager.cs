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

public class RudpRequestTable
{
    private class RequestEntry
    {
        public uint SeqNum;
        public long ReceivedTimeMs;

        public RequestEntry(uint seqNum, long receivedTimeMs)
        {
            SeqNum = seqNum;
            ReceivedTimeMs = receivedTimeMs;
        }
    }

    private readonly object _lock = new object();
    private readonly Dictionary<uint, RequestEntry> _requestTable = new Dictionary<uint, RequestEntry>();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private readonly int _keepAliveMs;

    public RudpRequestTable(int keepAliveMs = 5000)
    {
        _keepAliveMs = keepAliveMs;
    }
    public bool TryAdd(uint seqNum)
    {
        long nowMs = _stopwatch.ElapsedMilliseconds;

        lock (_lock)
        {
            if (_requestTable.ContainsKey(seqNum))
            {
                return false; // 이미 처리한 요청
            }

            _requestTable[seqNum] = new RequestEntry(seqNum, nowMs);
            return true; // 처음 들어온 요청
        }
    }
    public bool Contains(uint seqNum)
    {
        lock (_lock)
        {
            return _requestTable.ContainsKey(seqNum);
        }
    }

    public bool Remove(uint seqNum)
    {
        lock (_lock)
        {
            return _requestTable.Remove(seqNum);
        }
    }

    public void Update()
    {
        long nowMs = _stopwatch.ElapsedMilliseconds;
        List<uint> expiredList = new List<uint>();

        lock (_lock)
        {
            foreach (var kv in _requestTable)
            {
                RequestEntry entry = kv.Value;

                if (nowMs - entry.ReceivedTimeMs >= _keepAliveMs)
                {
                    expiredList.Add(entry.SeqNum);
                }
            }

            foreach (uint seqNum in expiredList)
            {
                _requestTable.Remove(seqNum);
            }
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _requestTable.Clear();
        }
    }
}