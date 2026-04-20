using PacketDesignTool.DefaultClass;
using PacketDesignTool.Managers.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers.Supports.CSharp
{
    public class PacketFormatsCSharp
    {
        public static string fileformat = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakePacketTemplate.Packets
{{
    public interface PacketInterface
    {{
        ArraySegment<byte> Serialize();
        void Desrialize(ArraySegment<byte> data);
        ushort GetSize();
    }}

    public interface PacketObject
    {{
        Tuple<ushort, ArraySegment<byte>> GetPacketSerialize();
        void Deserialize(ArraySegment<byte> data);
        ushort GetSize();
    }}

    public class PacketConvertList<T>
    {{
        public List<T> Values = new List<T>();

        public void Enter(T value)
        {{
            Values.Add(value);
        }}

        public ushort GetCount() => (ushort)Values.Count;

        public void Empty() {{ Values.Clear(); }}

        // 참조 타입 (PacketObject)
        public ushort GetSize_RefType<P>() where P : class, PacketObject
        {{
            ushort size = 0;
            

            foreach (var item in Values)
            {{
                if (item != null)
                    size += ((PacketObject)item).GetSize();
            }}

            return size;
        }}

        public T this[int index]
        {{
            get => Values[index];
            set => Values[index] = value;
        }}

        // 값 타입 (기본형)
        public ushort GetSize_Struct()
        {{
            ushort size = 0;

            size += (ushort)(Values.Count * System.Runtime.InteropServices.Marshal.SizeOf<T>());

            return size;
        }}
    }}

    {0}

    {1}

    public enum MsgID : ushort
    {{{2}
         DGRAM_SERVER_REQUESTACK = 25562,
         DGRAM_SERVER_ACKSEQ = 25563,
         DGRAM_SERVER_PING = 25564,
         DGRAM_SERVER_CONNECTPACKET = 25565
    }}

    public class DGRAM_SERVER_RequestAck : PacketInterface
    {{
        public MsgID MsgID = MsgID.DGRAM_SERVER_REQUESTACK;
        public ushort SeqNum;
        public ushort ReqSeqNum;

        public void Desrialize(ArraySegment<byte> data)
        {{
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
        }}

        public ushort GetSize()
        {{
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum
            size += sizeof(ushort); //ReqSeqNum

            return size;
        }}

        public ArraySegment<byte> Serialize()
        {{
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
        }}
    }}

    public class DGRAM_SERVER_AckSeq : PacketInterface
    {{
        public MsgID MsgID = MsgID.DGRAM_SERVER_ACKSEQ;
        public ushort SeqNum;

        public void Desrialize(ArraySegment<byte> data)
        {{
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
        }}

        public ushort GetSize()
        {{
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum

            return size;
        }}

        public ArraySegment<byte> Serialize()
        {{
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
        }}
    }}

    public class DGRAM_SERVER_Ping : PacketInterface
    {{
        public MsgID MsgID = MsgID.DGRAM_SERVER_PING;
        public ushort SeqNum;

        public void Desrialize(ArraySegment<byte> data)
        {{
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;

            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
        }}

        public ushort GetSize()
        {{
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum

            return size;
        }}

        public ArraySegment<byte> Serialize()
        {{
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
        }}
    }}


    public class DGRAM_SERVER_ConnectPacket : PacketInterface
    {{
        public MsgID MsgID = MsgID.DGRAM_SERVER_CONNECTPACKET;
        public ushort SeqNum;
        public uint SessionID;

        public void Desrialize(ArraySegment<byte> data)
        {{
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
        }}

        public ushort GetSize()
        {{
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); //SeqNum
            size += sizeof(uint);

            return size;
        }}

        public ArraySegment<byte> Serialize()
        {{
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
        }}
    }}

    {3}
}}";

        public static string MsgIDFormat = @"{0} = {1},";

        public static string ObjectFormat = @"
    public {0} {1} : PacketObject
    {{
        {2}
        
        public ushort GetSize()
        {{
            ushort size = 0;
               
            {3}
    
            return size;
        }}
    
        public Tuple<ushort, ArraySegment<byte>> GetPacketSerialize()
        {{
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            int offset = 0;
               
            {4}
    
            return new Tuple<ushort, ArraySegment<byte>>(size, new ArraySegment<byte>(buffer, 0, size));
        }}
        
        public void Deserialize(ArraySegment<byte> data)
        {{
            int offset = 0;
            byte[] buffer = data.Array;
            int start = data.Offset;

            {5}
        }}
    }}";

        public static string MemberTypeSingle = @"public {0} {1};";
        public static string MemberTypeList = @"public PacketConvertList<{0}> {1};";

        #region MemberType Offset Formats
        public static string StringOffsetFormatSerializeSingle =
            @"byte[] {0}Bytes = Encoding.UTF8.GetBytes({0});
            BitConverter.GetBytes((ushort){0}Bytes.Length).CopyTo(buffer, offset);
            offset += sizeof(ushort);
            {0}Bytes.CopyTo(buffer, offset);
            offset += (ushort){0}Bytes.Length;
            ";
        public static string StringOffsetFormatSerializeList =
            @"BitConverter.GetBytes({0}.GetCount()).CopyTo(buffer, offset);
            offset += sizeof(int);
            foreach (var str in {0}.Values)
            {{
                byte[] strBytes = Encoding.UTF8.GetBytes(str);
                BitConverter.GetBytes((ushort)strBytes.Length).CopyTo(buffer, offset);
                offset += sizeof(ushort);
                strBytes.CopyTo(buffer, offset);
                offset += (ushort)strBytes.Length;
            }}
            ";
        public static string GetSingleStringSizeFormat =
            @"size += sizeof(ushort);
            size += (ushort)Encoding.UTF8.GetByteCount({0});";
        public static string GetListStringSizeFormat =
            @"
            size += sizeof(int);
            foreach(var item in {0}.Values)
            {{
                size += sizeof(ushort);
                size += (ushort)Encoding.UTF8.GetByteCount(item);
            }}
            ";

        public static string GetSingleStringDeserializeFormat =
            @"ushort {0}Length = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);
            {0} = Encoding.UTF8.GetString(buffer, start + offset, {0}Length);
            offset += (ushort){0}Length;
            ";
        public static string GetListStringDeserializeFormat =
            @"{0} = new PacketConvertList<string>();
            int {0}Count = BitConverter.ToInt32(buffer, start + offset);
            offset += sizeof(int);
            for(int i = 0; i < {0}Count; ++i)
            {{
                ushort strLength = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);
                string tempString = Encoding.UTF8.GetString(buffer, start + offset, strLength);
                offset += (ushort)strLength;
                {0}.Enter(tempString);
            }}
            ";

        public static string GetSingleBasicSerializeFormat = 
            @"BitConverter.GetBytes({1}).CopyTo(buffer, offset);        
            offset += sizeof({0});
            ";
        public static string GetSingleBasicSizeFormat = 
            @"size += sizeof({0});
            ";

        public static string GetListBasicSerializeFormat =
            @"BitConverter.GetBytes({1}.GetCount()).CopyTo(buffer, offset);
            offset += sizeof(int);
            foreach(var item in {1}.Values)
            {{
                BitConverter.GetBytes(item).CopyTo(buffer, offset);
                offset += sizeof({0});
            }}
            ";

        public static string GetListBasicSizeFormat = @"
            size += (ushort)(sizeof({0}) * {1}.Values.Count) + sizeof(int);";

        public static string GetSingleBasicDeserializeFormat =
            @"{0} = BitConverter.{2}(buffer, start + offset);
            offset += sizeof({1});
            ";

        public static string GetListObjectSizeFormat =
            @"size += sizeof(ushort);
            size += (ushort)(sizeof(ushort) * ({0}.GetCount() == 0 ? 1 : {0}.GetCount()));
            size += {0}.GetSize_RefType<PacketObject>();
            ";

        public static string GetSingleObjectSizeFormat =
            @"size += sizeof(ushort);
            size += {0}.GetSize();
            ";

        public static string GetSingleObjectSerializeFormat =
            @"var ({0}PacketSize, {0}Serialize) = {0}.GetPacketSerialize();
            Array.Copy(BitConverter.GetBytes((ushort){0}PacketSize), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy({0}Serialize.Array, {0}Serialize.Offset, buffer, offset, {0}PacketSize);
            offset += {0}PacketSize;
            ";

        public static string GetSingleObjectDeserializeFormat =
            @"ushort {1}PacketSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            {1} = new {0}();
            byte[] {1}SliceCopied = new byte[{1}PacketSize];
            Array.Copy(buffer, start + offset, {1}SliceCopied, 0, {1}PacketSize);
            var {1}Array = new ArraySegment<byte>({1}SliceCopied);
            {1}.Deserialize({1}Array);
            offset += {1}PacketSize;
            ";

        public static string GetListObjectSerializeFormat =
            @"Array.Copy(BitConverter.GetBytes((ushort){0}.GetCount()), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            foreach (var packet in {0}.Values)
            {{
                var (packetSize, segment) = packet.GetPacketSerialize();
                Array.Copy(BitConverter.GetBytes((ushort)packetSize), 0, buffer, offset, sizeof(ushort));
                offset += sizeof(ushort);
                Array.Copy(segment.Array, segment.Offset, buffer, offset, packetSize);
                offset += packetSize;
            }}
            ";

        public static string GetListObjectDeserializeFormat =
            @"ushort {0}Count = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            {0} = new PacketConvertList<{1}>();
            {0}.Empty(); // 기존 리스트 비우기

            for (int i = 0; i < {0}Count; i++)
            {{
                ushort objectsize = BitConverter.ToUInt16(buffer, start + offset);
                offset += sizeof(ushort);

                {1} packet = new {1}();
                byte[] copied = new byte[objectsize];
                Array.Copy(buffer, start + offset, copied, 0, objectsize);
                var slice = new ArraySegment<byte>(copied);

                packet.Deserialize(slice);
                {0}.Enter(packet);

                offset += objectsize;
            }}
            ";

        #endregion

        public static string GetEventPacketClassFormat = @"
    public class {0} : PacketInterface
    {{
        public MsgID MsgID = MsgID.{1};

        {2}

        public void Desrialize(ArraySegment<byte> data)
        {{
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            {3}
        }}

        public ushort GetSize()
        {{
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID

            {4}

            return size;
        }}

        public ArraySegment<byte> Serialize()
        {{
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            {5}

            return new ArraySegment<byte>(buffer, 0, size);
        }}
    }}
    ";

        public static string GetEventUDPPacketClassFormat = @"
    public class {0} : PacketInterface
    {{
        public MsgID MsgID = MsgID.{1};
        public ushort SeqNum;

        {2}

        public void Desrialize(ArraySegment<byte> data)
        {{
            byte[] buffer = data.Array;
            int start = data.Offset;
            int offset = 0;
            
            ushort packetSize = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            ushort packetId = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            SeqNum = BitConverter.ToUInt16(buffer, start + offset);
            offset += sizeof(ushort);

            {3}
        }}

        public ushort GetSize()
        {{
            ushort size = 0;

            size += sizeof(ushort); // PacketSize
            size += sizeof(ushort); // PacketID
            size += sizeof(ushort); // SeqNum

            {4}

            return size;
        }}

        public ArraySegment<byte> Serialize()
        {{
            ushort size = GetSize();
            byte[] buffer = new byte[size];
            ushort offset = 0;

            Array.Copy(BitConverter.GetBytes(size), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)MsgID), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)SeqNum), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);

            {5}

            return new ArraySegment<byte>(buffer, 0, size);
        }}
    }}
    ";

        public static string UPacketManagerFormat =
            @"
