using PacketDesignTool.DefaultClass;
using PacketDesignTool.Managers.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers.Supports
{
    public class PacketFormatsCPP
    {
        public static string fileformat = @"
#pragma once
#include <vector>
#include <cstdint>
#include <cstring> // memcpy
#include <tuple>
#include <string>
#include <utility>

using namespace std;

// ---------------- PacketInterface ----------------
class PacketInterface
{{
public:
    virtual ~PacketInterface() = default;
    virtual std::vector<uint8_t> Serialize() = 0;
    virtual void Deserialize(const std::vector<uint8_t>& data) = 0;
    virtual uint16_t GetSize() const = 0;
}};

class PacketObject
{{
public:
    virtual ~PacketObject() = default;

    // Serialize → (size, buffer)
    virtual std::pair<uint16_t, std::vector<uint8_t>> GetPacketSerialize() = 0;

    // Deserialize
    virtual void Deserialize(const std::vector<uint8_t>& data) = 0;

    // Size
    virtual uint16_t GetSize() const = 0;
}};

// ---------------- PacketConvertList ----------------
template<typename T>
class PacketConvertList
{{
public:
    std::vector<T> Values;

    void Enter(const T& value) {{ Values.push_back(value); }}
    uint16_t GetCount() const {{ return static_cast<uint16_t>(Values.size()); }}
    void Empty() {{ Values.clear(); }}

    // 값 타입 크기 계산 (struct)
    uint16_t GetSize_Struct() const
    {{
        return static_cast<uint16_t>(Values.size() * sizeof(T));
    }}

    // 참조 타입 크기 계산 (PacketInterface 기반)
    template<typename P>
    uint16_t GetSize_RefType() const
    {{
        uint16_t size = 0;
        for (const auto& item : Values)
        {{
            if (item)
                size += static_cast<const P*>(item)->GetSize();
        }}
        return size;
    }}

    T& operator[](size_t index) {{ return Values[index]; }}
    const T& operator[](size_t index) const {{ return Values[index]; }}
}};

{0}

{1}

// ---------------- MsgID ----------------
enum class MsgID : uint16_t
{{{2}
    DGRAM_SERVER_REQUESTACK = 25562,
    DGRAM_SERVER_ACKSEQ = 25563,
    DGRAM_SERVER_PING = 25564,
    DGRAM_SERVER_CONNECTPACKET = 25565,
}};

class DGRAM_SERVER_RequestAck : public PacketInterface
{{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_REQUESTACK;
    uint16_t SeqNum = 0;
    uint16_t ReqSeqNum = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        if (data.size() < GetSize())
            return; // 안전 체크

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&SeqNum, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&ReqSeqNum, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
    }}

    virtual uint16_t GetSize() const override
    {{
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t); // ReqSeqNum
        return size;
    }}

    virtual std::vector<uint8_t> Serialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgIdVal = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgIdVal, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&buffer[offset], &SeqNum, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&buffer[offset], &ReqSeqNum, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        return buffer;
    }}
}};

class DGRAM_SERVER_AckSeq : public PacketInterface
{{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_ACKSEQ;
    uint16_t SeqNum = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&SeqNum, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
    }}

    virtual uint16_t GetSize() const override
    {{
        return sizeof(uint16_t) * 3;
    }}

    virtual std::vector<uint8_t> Serialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&buffer[offset], &SeqNum, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        return buffer;
    }}
}};

class DGRAM_SERVER_Ping : public PacketInterface
{{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_PING;
    uint16_t SeqNum = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&SeqNum, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
    }}

    virtual uint16_t GetSize() const override
    {{
        return sizeof(uint16_t) * 3;
    }}

    virtual std::vector<uint8_t> Serialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&buffer[offset], &SeqNum, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        return buffer;
    }}
}};

class DGRAM_SERVER_ConnectPacket : public PacketInterface
{{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_CONNECTPACKET;
    uint16_t SeqNum = 0;
    uint32_t SessionID = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&SeqNum, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&SessionID, &data[offset], sizeof(uint32_t));
        offset += sizeof(uint32_t);
    }}

    virtual uint16_t GetSize() const override
    {{
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint32_t); // SessionID
        return size;
    }}

    virtual std::vector<uint8_t> Serialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&buffer[offset], &SeqNum, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        std::memcpy(&buffer[offset], &SessionID, sizeof(uint32_t));
        offset += sizeof(uint32_t);

        return buffer;
    }}
}};

