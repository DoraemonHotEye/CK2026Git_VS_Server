using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Listener
    {
        private Socket _listenSocket;

        SocketAsyncEventArgs AcceptEventArg;

        public delegate Session OnListenerCallback(Socket socket);

        OnListenerCallback onListenerCallback;

        private object _lock = new object();

        public void Start(IPEndPoint localEndPoint, OnListenerCallback onConnectedCallback)
        {
            // 1. 리슨 소켓 생성
            _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(localEndPoint);
            _listenSocket.Listen(100);

            AcceptEventArg = new SocketAsyncEventArgs();
            AcceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptCompleted);

            onListenerCallback = onConnectedCallback;

            // 2. 최초 Accept 준비
            RegisterAccept(AcceptEventArg);
        }

        private void RegisterAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg != null)
            {
                acceptEventArg.AcceptSocket = null; // 재사용을 위해 초기화
            }

            bool pending = _listenSocket.AcceptAsync(acceptEventArg);
            if (!pending)
            {
                // 즉시 완료되면 직접 처리
                AcceptCompleted(null, acceptEventArg);
            }
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            lock (_lock)
            {
                Socket clientSocket = e.AcceptSocket;

                if (onListenerCallback != null)
                {
                    Session session = onListenerCallback.Invoke(clientSocket);
                    session.OnConnected(clientSocket.RemoteEndPoint);
                }
                // 다음 연결을 계속 받기 위해 재시작
                RegisterAccept(e);
            }
        }
    }
}
