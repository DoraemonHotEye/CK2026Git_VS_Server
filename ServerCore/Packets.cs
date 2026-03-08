
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
         MAINSERVER_SENDSESSIONID = 7,
         MAINSERVER_OTHERPLAYERROOMJOIN = 8,
         MAINSERVER_SENDROOMLIST = 9,
         MAINSERVER_JOINROOMSUCC = 10,
         MAINSERVER_ROOMMASTERCHANGED = 11,
         MAINSERVER_ROOMEXITSUCC = 12,
         MAINSERVER_OTHERPLAYERROOMEXIT = 13,
         MAINSERVER_NOROOMCALLBACK = 14,
         MAINSERVER_SENDPEERUSER = 15,
         MAINSERVER_GAMESTARTSUCC = 16,
         
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
            Array.Copy(buffer, offset, OtherPlayerSliceCopied, 0, OtherPlayerPacketSize);
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
            Array.Copy(buffer, offset, MasterPlayerSliceCopied, 0, MasterPlayerPacketSize);
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
                Array.Copy(buffer, offset, copied, 0, objectsize);
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
            Array.Copy(buffer, offset, RoomMasterDataSliceCopied, 0, RoomMasterDataPacketSize);
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
            Array.Copy(buffer, offset, LeavePlayerDataSliceCopied, 0, LeavePlayerDataPacketSize);
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

        public string endPointStr;
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

            ushort endPointStrLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            endPointStr = Encoding.UTF8.GetString(buffer, start + offset, endPointStrLength);
            offset += (ushort)endPointStrLength;
            SessionID = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(endPointStr);
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

            byte[] endPointStrBytes = Encoding.UTF8.GetBytes(endPointStr);
            BitConverter.GetBytes((ushort)endPointStrBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            endPointStrBytes.CopyTo(buffer, offset);
            offset += (ushort)endPointStrBytes.Length;
            BitConverter.GetBytes(SessionID).CopyTo(buffer, offset);        
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
    
}