{3}";

        public static string ObjectFormat = @"
{0} {1} : public PacketObject
{{
    {2}

    // Size
    virtual uint16_t GetSize() const override
    {{
        uint16_t size = 0;

        {3}

        return size;
    }}

    // Serialize
    virtual std::pair<uint16_t, std::vector<uint8_t>> GetPacketSerialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        {4}

        return {{ size, buffer }};
    }}

    // Deserialize
    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        size_t offset = 0;

        {5}
    }}
}};";

        public static string GetEventPacketClassFormat = @"
class {0} : public PacketInterface
{{
public:
    MsgID MsgIDValue = MsgID::{1};

    {2}

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        {3}
    }}

    virtual uint16_t GetSize() const override
    {{
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        {4}        
        
        return size;
    }}

    virtual std::vector<uint8_t> Serialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        {5}

        return buffer;
    }}
}};";

        public static string GetEventUPacketClassFormat = @"
class {0} : public PacketInterface
{{
public:
    MsgID MsgIDValue = MsgID::{1};
    uint16_t SeqNum = 0;

    {2}

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {{
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&SeqNum, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        {3}
    }}

    virtual uint16_t GetSize() const override
    {{
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        {4}        
        
        return size;
    }}

    virtual std::vector<uint8_t> Serialize() override
    {{
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SeqNum, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        {5}

        return buffer;
    }}
}};";

        public static string PacketManagerFormat = @"
#include ""CPacketManager.h""
#include ""../../CPPServerCore/Headers/Packets.h""
#include ""../../CPPServerCore/Session/CPacketSession.h""
#include ""../Handler/PacketHandler.h""


void PacketManager::Register()
{{
    {0}
}}

void PacketManager::OnRecvPacket(CPacketSession* session, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback)
{{
    size_t offset = 0;

    if (buffer.size() < 4) return; // 최소 크기 체크

    uint16_t size = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    uint16_t id = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    auto it = _makeFunc.find(id);
    if (it != _makeFunc.end())
    {{
        std::unique_ptr<PacketInterface> packet = it->second(session, buffer);

        if (onRecvCallback)
            onRecvCallback(session, packet.get());
        else
            HandlePacket(session, packet.get(), id);
    }}
}}

void PacketManager::RegisterHandler(uint16_t id, HandlerFunc func)
{{
    _handler[id] = func;
}}";

        public static string UPacketManagerFormat = @"
#include ""CUPacketManager.h""

#include ""../../CPPServerCore/Headers/Packets.h""
#include ""../../CPPServerCore/UDP/Session/CUPacketSession.h""

#include ""../Handler/UPacketHandler.h""

void UPacketManager::Register()
{{
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_PING] = MakePacket<DGRAM_SERVER_Ping>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_PING, UPacketHandler::DGRAM_SERVER_PingHandler);
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_ACKSEQ] = MakePacket<DGRAM_SERVER_AckSeq>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_ACKSEQ, UPacketHandler::DGRAM_SERVER_AckSeqHandler);
    _makeFunc[(uint16_t)MsgID::DGRAM_SERVER_REQUESTACK] = MakePacket<DGRAM_SERVER_RequestAck>;
    RegisterHandler((uint16_t)MsgID::DGRAM_SERVER_REQUESTACK, UPacketHandler::DGRAM_SERVER_RequestAckHandler);
    {0}
}}