using MakePacketTemplate.Packets;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using UDgramCore;

public class UPacketManager
{{
    #region Singleton
    static UPacketManager _instance = new UPacketManager();
    public static UPacketManager Instance {{ get {{ return _instance; }} }}
    #endregion

    UPacketManager()
    {{
        Register();
    }}

    Dictionary<ushort, Func<IPEndPoint, USession, ArraySegment<byte>, PacketInterface>> _makeFunc = new Dictionary<ushort, Func<IPEndPoint, USession, ArraySegment<byte>, PacketInterface>>();
    Dictionary<ushort, Action<IPEndPoint, USession, PacketInterface>> _handler = new Dictionary<ushort, Action<IPEndPoint, USession, PacketInterface>>();

    public void Register()
    {{
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_PING, MakePacket<DGRAM_SERVER_Ping>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_PING, UPacketHandler.DGRAM_SERVER_PingHandler);
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_ACKSEQ, MakePacket<DGRAM_SERVER_AckSeq>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_ACKSEQ, UPacketHandler.DGRAM_SERVER_AckSeqHandler);
	    _makeFunc.Add((ushort)MsgID.DGRAM_SERVER_REQUESTACK, MakePacket<DGRAM_SERVER_RequestAck>);
	    _handler.Add((ushort)MsgID.DGRAM_SERVER_REQUESTACK, UPacketHandler.DGRAM_SERVER_RequestAckHandler);
	    {0}
    }}

    public void OnRecvPacket(IPEndPoint ipendPoint, USession usession, ArraySegment<byte> buffer, Action<IPEndPoint, USession, PacketInterface> onRecvCallback = null)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;
        ushort SeqNum = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);

        if (SeqNum != 0)
        {{
            if (!usession.PeerRequestTable.ContainsKey(ipendPoint.ToString()))
                usession.PeerRequestTable.Add(ipendPoint.ToString(), new RudpRequestTable());

            bool succ = usession.PeerRequestTable[ipendPoint.ToString()].TryAdd(SeqNum);

            if (succ == true)
            {{
                //TODO ACK 처리
                DGRAM_SERVER_AckSeq ackSeq = new DGRAM_SERVER_AckSeq();
                ackSeq.SeqNum = SeqNum;
                usession._server.Send(ackSeq.Serialize().Array, ackSeq.GetSize(), ipendPoint);
            }}
            else 
            {{
                return;
            }}
        }}

        Func<IPEndPoint, USession, ArraySegment<byte>, PacketInterface> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {{
            PacketInterface packet = func.Invoke(ipendPoint, usession, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(ipendPoint, usession, packet);
            else
                HandlePacket(ipendPoint, usession, packet, id);
        }}
    }}

    T MakePacket<T>(IPEndPoint ipendPoint, USession usession, ArraySegment<byte> buffer) where T : PacketInterface, new()
    {{
        T pkt = new T();
        pkt.Desrialize(buffer);
        return pkt;
    }}

    public void HandlePacket(IPEndPoint session, USession usession, PacketInterface packet, ushort ID)
    {{
        Action<IPEndPoint, USession, PacketInterface> action = null;
        if (_handler.TryGetValue(ID, out action))
            action.Invoke(session, usession, packet);
    }}
}}";

        public static string PacketManagerFormat =
            @"
