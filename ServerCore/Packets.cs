
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
         MAINSERVER_SENDSESSIONID = 3,
         MAINSERVER_CREATEROOMSUCC = 4,
         MAINSERVER_OTHERPLAYERROOMJOIN = 5,
         MAINSERVER_SENDROOMLIST = 6,
         
         DGRAM_SERVER_PING = 25564,
         DGRAM_SERVER_CONNECTPACKET = 25565
    }

    public class DGRAM_SERVER_Ping : PacketInterface
    {
        public MsgID MsgID = MsgID.DGRAM_SERVER_PING;

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

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

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }


    public class DGRAM_SERVER_ConnectPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.DGRAM_SERVER_CONNECTPACKET;
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

            SessionID = BitConverter.ToUInt32(buffer, start + offset);
            offset += sizeof(uint);
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
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

            Array.Copy(BitConverter.GetBytes(SessionID), 0, buffer, offset, sizeof(uint));
            offset += sizeof(uint);

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }

    
    public class GAMECLIENT_DisconnectUDP : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_DISCONNECTUDP;

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
    
    public class GAMECLIENT_USessionIDMatch : PacketInterface
    {
        public MsgID MsgID = MsgID.GAMECLIENT_USESSIONIDMATCH;

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
    
    public class MAINSERVER_CreateRoomSucc : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_CREATEROOMSUCC;

        public ushort timeout;
        public string RoomHashData;
        public ushort MaxPlayerCount;
        

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
            ushort RoomHashDataLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            RoomHashData = Encoding.UTF8.GetString(buffer, start + offset, RoomHashDataLength);
            offset += (ushort)RoomHashDataLength;
            MaxPlayerCount = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            
            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(RoomHashData);
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
            byte[] RoomHashDataBytes = Encoding.UTF8.GetBytes(RoomHashData);
            BitConverter.GetBytes((ushort)RoomHashDataBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            RoomHashDataBytes.CopyTo(buffer, offset);
            offset += (ushort)RoomHashDataBytes.Length;
            BitConverter.GetBytes(MaxPlayerCount).CopyTo(buffer, offset);        
            offset += sizeof(ushort);
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
    public class MAINSERVER_OtherPlayerRoomJoin : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_OTHERPLAYERROOMJOIN;

        public int SessionID;
        public PacketConvertList<PlayerData> PlayerList;
        

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
            ushort PlayerListCount = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            PlayerList = new PacketConvertList<PlayerData>();
            PlayerList.Empty(); // 기존 리스트 비우기

            for (int i = 0; i < PlayerListCount; i++)
            {
                ushort objectsize = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);

                PlayerData packet = new PlayerData();
                byte[] copied = new byte[objectsize];
                Array.Copy(buffer, offset, copied, 0, objectsize);
                var slice = new ArraySegment<byte>(copied);

                packet.Deserialize(slice);
                PlayerList.Enter(packet);

                offset += objectsize;
            }
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(int);
            
            size += sizeof(ushort);
            size += (ushort)(sizeof(ushort) * (PlayerList.GetCount() == 0 ? 1 : PlayerList.GetCount()));
            size += PlayerList.GetSize_RefType<PacketObject>();
            
            

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
            Array.Copy(BitConverter.GetBytes((ushort)PlayerList.GetCount()), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            foreach (var packet in PlayerList.Values)
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
    
}