void UPacketManager::OnRecvPacket(CUPacketSession* session, SOCKADDR_IN& addr, const std::vector<uint8_t>& buffer, HandlerFunc onRecvCallback)
{{
    size_t offset = 0;

    if (buffer.size() < 4) return; // 최소 크기 체크

    uint16_t size = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    uint16_t id = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    uint16_t seqNum = *reinterpret_cast<const uint16_t*>(buffer.data() + offset);
    offset += sizeof(uint16_t);

    if (seqNum != 0 && id != static_cast<uint16_t>(MsgID::DGRAM_SERVER_ACKSEQ))
    {{
        // 요청 테이블 확인
        if (session->RequestTables.find(addr) == session->RequestTables.end()) {{
            session->RequestTables[addr] = std::make_shared<RudpRequestTable>();
        }}
        bool succ = session->RequestTables[addr]->TryAdd(seqNum);
        if (succ)
        {{
            // ACK 처리
            DGRAM_SERVER_AckSeq ack = DGRAM_SERVER_AckSeq();
            ack.SeqNum = seqNum;

            session->Send(ack, addr);
        }}
        else
        {{
            return; // 이미 처리된 seqNum
        }}
    }}

    auto it = _makeFunc.find(id);
    if (it != _makeFunc.end())
    {{
        std::unique_ptr<PacketInterface> packet = it->second(session, buffer);

        if (onRecvCallback)
            onRecvCallback(session, addr, packet.get());
        else
            HandlePacket(session, addr, packet.get(), id);
    }}
}}

void UPacketManager::RegisterHandler(uint16_t id, HandlerFunc func)
{{
    _handler[id] = func;
}}";

        public static string PacketHandlerFormat = @"
#pragma once

class PacketHandler 
{{
public:
	{0}
}};";

        public static string UPacketHandlerFormat = @"
#pragma once
#include <WinSock2.h>

