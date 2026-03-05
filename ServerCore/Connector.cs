using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Connector
    {
        public delegate Session OnConnectorCallback(Socket socket);

        OnConnectorCallback onConnectorCallback;

        public void Connect(IPEndPoint endPoint, OnConnectorCallback onConnectedCallback, int count = 1)
        {
            for (int i = 0; i < count; ++i)
            {
                Socket _clientSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs connectEventArgs  = new SocketAsyncEventArgs();
                connectEventArgs.RemoteEndPoint = endPoint;
                connectEventArgs.Completed += ConnectCompleted;

                onConnectorCallback = onConnectedCallback;

                RegisterConnect(_clientSocket, connectEventArgs);
            }
        }

        public void RegisterConnect(Socket clientSocket, SocketAsyncEventArgs ConnectEventArgs)
        {
            bool pending = clientSocket.ConnectAsync(ConnectEventArgs);
            if (!pending)
            {
                // 동기적으로 완료되었을 경우 직접 처리
                ConnectCompleted(null, ConnectEventArgs);
            }
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // TODO: 이후 ReceiveAsync, SendAsync 등을 수행
                if (onConnectorCallback != null)
                {
                    Session session = onConnectorCallback.Invoke(e.ConnectSocket);

                    session.OnConnected(e.ConnectSocket.RemoteEndPoint);
                }
            }
            else
            {
                Console.WriteLine($"연결 실패: {e.SocketError}");
            }
        }
    }
}