using MakePacketTemplate.Packets;
using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance {{ get {{ return _instance; }} }}
    #endregion

    PacketManager()
    {{
        Register();
    }}

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, PacketInterface>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, PacketInterface>>();
    Dictionary<ushort, Action<PacketSession, PacketInterface>> _handler = new Dictionary<ushort, Action<PacketSession, PacketInterface>>();

    public void Register()
    {{
        {0}
    }}

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, PacketInterface> onRecvCallback = null)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func<PacketSession, ArraySegment<byte>, PacketInterface> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {{
            PacketInterface packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet, id);
        }}
    }}

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : PacketInterface, new()
    {{
        T pkt = new T();
        pkt.Desrialize(buffer);
        return pkt;
    }}

    public void HandlePacket(PacketSession session, PacketInterface packet, ushort ID)
    {{
        Action<PacketSession, PacketInterface> action = null;
        if (_handler.TryGetValue(ID, out action))
            action.Invoke(session, packet);
    }}
}}";

        public static string MakeFuncManagerFormat =
            @"_makeFunc.Add((ushort)MsgID.{0}, MakePacket<{1}>);
        _handler.Add((ushort)MsgID.{0}, PacketHandler.{1}Handler);
        ";

        public static string UMakeFuncManagerFormat =
            @"_makeFunc.Add((ushort)MsgID.{0}, MakePacket<{1}>);
        _handler.Add((ushort)MsgID.{0}, UPacketHandler.{1}Handler);
        ";

        public static string GetFormatManagerMakeFunc(Packer packer, ProjectSettingProfiler profiler, string MsgID)
        {
            return string.Format(MakeFuncManagerFormat, MsgID, ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name);
        }
        public static string GetFormatUManagerMakeFunc(Packer packer, ProjectSettingProfiler profiler, string MsgID)
        {
            return string.Format(UMakeFuncManagerFormat, MsgID, ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name);
        }


        public static string GetFormatPacker(string MemberType, string MemberName)
        {
            switch (MemberType)
            {
                case "list:string":
                    return string.Format(StringOffsetFormatSerializeList, MemberName);
                case "single:string":
                    return string.Format(StringOffsetFormatSerializeSingle, MemberName);

                case "list:int":
                case "list:uint":
                case "list:float":
                case "list:ushort":
                case "list:short":
                case "list:long":
                    return string.Format(GetListBasicSerializeFormat, MemberType.Split(':')[1], MemberName);

                case "single:int":
                case "single:uint":
                case "single:float":
                case "single:ushort":
                case "single:short":
                case "single:long":
                    return string.Format(GetSingleBasicSerializeFormat, MemberType.Split(':')[1], MemberName);

                default: //class
                    return GetFormatObjectSerialize(MemberType, MemberName);
            }
        }

        public static string GetFormatDeserialize(string MemberType, string MemberName)
        {
            switch (MemberType)
            {
                case "list:string":
                    return string.Format(GetListStringDeserializeFormat, MemberName);
                case "single:string":
                    return string.Format(GetSingleStringDeserializeFormat, MemberName);

                case "list:int":
                case "list:uint":
                case "list:float":
                case "list:ushort":
                case "list:short":
                case "list:long":
                    return "";

                case "single:int":
                case "single:uint":
                case "single:float":
                case "single:ushort":
                case "single:short":
                case "single:long":
                    return string.Format(GetSingleBasicDeserializeFormat, MemberName, MemberType.Split(':')[1], GetDeserializeFunctionName(MemberType.Split(':')[1]));

                default: //class
                    return GetFormatObjectDeSerialize(MemberType, MemberName);
            }
        }

        public static string GetFormatObjectSerialize(string MemberType, string MemberName)
        {
            if (MemberType.Split(':')[0] == "list")
            {
                return string.Format(GetListObjectSerializeFormat, MemberName, MemberType.Split(':')[1]);
            }
            else if (MemberType.Split(':')[0] == "single")
            {
                return string.Format(GetSingleObjectSerializeFormat, MemberName);
            }

            return "Error";
        }

        public static string GetFormatObjectDeSerialize(string MemberType, string MemberName)
        {
            if (MemberType.Split(':')[0] == "list")
            {
                return string.Format(GetListObjectDeserializeFormat, MemberName, MemberType.Split(':')[1]);
            }
            else if (MemberType.Split(':')[0] == "single")
            {
                return string.Format(GetSingleObjectDeserializeFormat, MemberType.Split(':')[1], MemberName);
            }

            return "Error";
        }

        public static string GetFormatObjectSizeable(string MemberType, string MemberName)
        {
            if (MemberType.Split(':')[0] == "list")
            {
                return string.Format(GetListObjectSizeFormat, MemberName);
            }
            else if (MemberType.Split(':')[0] == "single")
            {
                return string.Format(GetSingleObjectSizeFormat, MemberName);
            }

            return "Error";
        }

        public static string GetDeserializeFunctionName(string MemberType)
        {
            switch (MemberType)
            {
                case "int":
                    return "ToInt32";
                case "uint":
                    return "ToUInt32";
                case "long":
                    return "ToInt64";
                case "float":
                    return "ToSingle";
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "char":
                    return "";

                default:
                    return "";
            }
        }

        public static string GetFormatPackerSizeble(string MemberType, string MemberName)
        {
            switch (MemberType)
            {
                case "list:string":
                    return string.Format(GetListStringSizeFormat, MemberName);
                case "single:string":
                    return string.Format(GetSingleStringSizeFormat, MemberName);

                case "list:int":
                case "list:uint":
                case "list:float":
                case "list:ushort":
                case "list:short":
                case "list:long":
                    return string.Format(GetListBasicSizeFormat, MemberType.Split(':')[1], MemberName);

                case "single:int":
                case "single:uint":
                case "single:float":
                case "single:ushort":
                case "single:short":
                case "single:long":
                    return string.Format(GetSingleBasicSizeFormat, MemberType.Split(':')[1]);

                default:
                    return GetFormatObjectSizeable(MemberType, MemberName);
            }
        }

        public static string GetMemberTypeStringFormat(string MemberType, string MemberName)
        {
            string[] SplitMember = MemberType.Split(':');

            if (SplitMember[0] == "single")
            {
                return string.Format(MemberTypeSingle, SplitMember[1], MemberName);
            }
            else if (SplitMember[0] == "list")
            {
                return string.Format(MemberTypeList, SplitMember[1], MemberName);
            }
            else
                return "Error";
        }

    }
}
