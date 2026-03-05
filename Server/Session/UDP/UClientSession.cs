using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDgramCore;

namespace Server.Session.UDP
{
    public class UClientSession : UPacketSession
    {
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            UPacketManager.Instance.OnRecvPacket(_recvServer, this, buffer);
        }

        public override void OnSend(bool isCompeleted)
        {

        }
    }
}
