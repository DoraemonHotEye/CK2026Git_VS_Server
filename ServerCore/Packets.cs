
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakePacketTemplate.Packets
{
    public interface PacketInterface
    {
        ArraySegment<byte> Serialize();
        void Desrialize(ArraySegment<byte> data);
        ushort GetSize();
    }

    public interface PacketObject
    {
        Tuple<ushort, ArraySegment<byte>> GetPacketSerialize();
        void Deserialize(ArraySegment<byte> data);
        ushort GetSize();
    }

    public class PacketConvertList<T>
    {
        public List<T> Values = new List<T>();

        public void Enter(T value)
        {
            Values.Add(value);
        }

        public ushort GetCount() => (ushort)Values.Count;

        public void Empty() { Values.Clear(); }

        // 참조 타입 (PacketObject)
        public ushort GetSize_RefType<P>() where P : class, PacketObject
        {
            ushort size = 0;
            

            foreach (var item in Values)
            {
                if (item != null)
                    size += ((PacketObject)item).GetSize();
            }

            return size;
        }

        public T this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        // 값 타입 (기본형)
        public ushort GetSize_Struct()
        {
            ushort size = 0;

            size += (ushort)(Values.Count * System.Runtime.InteropServices.Marshal.SizeOf<T>());

            return size;
        }
    }

    

    
    public struct PlayerData : PacketObject
    {
        public int SessionID;
        
        
        public ushort GetSize()
        {
            ushort size = 0;
               
            size += sizeof(int);
            
            
    
            return size;
        }
    
        public Tuple<ushort, ArraySegment<byte>> GetPacketSerialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            int offset = 0;
               
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(int);
            
    
            return new Tuple<ushort, ArraySegment<byte>>(size, new ArraySegment<byte>(buffer, 0, size));
        }
        
        public void Deserialize(ArraySegment<byte> data)
        {
            int offset = 0;
            byte[] buffer = data.Array;
            int start = data.Offset;

            SessionID = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            
        }
    }


    public enum MsgID : ushort
    {
         GAMECLIENT_DISCONNECTUDP = 0,
         GAMECLIENT_USESSIONIDMATCH = 1,
         GAMECLIENT_CREATEROOMCALL = 2,
         GAMECLIENT_ROOMSENDCALL = 3,
         GAMECLIENT_JOINROOMCALL = 4,
         GAMECLIENT_ROOMEXITCALL = 5,
         GAMECLIENT_GAMESTART = 6,
         GAMECLIENT_UMOVEPACKET = 7,
         GAMECLIENT_UPOSSYNCPACKET = 8,
         GAMECLIENT_PEERPINGPACKET = 9,
         GAMECLIENT_PEERPINGACK = 10,
         GAMECLIENT_UJUMPPACKET = 11,
         GAMECLIENT_UJUMPENDPACKET = 12,
         GAMECLIENT_GAMESTARTREADY = 13,
         GAMECLIENT_SENDLOCALENDPOINT = 14,
         GAMECLIENT_MONSTERMOVESYNC = 15,
         GAMECLIENT_MONSTERSYNCPOS = 16,
         GAMECLIENT_UDOWNJUMPPACKET = 17,
         GAMECLIENT_ENEMYPROJECTILEFIRE = 18,
         GAMECLIENT_PLAYERATTACK = 19,
         GAMECLIENT_MONSTERLOCKFLIP = 20,
         GAMECLIENT_MONSTERATTACK = 21,
         GAMECLIENT_MONSTERATTACKSTOP = 22,
         GAMECLIENT_TAKEDAMAGEMONSTER = 23,
         GAMECLIENT_MONSTERDIE = 24,
         GAMECLIENT_PLAYERHIT = 25,
         GAMECLIENT_PLAYERDIE = 26,
         GAMECLIENT_MONSTERJUMP = 27,
         GAMECLIENT_MONSTERUSEGRAVITY = 28,
         GAMECLIENT_MONSTERSWITCHANIM = 29,
         GAMECLIENT_PROJECTILESTOP = 30,
         GAMECLIENT_MONSTERVELOCITYSET = 31,
         GAMECLIENT_PROJECTILEDISABLECALL = 32,
         GAMECLIENT_GROUNDCHECK = 33,
         GAMECLIENT_MONSTERGRABSTART = 34,
         GAMECLIENT_MONSTERTELEPORT = 35,
         GAMECLIENT_MONSTERGRABING = 36,
         GAMECLIENT_MONSTERGRABEND = 37,
         GAMECLIENT_RPCCALL = 38,
         GAMECLIENT_SWITCHANIMINT = 39,
         GAMECLIENT_LANDINGSYNCPOS = 40,
         MAINSERVER_SENDSESSIONID = 41,
         MAINSERVER_OTHERPLAYERROOMJOIN = 42,
         MAINSERVER_SENDROOMLIST = 43,
         MAINSERVER_JOINROOMSUCC = 44,
         MAINSERVER_ROOMMASTERCHANGED = 45,
         MAINSERVER_ROOMEXITSUCC = 46,
         MAINSERVER_OTHERPLAYERROOMEXIT = 47,
         MAINSERVER_NOROOMCALLBACK = 48,
         MAINSERVER_SENDPEERUSER = 49,
         MAINSERVER_GAMESTARTSUCC = 50,
         MAINSERVER_SENDPBENDPOINT = 51,
         MAINSERVER_ALLCOMPELETEDREADY = 52,
         
         DGRAM_SERVER_REQUESTACK = 25562,
         DGRAM_SERVER_ACKSEQ = 25563,
         DGRAM_SERVER_PING = 25564,
         DGRAM_SERVER_CONNECTPACKET = 25565
    }

    public class DGRAM_SERVER_RequestAck : PacketInterface
    {
        public MsgID MsgID = MsgID.DGRAM_SERVER_REQUESTACK;
        public ushort SeqNum;
        public ushort ReqSeqNum;

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ReqSeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum
            size += sizeof(ushort); //ReqSeqNum

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)ReqSeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }

    public class DGRAM_SERVER_AckSeq : PacketInterface
    {
        public MsgID MsgID = MsgID.DGRAM_SERVER_ACKSEQ;
        public ushort SeqNum;

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((uint)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }

    public class DGRAM_SERVER_Ping : PacketInterface
    {
        public MsgID MsgID = MsgID.DGRAM_SERVER_PING;
        public ushort SeqNum;

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((uint)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }


    public class DGRAM_SERVER_ConnectPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.DGRAM_SERVER_CONNECTPACKET;
        public ushort SeqNum;
        public uint SessionID;

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SessionID = BitConverter.ToUInt32(buffer, start + offset);
            offset += sizeof(uint);
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum
            size += sizeof(uint);

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(SessionID), 0, buffer, offset, sizeof(uint));
            offset += sizeof(uint);

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }

    
    public class GAMECLIENT_DisconnectUDP : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_DISCONNECTUDP;
        public ushort SeqNum;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_USessionIDMatch : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_USESSIONIDMATCH;
        public ushort SeqNum;

        public int SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SessionID = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(int);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(int);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_CreateRoomCall : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_CREATEROOMCALL;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_RoomSendCall : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_ROOMSENDCALL;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_JoinRoomCall : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_JOINROOMCALL;

        public string RoomHashID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort RoomHashIDLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            RoomHashID = Encoding.UTF8.GetString(buffer, start + offset, RoomHashIDLength);
            offset += (ushort)RoomHashIDLength;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(RoomHashID);
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            byte[] RoomHashIDBytes = Encoding.UTF8.GetBytes(RoomHashID);
            BitConverter.GetBytes((ushort)RoomHashIDBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            RoomHashIDBytes.CopyTo(buffer, offset);
            offset += (ushort)RoomHashIDBytes.Length;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_RoomExitCall : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_ROOMEXITCALL;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_GameStart : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_GAMESTART;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_UMovepacket : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_UMOVEPACKET;
        public ushort SeqNum;

        public short XDir;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            XDir = BitConverter.ToInt16(buffer, start + offset);
            offset += sizeof(short);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(short);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(XDir).CopyTo(buffer, offset);        
            offset += sizeof(short);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_UPosSyncPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_UPOSSYNCPACKET;
        public ushort SeqNum;

        public float x;
        public float y;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            x = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            y = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(x).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(y).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_PeerPingPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PEERPINGPACKET;
        public ushort SeqNum;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_PeerPingACK : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PEERPINGACK;
        public ushort SeqNum;

        public ushort SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SessionID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_UJumpPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_UJUMPPACKET;
        public ushort SeqNum;

        public float JumpPos;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            JumpPos = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(JumpPos).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_UJumpEndPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_UJUMPENDPACKET;
        public ushort SeqNum;

        public float currY;
        public float currX;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            currY = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            currX = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(currY).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(currX).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_GameStartReady : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_GAMESTARTREADY;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_SendLocalEndPoint : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_SENDLOCALENDPOINT;

        public string LocalAddress;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort LocalAddressLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            LocalAddress = Encoding.UTF8.GetString(buffer, start + offset, LocalAddressLength);
            offset += (ushort)LocalAddressLength;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(LocalAddress);
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            byte[] LocalAddressBytes = Encoding.UTF8.GetBytes(LocalAddress);
            BitConverter.GetBytes((ushort)LocalAddressBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            LocalAddressBytes.CopyTo(buffer, offset);
            offset += (ushort)LocalAddressBytes.Length;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterMoveSync : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERMOVESYNC;
        public ushort SeqNum;

        public short XDir;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            XDir = BitConverter.ToInt16(buffer, start + offset);
            offset += sizeof(short);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(short);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(XDir).CopyTo(buffer, offset);        
            offset += sizeof(short);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterSyncPos : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERSYNCPOS;
        public ushort SeqNum;

        public float x;
        public float y;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            x = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            y = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(x).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(y).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_UDownJumpPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_UDOWNJUMPPACKET;
        public ushort SeqNum;

        public float duration;
        public float downSpeed;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            duration = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            downSpeed = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(duration).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(downSpeed).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_EnemyProjectileFire : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_ENEMYPROJECTILEFIRE;
        public ushort SeqNum;

        public ushort OwnerID;
        public ushort NetID;
        public float dirX;
        public float dirY;
        public float posX;
        public float posY;
        public ushort CarrierIdx;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            OwnerID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            dirX = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            dirY = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            posX = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            posY = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            CarrierIdx = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(OwnerID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(dirX).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(dirY).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(posX).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(posY).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(CarrierIdx).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_PlayerAttack : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PLAYERATTACK;
        public ushort SeqNum;

        public ushort NetID;
        public ushort Combo;
        public short FlipX;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            Combo = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            FlipX = BitConverter.ToInt16(buffer, start + offset);
            offset += sizeof(short);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            size += sizeof(short);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(Combo).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(FlipX).CopyTo(buffer, offset);        
            offset += sizeof(short);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterLockFlip : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERLOCKFLIP;
        public ushort SeqNum;

        public short LockType;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            LockType = BitConverter.ToInt16(buffer, start + offset);
            offset += sizeof(short);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(short);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(LockType).CopyTo(buffer, offset);        
            offset += sizeof(short);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterAttack : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERATTACK;
        public ushort SeqNum;

        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterAttackStop : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERATTACKSTOP;
        public ushort SeqNum;

        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_TakeDamageMonster : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_TAKEDAMAGEMONSTER;
        public ushort SeqNum;

        public ushort NetID;
        public ushort SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            SessionID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterDie : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERDIE;
        public ushort SeqNum;

        public ushort NetID;
        public ushort SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            SessionID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_PlayerHit : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PLAYERHIT;
        public ushort SeqNum;

        public ushort NetID;
        public ushort SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            SessionID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_PlayerDie : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PLAYERDIE;
        public ushort SeqNum;

        public ushort NetID;
        public ushort SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            SessionID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterJump : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERJUMP;
        public ushort SeqNum;

        public float JumpPower;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            JumpPower = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(JumpPower).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterUseGravity : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERUSEGRAVITY;
        public ushort SeqNum;

        public ushort switchtype;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            switchtype = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(switchtype).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterSwitchAnim : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERSWITCHANIM;
        public ushort SeqNum;

        public ushort NetID;
        public ushort AnimCode;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            AnimCode = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(AnimCode).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_ProjectileStop : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PROJECTILESTOP;
        public ushort SeqNum;

        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterVelocitySet : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERVELOCITYSET;
        public ushort SeqNum;

        public float x;
        public float y;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            x = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            y = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(x).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(y).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_ProjectileDisableCall : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_PROJECTILEDISABLECALL;
        public ushort SeqNum;

        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_GroundCheck : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_GROUNDCHECK;
        public ushort SeqNum;

        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterGrabStart : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERGRABSTART;
        public ushort SeqNum;

        public ushort NetID;
        public ushort OwnerID;
        public float posX;
        public float posY;
        public ushort SessionID;
        public ushort CarrierIdx;
        public ushort TargetIdx;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            OwnerID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            posX = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            posY = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            SessionID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            CarrierIdx = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            TargetIdx = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(OwnerID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(posX).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(posY).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(CarrierIdx).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(TargetIdx).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterTeleport : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERTELEPORT;
        public ushort SeqNum;

        public ushort NetID;
        public float PosX;
        public float PosY;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            PosX = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            PosY = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(float);
            
            size += sizeof(float);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(PosX).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(PosY).CopyTo(buffer, offset);        
            offset += sizeof(float);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterGrabIng : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERGRABING;
        public ushort SeqNum;

        public ushort NetID;
        public ushort TargetID;
        public float posX;
        public float posY;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            TargetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            posX = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            posY = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            
            size += sizeof(float);
            
            size += sizeof(float);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(TargetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            BitConverter.GetBytes(posX).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(posY).CopyTo(buffer, offset);        
            offset += sizeof(float);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_MonsterGrabEnd : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_MONSTERGRABEND;
        public ushort SeqNum;

        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_RpcCall : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_RPCCALL;
        public ushort SeqNum;

        public string JsonHash;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort JsonHashLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            JsonHash = Encoding.UTF8.GetString(buffer, start + offset, JsonHashLength);
            offset += (ushort)JsonHashLength;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(JsonHash);
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            byte[] JsonHashBytes = Encoding.UTF8.GetBytes(JsonHash);
            BitConverter.GetBytes((ushort)JsonHashBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            JsonHashBytes.CopyTo(buffer, offset);
            offset += (ushort)JsonHashBytes.Length;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_SwitchAnimInt : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_SWITCHANIMINT;
        public ushort SeqNum;

        public ushort AnimCode;
        public string ParamName;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            AnimCode = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            ushort ParamNameLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            ParamName = Encoding.UTF8.GetString(buffer, start + offset, ParamNameLength);
            offset += (ushort)ParamNameLength;
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(ParamName);
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(AnimCode).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            byte[] ParamNameBytes = Encoding.UTF8.GetBytes(ParamName);
            BitConverter.GetBytes((ushort)ParamNameBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            ParamNameBytes.CopyTo(buffer, offset);
            offset += (ushort)ParamNameBytes.Length;
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class GAMECLIENT_LandingSyncPos : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_LANDINGSYNCPOS;
        public ushort SeqNum;

        public float x;
        public float y;
        public ushort NetID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            x = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            y = BitConverter.ToSingle(buffer, start + offset);
            offset += sizeof(float);
            NetID = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            size += sizeof(float);
            
            size += sizeof(float);
            
            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(x).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(y).CopyTo(buffer, offset);        
            offset += sizeof(float);
            BitConverter.GetBytes(NetID).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_SendSessionID : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_SENDSESSIONID;

        public int SessionID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SessionID = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(int);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(int);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_OtherPlayerRoomJoin : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_OTHERPLAYERROOMJOIN;

        public PlayerData OtherPlayer;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort OtherPlayerPacketSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            OtherPlayer = new PlayerData();
            byte[] OtherPlayerSliceCopied = new byte[OtherPlayerPacketSize];
            Array.Copy(buffer, start + offset, OtherPlayerSliceCopied, 0, OtherPlayerPacketSize);
            var OtherPlayerArray = new ArraySegment<byte>(OtherPlayerSliceCopied);
            OtherPlayer.Deserialize(OtherPlayerArray);
            offset += OtherPlayerPacketSize;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += OtherPlayer.GetSize();
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            var (OtherPlayerPacketSize, OtherPlayerSerialize) = OtherPlayer.GetPacketSerialize();
            Array.Copy(BitConverter.GetBytes((ushort)OtherPlayerPacketSize), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(OtherPlayerSerialize.Array, OtherPlayerSerialize.Offset, buffer, offset, OtherPlayerPacketSize);
            offset += OtherPlayerPacketSize;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_SendRoomList : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_SENDROOMLIST;

        public PacketConvertList<string> RoomHashDatas;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            RoomHashDatas = new PacketConvertList<string>();
            int RoomHashDatasCount = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            for(int i = 0; i < RoomHashDatasCount; ++i)
            {
                ushort strLength = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);
                string tempString = Encoding.UTF8.GetString(buffer, start + offset, strLength);
                offset += (ushort)strLength;
                RoomHashDatas.Enter(tempString);
            }
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            
            size += sizeof(int);
            foreach(var item in RoomHashDatas.Values)
            {
                size += sizeof(ushort);
                size += (ushort)Encoding.UTF8.GetByteCount(item);
            }
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(RoomHashDatas.GetCount()).CopyTo(buffer, offset);
            offset += sizeof(int);
            foreach (var str in RoomHashDatas.Values)
            {
                byte[] strBytes = Encoding.UTF8.GetBytes(str);
                BitConverter.GetBytes((ushort)strBytes.Length).CopyTo(buffer, offset);
                offset += sizeof(ushort);
                strBytes.CopyTo(buffer, offset);
                offset += (ushort)strBytes.Length;
            }
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_JoinRoomSucc : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_JOINROOMSUCC;

        public PlayerData MasterPlayer;
        public string RoomHashData;
        public ushort MaxPlayerCount;
        public PacketConvertList<PlayerData> OtherPlayers;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort MasterPlayerPacketSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            MasterPlayer = new PlayerData();
            byte[] MasterPlayerSliceCopied = new byte[MasterPlayerPacketSize];
            Array.Copy(buffer, start + offset, MasterPlayerSliceCopied, 0, MasterPlayerPacketSize);
            var MasterPlayerArray = new ArraySegment<byte>(MasterPlayerSliceCopied);
            MasterPlayer.Deserialize(MasterPlayerArray);
            offset += MasterPlayerPacketSize;
            ushort RoomHashDataLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            RoomHashData = Encoding.UTF8.GetString(buffer, start + offset, RoomHashDataLength);
            offset += (ushort)RoomHashDataLength;
            MaxPlayerCount = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            ushort OtherPlayersCount = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            OtherPlayers = new PacketConvertList<PlayerData>();
            OtherPlayers.Empty(); // 기존 리스트 비우기

            for (int i = 0; i < OtherPlayersCount; i++)
            {
                ushort objectsize = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);

                PlayerData packet = new PlayerData();
                byte[] copied = new byte[objectsize];
                Array.Copy(buffer, start + offset, copied, 0, objectsize);
                var slice = new ArraySegment<byte>(copied);

                packet.Deserialize(slice);
                OtherPlayers.Enter(packet);

                offset += objectsize;
            }
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += MasterPlayer.GetSize();
            
            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(RoomHashData);
            size += sizeof(ushort);
            
            size += sizeof(ushort);
            size += (ushort)(sizeof(ushort) * (OtherPlayers.GetCount() == 0 ? 1 : OtherPlayers.GetCount()));
            size += OtherPlayers.GetSize_RefType<PacketObject>();
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            var (MasterPlayerPacketSize, MasterPlayerSerialize) = MasterPlayer.GetPacketSerialize();
            Array.Copy(BitConverter.GetBytes((ushort)MasterPlayerPacketSize), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(MasterPlayerSerialize.Array, MasterPlayerSerialize.Offset, buffer, offset, MasterPlayerPacketSize);
            offset += MasterPlayerPacketSize;
            byte[] RoomHashDataBytes = Encoding.UTF8.GetBytes(RoomHashData);
            BitConverter.GetBytes((ushort)RoomHashDataBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            RoomHashDataBytes.CopyTo(buffer, offset);
            offset += (ushort)RoomHashDataBytes.Length;
            BitConverter.GetBytes(MaxPlayerCount).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes((ushort)OtherPlayers.GetCount()), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            foreach (var packet in OtherPlayers.Values)
            {
                var (packetSize, segment) = packet.GetPacketSerialize();
                Array.Copy(BitConverter.GetBytes((ushort)packetSize), 0, buffer, offset, sizeof(ushort));
                offset += sizeof(ushort);
                Array.Copy(segment.Array, segment.Offset, buffer, offset, packetSize);
                offset += packetSize;
            }
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_RoomMasterChanged : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_ROOMMASTERCHANGED;

        public PlayerData RoomMasterData;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort RoomMasterDataPacketSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            RoomMasterData = new PlayerData();
            byte[] RoomMasterDataSliceCopied = new byte[RoomMasterDataPacketSize];
            Array.Copy(buffer, start + offset, RoomMasterDataSliceCopied, 0, RoomMasterDataPacketSize);
            var RoomMasterDataArray = new ArraySegment<byte>(RoomMasterDataSliceCopied);
            RoomMasterData.Deserialize(RoomMasterDataArray);
            offset += RoomMasterDataPacketSize;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += RoomMasterData.GetSize();
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            var (RoomMasterDataPacketSize, RoomMasterDataSerialize) = RoomMasterData.GetPacketSerialize();
            Array.Copy(BitConverter.GetBytes((ushort)RoomMasterDataPacketSize), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(RoomMasterDataSerialize.Array, RoomMasterDataSerialize.Offset, buffer, offset, RoomMasterDataPacketSize);
            offset += RoomMasterDataPacketSize;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_RoomExitSucc : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_ROOMEXITSUCC;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_OtherPlayerRoomExit : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_OTHERPLAYERROOMEXIT;

        public PlayerData LeavePlayerData;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort LeavePlayerDataPacketSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            LeavePlayerData = new PlayerData();
            byte[] LeavePlayerDataSliceCopied = new byte[LeavePlayerDataPacketSize];
            Array.Copy(buffer, start + offset, LeavePlayerDataSliceCopied, 0, LeavePlayerDataPacketSize);
            var LeavePlayerDataArray = new ArraySegment<byte>(LeavePlayerDataSliceCopied);
            LeavePlayerData.Deserialize(LeavePlayerDataArray);
            offset += LeavePlayerDataPacketSize;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += LeavePlayerData.GetSize();
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            var (LeavePlayerDataPacketSize, LeavePlayerDataSerialize) = LeavePlayerData.GetPacketSerialize();
            Array.Copy(BitConverter.GetBytes((ushort)LeavePlayerDataPacketSize), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(LeavePlayerDataSerialize.Array, LeavePlayerDataSerialize.Offset, buffer, offset, LeavePlayerDataPacketSize);
            offset += LeavePlayerDataPacketSize;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_NoRoomCallback : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_NOROOMCALLBACK;

        public string ReturnHashID;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort ReturnHashIDLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            ReturnHashID = Encoding.UTF8.GetString(buffer, start + offset, ReturnHashIDLength);
            offset += (ushort)ReturnHashIDLength;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(ReturnHashID);
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            byte[] ReturnHashIDBytes = Encoding.UTF8.GetBytes(ReturnHashID);
            BitConverter.GetBytes((ushort)ReturnHashIDBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            ReturnHashIDBytes.CopyTo(buffer, offset);
            offset += (ushort)ReturnHashIDBytes.Length;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_SendPeerUser : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_SENDPEERUSER;

        public int SessionID;
        public string LocalAddress;
        public string PublicAddress;
        public int Port;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SessionID = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            ushort LocalAddressLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            LocalAddress = Encoding.UTF8.GetString(buffer, start + offset, LocalAddressLength);
            offset += (ushort)LocalAddressLength;
            ushort PublicAddressLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            PublicAddress = Encoding.UTF8.GetString(buffer, start + offset, PublicAddressLength);
            offset += (ushort)PublicAddressLength;
            Port = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(int);
            
            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(LocalAddress);
            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(PublicAddress);
            size += sizeof(int);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
            offset += sizeof(int);
            byte[] LocalAddressBytes = Encoding.UTF8.GetBytes(LocalAddress);
            BitConverter.GetBytes((ushort)LocalAddressBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            LocalAddressBytes.CopyTo(buffer, offset);
            offset += (ushort)LocalAddressBytes.Length;
            byte[] PublicAddressBytes = Encoding.UTF8.GetBytes(PublicAddress);
            BitConverter.GetBytes((ushort)PublicAddressBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            PublicAddressBytes.CopyTo(buffer, offset);
            offset += (ushort)PublicAddressBytes.Length;
            BitConverter.GetBytes(Port).CopyTo(buffer, offset);        
            offset += sizeof(int);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_GameStartSucc : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_GAMESTARTSUCC;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_SendPbEndPoint : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_SENDPBENDPOINT;

        public string IPAddress;
        public int Port;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort IPAddressLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            IPAddress = Encoding.UTF8.GetString(buffer, start + offset, IPAddressLength);
            offset += (ushort)IPAddressLength;
            Port = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(IPAddress);
            size += sizeof(int);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            byte[] IPAddressBytes = Encoding.UTF8.GetBytes(IPAddress);
            BitConverter.GetBytes((ushort)IPAddressBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            IPAddressBytes.CopyTo(buffer, offset);
            offset += (ushort)IPAddressBytes.Length;
            BitConverter.GetBytes(Port).CopyTo(buffer, offset);        
            offset += sizeof(int);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_AllCompeletedReady : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_ALLCOMPELETEDREADY;

        public ushort timeout;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            timeout = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            

            return size;
        }

        public ArraySegment<byte> Serialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            BitConverter.GetBytes(timeout).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
}