class UPacketHandler
{{
public:
    static void DGRAM_SERVER_PingHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    static void DGRAM_SERVER_AckSeqHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    static void DGRAM_SERVER_RequestAckHandler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    {0}

}};";

        public static string MakeHandlerMethodFormat = 
            @"static void {0}Handler(class CPacketSession* session, class PacketInterface* message);
    ";

        public static string UMakeHandlerMethodFormat =
            @"static void {0}Handler(class CUPacketSession* session, SOCKADDR_IN& addr, class PacketInterface* message);
    ";

        public static string MakeFuncManagerFormat =
            @"_makeFunc[(uint16_t)MsgID::{0}] = MakePacket<{1}>;
    RegisterHandler((uint16_t)MsgID::{0}, PacketHandler::{1}Handler);
    ";

        public static string MakeUFuncManagerFormat =
            @"_makeFunc[(uint16_t)MsgID::{0}] = MakePacket<{1}>;
    RegisterHandler((uint16_t)MsgID::{0}, UPacketHandler::{1}Handler);
    ";

        #region MemberType Offset Formats (C++)

        public static string StringOffsetFormatSerializeSingle =
        @"uint16_t {0}Len = static_cast<uint16_t>({0}.size());
        std::memcpy(&buffer[offset], &{0}Len, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], {0}.data(), {0}Len);
        offset += {0}Len;
        ";

        public static string StringOffsetFormatSerializeList =
        @"int count = {0}.GetCount();
        std::memcpy(&buffer[offset], &count, sizeof(int));
        offset += sizeof(int);
        for (auto& str : {0}.Values)
        {{
            uint16_t strLen = static_cast<uint16_t>(str.size());
            std::memcpy(&buffer[offset], &strLen, sizeof(uint16_t));
            offset += sizeof(uint16_t);
            std::memcpy(&buffer[offset], str.data(), strLen);
            offset += strLen;
        }}
        ";

        public static string GetSingleStringSizeFormat =
        @"size += sizeof(uint16_t);
        size += static_cast<uint16_t>({0}.size());";

        public static string GetListStringSizeFormat =
        @"
        size += sizeof(int);
        for(auto& item : {0}.Values)
        {{
            size += sizeof(uint16_t);
            size += static_cast<uint16_t>(item.size());
        }}
        ";

        public static string GetSingleStringDeserializeFormat =
        @"uint16_t {0}Length;
        std::memcpy(&{0}Length, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        {0}.assign(reinterpret_cast<const char*>(&data[offset]), {0}Length);
        offset += {0}Length;
        ";

        public static string GetListStringDeserializeFormat =
        @"{0}.Empty();
        int {0}Count;
        std::memcpy(&{0}Count, &data[offset], sizeof(int));
        offset += sizeof(int);
        for(int i = 0; i < {0}Count; ++i)
        {{
            uint16_t strLength;
            std::memcpy(&strLength, &data[offset], sizeof(uint16_t));
            offset += sizeof(uint16_t);
            std::string tempString(reinterpret_cast<const char*>(&data[offset]), strLength);
            offset += strLength;
            {0}.Enter(tempString);
        }}
        ";

        public static string GetSingleBasicSerializeFormat =
        @"std::memcpy(&buffer[offset], &{1}, sizeof({0}));
        offset += sizeof({0});
        ";

        public static string GetSingleBasicSizeFormat =
        @"size += sizeof({0});
        ";

        public static string GetListBasicSerializeFormat =
        @"uint32_t count = {1}.GetCount();
        std::memcpy(&buffer[offset], &count, sizeof(uint32_t));
        offset += sizeof(int);
        for(auto& item : {1}.Values)
        {{
            std::memcpy(&buffer[offset], &item, sizeof({0}));
            offset += sizeof({0});
        }}";

        public static string GetListBasicSizeFormat = @"
        size += (sizeof({0}) * {1}.GetCount()) + sizeof(uint32_t);";

        public static string GetSingleBasicDeserializeFormat =
        @"std::memcpy(&{0}, &data[offset], sizeof({1}));
        offset += sizeof({1});";

        public static string GetListObjectSizeFormat =
        @"size += sizeof(uint16_t);
        size += static_cast<uint16_t>(sizeof(uint16_t) * ({0}.GetCount() == 0 ? 1 : {0}.GetCount()));
        for(auto& obj : {0}.Values)
        {{
            size += obj.GetSize();
        }}";

        public static string GetSingleObjectSizeFormat =
        @"size += sizeof(uint16_t);
        size += {0}.GetSize();";

        public static string GetSingleObjectSerializeFormat =
        @"uint16_t {0}PacketSize = {0}.GetSize();
        std::memcpy(&buffer[offset], &{0}PacketSize, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        auto {0}Data = {0}.GetPacketSerialize();
        std::memcpy(&buffer[offset], {0}Data.second.data(), {0}PacketSize);
        offset += {0}PacketSize;";

        public static string GetSingleObjectDeserializeFormat =
        @"uint16_t {1}PacketSize;
        std::memcpy(&{1}PacketSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        {1} = {0}();
        {1}.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + {1}PacketSize));
        offset += {1}PacketSize;
        ";

        public static string GetListObjectSerializeFormat =
        @"uint16_t count = static_cast<uint16_t>({0}.GetCount());
        std::memcpy(&buffer[offset], &count, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        for (auto& packet : {0}.Values)
        {{
            uint16_t packetSize = packet.GetSize();
            std::memcpy(&buffer[offset], &packetSize, sizeof(uint16_t));
            offset += sizeof(uint16_t);
        
            auto data = packet.GetPacketSerialize();
            std::memcpy(&buffer[offset], data.second.data(), packetSize);
            offset += packetSize;
        }}
        ";

        public static string GetListObjectDeserializeFormat =
        @"uint16_t {0}Count;
        std::memcpy(&{0}Count, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        {0}.Empty();
        
        for (int i = 0; i < {0}Count; i++)
        {{
            uint16_t objectsize;
            std::memcpy(&objectsize, &data[offset], sizeof(uint16_t));
            offset += sizeof(uint16_t);
        
            {1} packet;
            packet.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + objectsize));
            {0}.Enter(packet);
        
            offset += objectsize;
        }}
        ";

        #endregion

        public static string MemberTypeSingle = @"{0} {1};";
        public static string MemberTypeList = @"PacketConvertList<{0}> {1};";

        public static string MsgIDFormat = @"{0} = {1},";

        public static string GetFormatManagerMakeFunc(Packer packer, ProjectSettingProfiler profiler, string MsgID)
        {
            return string.Format(MakeFuncManagerFormat, MsgID, ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name);
        }
        public static string GetUFormatManagerMakeFunc(Packer packer, ProjectSettingProfiler profiler, string MsgID)
        {
            return string.Format(MakeUFuncManagerFormat, MsgID, ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name);
        }
        public static string GetFormatHandlerMakeFunc(Packer packer, ProjectSettingProfiler profiler, string MsgID)
        {
            return string.Format(MakeHandlerMethodFormat, ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name);
        }
        public static string GetUFormatHandlerMakeFunc(Packer packer, ProjectSettingProfiler profiler, string MsgID)
        {
            return string.Format(UMakeHandlerMethodFormat, ToolStaticLibrary.GetHashSCCode(packer.Name, profiler) + "_" + packer.Name);
        }

        public static string GetMemberTypeStringFormat(string MemberType, string MemberName)
        {
            string[] SplitMember = MemberType.Split(':');

            if (SplitMember[0] == "single")
            {
                return string.Format(MemberTypeSingle, ToCppType(SplitMember[1]), MemberName);
            }
            else if (SplitMember[0] == "list")
            {
                return string.Format(MemberTypeList, ToCppType(SplitMember[1]), MemberName);
            }
            else
                return "Error";
        }

        public static string GetFormatPacker(string MemberType, string MemberName)
        {
            var split = MemberType.Split(':');
            string kind = split[0];
            string type = split.Length > 1 ? split[1] : "";

            if (type == "string")
            {
                if (kind == "single")
                    return string.Format(StringOffsetFormatSerializeSingle, MemberName);
                else
                    return string.Format(StringOffsetFormatSerializeList, MemberName);
            }

            if (IsBasicType(type))
            {
                string cppType = ToCppType(type);

                if (kind == "list")
                    return string.Format(GetListBasicSerializeFormat, cppType, MemberName);

                if (kind == "single")
                    return string.Format(GetSingleBasicSerializeFormat, cppType, MemberName);
            }

            //커스텀 객체
            if (kind == "single")
                return string.Format(GetSingleObjectSerializeFormat, MemberName);

            if (kind == "list")
                return string.Format(GetListObjectSerializeFormat, MemberName);

            return "";
        }

        public static string GetFormatPackerSizeble(string MemberType, string MemberName)
        {
            var split = MemberType.Split(':');
            string kind = split[0];
            string type = split.Length > 1 ? split[1] : "";

            // ⭐ string
            if (type == "string")
            {
                if (kind == "single")
                    return string.Format(GetSingleStringSizeFormat, MemberName);

                if (kind == "list")
                    return string.Format(GetListStringSizeFormat, MemberName);
            }

            // ⭐ basic 타입
            if (IsBasicType(type))
            {
                string cppType = ToCppType(type);

                if (kind == "single")
                    return string.Format(GetSingleBasicSizeFormat, cppType);

                if (kind == "list")
                    return string.Format(GetListBasicSizeFormat, cppType, MemberName);
            }

            // ⭐ 커스텀 객체
            if (kind == "single")
                return string.Format(GetSingleObjectSizeFormat, MemberName);

            if (kind == "list")
                return string.Format(GetListObjectSizeFormat, MemberName);

            return "";
        }

        public static string GetDeserializeFormat(string MemberType, string MemberName)
        {
            var split = MemberType.Split(':');
            string kind = split[0];
            string type = split.Length > 1 ? split[1] : "";

            //string 처리
            if (type == "string")
            {
                if (kind == "single")
                    return string.Format(GetSingleStringDeserializeFormat, MemberName);
                else
                    return string.Format(GetListStringDeserializeFormat, MemberName);
            }

            // ⭐ 기본 타입
            if (IsBasicType(type))
            {
                string cppType = ToCppType(type);

                return string.Format(
                    GetSingleBasicDeserializeFormat,
                    MemberName,
                    cppType
                );
            }

            //커스텀 객체
            if (kind == "single")
                return string.Format(GetSingleObjectDeserializeFormat, type, MemberName);

            if (kind == "list")
                return string.Format(GetListObjectDeserializeFormat, MemberName, type);

            return "";
        }
        static bool IsBasicType(string type)
        {
            return StaticMembers.memberList.isContains(type);
        }

        static string ToCppType(string csType)
        {
            switch (csType)
            {
                case "ushort": return "uint16_t";
                case "short": return "int16_t";
                case "int": return "int32_t";
                case "uint": return "uint32_t";
                case "long": return "int64_t";
                case "float": return "float";
                default: return csType;
            }
        }
    }
}
