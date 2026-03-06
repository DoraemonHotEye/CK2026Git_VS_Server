using MakePacketTemplate.Packets;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDgramCore
{
    public abstract class UPacketSession : USession
    {
        public static readonly int HeaderSize = 2;

        // [size(2)][packetId(2)][ ... ][size(2)][packetId(2)][ ... ]
        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;

            // 최소한 헤더는 파싱할 수 있는지 확인
            if (buffer.Count < HeaderSize)
                return 0;

            //패킷이 완전체로 도착했는지 확인
            ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            if (buffer.Count < dataSize)
                return 0; ;

            // 여기까지 왔으면 패킷 조립 가능
            OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, buffer.Count));

            return processLen;
        }
        public void Send(PacketInterface packet, IPEndPoint endPoint, bool RUDPUse = false)
        {
            var sendBuffer = packet.Serialize();

            lock (_lock)
            {
                _server.BeginSend(sendBuffer.Array, sendBuffer.Count, endPoint, udpSendCallback, _server);
            }

            OnSend(true);
        }

        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }

    public abstract class USession
    {
        public int SessionId = -1;

        //TODO PacketHandler 쪽에서 처리하게 추가하도록 처리함
        public Dictionary<int, IPEndPoint> _ipList = new Dictionary<int, IPEndPoint>();

        public UdpClient _server;
        public UdpClient SendServer;

        public IPEndPoint _holePunchServer;
        public IPEndPoint _recvServer;
        RecvBuffer _recvBuffer = new RecvBuffer(65535);

        public object _lock = new object();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public abstract int OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend(bool isCompeleted);

        void Clear()
        {
            lock (_lock)
            {
                _sendQueue.Clear();
                _pendingList.Clear();
            }
        }

        public void Start(IPEndPoint endPoint)
        {
            _server = new UdpClient(endPoint);
            SendServer = new UdpClient();

            Console.WriteLine("UDP Port: " + endPoint.Port);
            _server.BeginReceive(udpReceiveCallback, _server);
            ThreadPool.QueueUserWorkItem((WaitCallback)pingCallback);
        }

        public void SendCallBack(object state)
        {
            Console.WriteLine("-> 데이터 전송");
        }

        void pingCallback(object state)
        {
            lock (UPlayerManager.Instance.IPEndPoints)
            {
                foreach (var item in UPlayerManager.Instance.IPEndPoints)
                {
                    DGRAM_SERVER_Ping _pingpacket = new DGRAM_SERVER_Ping();
                    var serialize = _pingpacket.Serialize();
                    _server.BeginSend(serialize.Array, serialize.Count, item, udpSendCallback, _server);
                }
            }
            Thread.Sleep(5000); // 5초에 한번씩 클라이언트 목록 전송 (P2P 포트가 닫히는 것을 방지하기 위함)
            ThreadPool.QueueUserWorkItem((WaitCallback)pingCallback);
        }

        public void udpSendCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient udpClient = ar.AsyncState as UdpClient;
                udpClient.EndSend(ar);  // 싱크 마지막 값 보내기
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send Error: " + ex.ToString());
            }
        }

        public int _SessionID = 0;
        void udpReceiveCallback(IAsyncResult ar)
        {
            UdpClient udpServer = ar.AsyncState as UdpClient;
            IPEndPoint remoteEndPoint = null;

            try
            {
                byte[] receiveBytes = null;

                try
                {
                    lock (_lock)
                    {
                        receiveBytes = udpServer.EndReceive(ar, ref remoteEndPoint);

                        UPlayerManager.Instance.RegisterPlayer(remoteEndPoint);

                        Console.WriteLine("[UDP 받음]");

                        _recvServer = remoteEndPoint;

                        int processLen = OnRecv(new ArraySegment<byte>(receiveBytes, 0, receiveBytes.Length));
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("패킷Loss 감지 중간에 호스트가 끊겼습니다.");

                    IPEndPoint removeTarget = remoteEndPoint ?? _recvServer;

                    if (removeTarget != null)
                        UPlayerManager.Instance.RemovePlayer(removeTarget);
                }

                udpServer.BeginReceive(udpReceiveCallback, udpServer);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("UDP 소켓이 이미 종료되었습니다.");

                if (_recvServer != null)
                    UPlayerManager.Instance.RemovePlayer(_recvServer);
            }
            catch (Exception)
            {
                Console.WriteLine("패킷 강제 Loss 불안전한 연결입니다.");

                if (_recvServer != null)
                    UPlayerManager.Instance.RemovePlayer(_recvServer);
            }
        }

        private void OutputIPList()
        {
            foreach (var item in _ipList.Keys)
            {
                IPEndPoint ipAddr = _ipList[item];
                Console.WriteLine(item + " == " + ipAddr.ToString() + ":" + ipAddr.Port);
            }
        }

    }
}