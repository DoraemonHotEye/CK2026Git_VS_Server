
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

    

    

    public enum MsgID : ushort
    {
         MAINSERVER_TESTPACKET = 0,
         
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

    
    public class MAINSERVER_TestPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.MAINSERVER_TESTPACKET;

        public string Message;
        

        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort MessageLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            Message = Encoding.UTF8.GetString(buffer, start + offset, MessageLength);
            offset += (ushort)MessageLength;
            
        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(Message);
            

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

            byte[] MessageBytes = Encoding.UTF8.GetBytes(Message);
            BitConverter.GetBytes((ushort)MessageBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            MessageBytes.CopyTo(buffer, offset);
            offset += MessageBytes.Length;
            

            return new ArraySegment<byte>(buffer, 0, size);
        }
    }
    
}