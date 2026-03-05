
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


    public class LoginClass : PacketObject
    {
        public string ID;
        public string Password;


        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(ID);
            size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount(Password);


            return size;
        }

        public Tuple<ushort, ArraySegment<byte>> GetPacketSerialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            int offset = 0;

            byte[] IDBytes = Encoding.UTF8.GetBytes(ID);
            BitConverter.GetBytes((ushort)IDBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            IDBytes.CopyTo(buffer, offset);
            offset += IDBytes.Length;
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(Password);
            BitConverter.GetBytes((ushort)PasswordBytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            PasswordBytes.CopyTo(buffer, offset);
            offset += PasswordBytes.Length;


            return new Tuple<ushort, ArraySegment<byte>>(size, new ArraySegment<byte>(buffer, 0, size));
        }

        public void Deserialize(ArraySegment<byte> data)
        {
            int offset = 0;
            byte[] buffer = data.Array;
            int start = data.Offset;

            ushort IDLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            ID = Encoding.UTF8.GetString(buffer, start + offset, IDLength);
            offset += IDLength;
            ushort PasswordLength = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            Password = Encoding.UTF8.GetString(buffer, start + offset, PasswordLength);
            offset += PasswordLength;

        }
    }



    public struct testpacket : PacketObject
    {
        public long test;
        public PacketConvertList<string> test2;


        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(long);


            size += sizeof(int);
            foreach (var item in test2.Values)
            {
                size += sizeof(ushort);
                size += (ushort)Encoding.UTF8.GetByteCount(item);
            }



            return size;
        }

        public Tuple<ushort, ArraySegment<byte>> GetPacketSerialize()
        {
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            int offset = 0;

            BitConverter.GetBytes(test).CopyTo(buffer, offset);
            offset += sizeof(long);
            BitConverter.GetBytes(test2.GetCount()).CopyTo(buffer, offset);
            offset += sizeof(int);
            foreach (var str in test2.Values)
            {
                byte[] strBytes = Encoding.UTF8.GetBytes(str);
                BitConverter.GetBytes((ushort)strBytes.Length).CopyTo(buffer, offset);
                offset += sizeof(ushort);
                strBytes.CopyTo(buffer, offset);
                offset += strBytes.Length;
            }


            return new Tuple<ushort, ArraySegment<byte>>(size, new ArraySegment<byte>(buffer, 0, size));
        }

        public void Deserialize(ArraySegment<byte> data)
        {
            int offset = 0;
            byte[] buffer = data.Array;
            int start = data.Offset;

            test = BitConverter.ToInt64(data.ToArray(), start + offset);
            offset += sizeof(long);
            test2 = new PacketConvertList<string>();
            int test2Count = BitConverter.ToInt32(data.ToArray(), start + offset);
            offset += sizeof(int);
            for (int i = 0; i < test2Count; ++i)
            {
                ushort strLength = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);
                string tempString = Encoding.UTF8.GetString(buffer, start + offset, strLength);
                offset += strLength;
                test2.Enter(tempString);
            }

        }
    }


    public enum MsgID : ushort
    {
        DEFAULT_SERVER_LOGINPACKET = 0,
        DEFAULT_SERVER_HELLOPACKET = 1,

    }


    public class DEFAULT_SERVER_LoginPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.DEFAULT_SERVER_LOGINPACKET;

        public short helloworld;


        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            helloworld = BitConverter.ToInt16(data.ToArray(), start + offset);
            offset += sizeof(short);

        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

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

            BitConverter.GetBytes(helloworld).CopyTo(buffer, offset);
            offset += sizeof(short);


            return new ArraySegment<byte>(buffer, 0, size);
        }
    }

    public class DEFAULT_SERVER_HelloPacket : PacketInterface
    {
        public MsgID MsgID = MsgID.DEFAULT_SERVER_HELLOPACKET;

        public LoginClass Login1;
        public PacketConvertList<LoginClass> Login2;


        public void Desrialize(ArraySegment<byte> data)
        {
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort Login1PacketSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            Login1 = new LoginClass();
            byte[] Login1SliceCopied = new byte[Login1PacketSize];
            Array.Copy(buffer, offset, Login1SliceCopied, 0, Login1PacketSize);
            var Login1Array = new ArraySegment<byte>(Login1SliceCopied);
            Login1.Deserialize(Login1Array);
            offset += Login1PacketSize;
            ushort Login2Count = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            Login2 = new PacketConvertList<LoginClass>();
            Login2.Empty(); // 기존 리스트 비우기

            for (int i = 0; i < Login2Count; i++)
            {
                ushort objectsize = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);

                LoginClass packet = new LoginClass();
                byte[] copied = new byte[objectsize];
                Array.Copy(buffer, offset, copied, 0, objectsize);
                var slice = new ArraySegment<byte>(copied);

                packet.Deserialize(slice);
                Login2.Enter(packet);

                offset += objectsize;
            }

        }

        public ushort GetSize()
        {
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            size += sizeof(ushort);
            size += Login1.GetSize();

            size += sizeof(ushort);
            size += (ushort)(sizeof(ushort) * (Login2.GetCount() == 0 ? 1 : Login2.GetCount()));
            size += Login2.GetSize_RefType<PacketObject>();



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

            var (Login1PacketSize, Login1Serialize) = Login1.GetPacketSerialize();
            Array.Copy(BitConverter.GetBytes((ushort)Login1PacketSize), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(Login1Serialize.Array, Login1Serialize.Offset, buffer, offset, Login1PacketSize);
            offset += Login1PacketSize;
            Array.Copy(BitConverter.GetBytes((ushort)Login2.GetCount()), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            foreach (var packet in Login2.Values)
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

}