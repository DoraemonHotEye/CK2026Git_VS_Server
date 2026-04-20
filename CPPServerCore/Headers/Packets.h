
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
{
public:
    virtual ~PacketInterface() = default;
    virtual std::vector<uint8_t> Serialize() = 0;
    virtual void Deserialize(const std::vector<uint8_t>& data) = 0;
    virtual uint16_t GetSize() const = 0;
};

class PacketObject
{
public:
    virtual ~PacketObject() = default;

    // Serialize → (size, buffer)
    virtual std::pair<uint16_t, std::vector<uint8_t>> GetPacketSerialize() = 0;

    // Deserialize
    virtual void Deserialize(const std::vector<uint8_t>& data) = 0;

    // Size
    virtual uint16_t GetSize() const = 0;
};

// ---------------- PacketConvertList ----------------
template<typename T>
class PacketConvertList
{
public:
    std::vector<T> Values;

    void Enter(const T& value) { Values.push_back(value); }
    uint16_t GetCount() const { return static_cast<uint16_t>(Values.size()); }
    void Empty() { Values.clear(); }

    // 값 타입 크기 계산 (struct)
    uint16_t GetSize_Struct() const
    {
        return static_cast<uint16_t>(Values.size() * sizeof(T));
    }

    // 참조 타입 크기 계산 (PacketInterface 기반)
    template<typename P>
    uint16_t GetSize_RefType() const
    {
        uint16_t size = 0;
        for (const auto& item : Values)
        {
            if (item)
                size += static_cast<const P*>(item)->GetSize();
        }
        return size;
    }

    T& operator[](size_t index) { return Values[index]; }
    const T& operator[](size_t index) const { return Values[index]; }
};




struct PlayerData : public PacketObject
{
    int32_t SessionID;
    

public:
    // Size
    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;

        size += sizeof(int32_t);
        
        

        return size;
    }

    // Serialize
    virtual std::pair<uint16_t, std::vector<uint8_t>> GetPacketSerialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &SessionID, sizeof(int32_t));
        offset += sizeof(int32_t);
        
        

        return { size, buffer };
    }

    // Deserialize
    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        size_t offset = 0;

        std::memcpy(&SessionID, &data[offset], sizeof(int32_t));
        offset += sizeof(int32_t);
        
    }
};


// ---------------- MsgID ----------------
enum class MsgID : uint16_t
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
    DGRAM_SERVER_CONNECTPACKET = 25565,
};

class DGRAM_SERVER_RequestAck : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_REQUESTACK;
    uint16_t SeqNum = 0;
    uint16_t ReqSeqNum = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t); // ReqSeqNum
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
    }
};

class DGRAM_SERVER_AckSeq : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_ACKSEQ;
    uint16_t SeqNum = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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
    }

    virtual uint16_t GetSize() const override
    {
        return sizeof(uint16_t) * 3;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
    }
};

class DGRAM_SERVER_Ping : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_PING;
    uint16_t SeqNum = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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
    }

    virtual uint16_t GetSize() const override
    {
        return sizeof(uint16_t) * 3;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
    }
};

class DGRAM_SERVER_ConnectPacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::DGRAM_SERVER_CONNECTPACKET;
    uint16_t SeqNum = 0;
    uint32_t SessionID = 0;

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint32_t); // SessionID
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
    }
};


class GAMECLIENT_DisconnectUDP : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_DISCONNECTUDP;
    uint16_t SeqNum = 0;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_USessionIDMatch : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_USESSIONIDMATCH;
    uint16_t SeqNum = 0;

    int32_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&SessionID, &data[offset], sizeof(int32_t));
        offset += sizeof(int32_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(int32_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(int32_t));
        offset += sizeof(int32_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_CreateRoomCall : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_CREATEROOMCALL;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_RoomSendCall : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_ROOMSENDCALL;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_JoinRoomCall : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_JOINROOMCALL;

    string RoomHashID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t RoomHashIDLength;
        std::memcpy(&RoomHashIDLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        RoomHashID.assign(reinterpret_cast<const char*>(&data[offset]), RoomHashIDLength);
        offset += RoomHashIDLength;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(RoomHashID.size());
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t RoomHashIDLen = static_cast<uint16_t>(RoomHashID.size());
        std::memcpy(&buffer[offset], &RoomHashIDLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], RoomHashID.data(), RoomHashIDLen);
        offset += RoomHashIDLen;
        
        

        return buffer;
    }
};
class GAMECLIENT_RoomExitCall : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_ROOMEXITCALL;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_GameStart : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_GAMESTART;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_UMovepacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_UMOVEPACKET;
    uint16_t SeqNum = 0;

    int16_t XDir;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&XDir, &data[offset], sizeof(int16_t));
        offset += sizeof(int16_t);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(int16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &XDir, sizeof(int16_t));
        offset += sizeof(int16_t);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_UPosSyncPacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_UPOSSYNCPACKET;
    uint16_t SeqNum = 0;

    float x;
    float y;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&x, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&y, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &x, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &y, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_PeerPingPacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PEERPINGPACKET;
    uint16_t SeqNum = 0;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_PeerPingACK : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PEERPINGACK;
    uint16_t SeqNum = 0;

    uint16_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&SessionID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_UJumpPacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_UJUMPPACKET;
    uint16_t SeqNum = 0;

    float JumpPos;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&JumpPos, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &JumpPos, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_UJumpEndPacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_UJUMPENDPACKET;
    uint16_t SeqNum = 0;

    float currY;
    float currX;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&currY, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&currX, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &currY, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &currX, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_GameStartReady : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_GAMESTARTREADY;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_SendLocalEndPoint : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_SENDLOCALENDPOINT;

    string LocalAddress;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t LocalAddressLength;
        std::memcpy(&LocalAddressLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        LocalAddress.assign(reinterpret_cast<const char*>(&data[offset]), LocalAddressLength);
        offset += LocalAddressLength;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(LocalAddress.size());
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t LocalAddressLen = static_cast<uint16_t>(LocalAddress.size());
        std::memcpy(&buffer[offset], &LocalAddressLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], LocalAddress.data(), LocalAddressLen);
        offset += LocalAddressLen;
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterMoveSync : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERMOVESYNC;
    uint16_t SeqNum = 0;

    int16_t XDir;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&XDir, &data[offset], sizeof(int16_t));
        offset += sizeof(int16_t);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(int16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &XDir, sizeof(int16_t));
        offset += sizeof(int16_t);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterSyncPos : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERSYNCPOS;
    uint16_t SeqNum = 0;

    float x;
    float y;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&x, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&y, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &x, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &y, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_UDownJumpPacket : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_UDOWNJUMPPACKET;
    uint16_t SeqNum = 0;

    float duration;
    float downSpeed;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&duration, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&downSpeed, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &duration, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &downSpeed, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_EnemyProjectileFire : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_ENEMYPROJECTILEFIRE;
    uint16_t SeqNum = 0;

    uint16_t OwnerID;
    uint16_t NetID;
    float dirX;
    float dirY;
    float posX;
    float posY;
    uint16_t CarrierIdx;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&OwnerID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&dirX, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&dirY, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&posX, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&posY, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&CarrierIdx, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &OwnerID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &dirX, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &dirY, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &posX, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &posY, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &CarrierIdx, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_PlayerAttack : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PLAYERATTACK;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t Combo;
    int16_t FlipX;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&Combo, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&FlipX, &data[offset], sizeof(int16_t));
        offset += sizeof(int16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
        size += sizeof(int16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &Combo, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &FlipX, sizeof(int16_t));
        offset += sizeof(int16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterLockFlip : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERLOCKFLIP;
    uint16_t SeqNum = 0;

    int16_t LockType;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&LockType, &data[offset], sizeof(int16_t));
        offset += sizeof(int16_t);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(int16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &LockType, sizeof(int16_t));
        offset += sizeof(int16_t);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterAttack : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERATTACK;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterAttackStop : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERATTACKSTOP;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_TakeDamageMonster : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_TAKEDAMAGEMONSTER;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&SessionID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterDie : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERDIE;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&SessionID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_PlayerHit : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PLAYERHIT;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&SessionID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_PlayerDie : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PLAYERDIE;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&SessionID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterJump : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERJUMP;
    uint16_t SeqNum = 0;

    float JumpPower;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&JumpPower, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &JumpPower, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterUseGravity : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERUSEGRAVITY;
    uint16_t SeqNum = 0;

    uint16_t switchtype;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&switchtype, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &switchtype, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterSwitchAnim : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERSWITCHANIM;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t AnimCode;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&AnimCode, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &AnimCode, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_ProjectileStop : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PROJECTILESTOP;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterVelocitySet : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERVELOCITYSET;
    uint16_t SeqNum = 0;

    float x;
    float y;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&x, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&y, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &x, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &y, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_ProjectileDisableCall : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_PROJECTILEDISABLECALL;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_GroundCheck : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_GROUNDCHECK;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterGrabStart : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERGRABSTART;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t OwnerID;
    float posX;
    float posY;
    uint16_t SessionID;
    uint16_t CarrierIdx;
    uint16_t TargetIdx;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&OwnerID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&posX, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&posY, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&SessionID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&CarrierIdx, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&TargetIdx, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &OwnerID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &posX, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &posY, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &CarrierIdx, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &TargetIdx, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterTeleport : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERTELEPORT;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    float PosX;
    float PosY;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&PosX, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&PosY, &data[offset], sizeof(float));
        offset += sizeof(float);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(float);
        
        size += sizeof(float);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &PosX, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &PosY, sizeof(float));
        offset += sizeof(float);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterGrabIng : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERGRABING;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    uint16_t TargetID;
    float posX;
    float posY;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&TargetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&posX, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&posY, &data[offset], sizeof(float));
        offset += sizeof(float);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        
        size += sizeof(float);
        
        size += sizeof(float);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &TargetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &posX, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &posY, sizeof(float));
        offset += sizeof(float);
        
        

        return buffer;
    }
};
class GAMECLIENT_MonsterGrabEnd : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_MONSTERGRABEND;
    uint16_t SeqNum = 0;

    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_RpcCall : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_RPCCALL;
    uint16_t SeqNum = 0;

    string JsonHash;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        uint16_t JsonHashLength;
        std::memcpy(&JsonHashLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        JsonHash.assign(reinterpret_cast<const char*>(&data[offset]), JsonHashLength);
        offset += JsonHashLength;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(JsonHash.size());
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        uint16_t JsonHashLen = static_cast<uint16_t>(JsonHash.size());
        std::memcpy(&buffer[offset], &JsonHashLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], JsonHash.data(), JsonHashLen);
        offset += JsonHashLen;
        
        

        return buffer;
    }
};
class GAMECLIENT_SwitchAnimInt : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_SWITCHANIMINT;
    uint16_t SeqNum = 0;

    uint16_t AnimCode;
    string ParamName;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&AnimCode, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        uint16_t ParamNameLength;
        std::memcpy(&ParamNameLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        ParamName.assign(reinterpret_cast<const char*>(&data[offset]), ParamNameLength);
        offset += ParamNameLength;
        
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(ParamName.size());
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &AnimCode, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t ParamNameLen = static_cast<uint16_t>(ParamName.size());
        std::memcpy(&buffer[offset], &ParamNameLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], ParamName.data(), ParamNameLen);
        offset += ParamNameLen;
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class GAMECLIENT_LandingSyncPos : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::GAMECLIENT_LANDINGSYNCPOS;
    uint16_t SeqNum = 0;

    float x;
    float y;
    uint16_t NetID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
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

        std::memcpy(&x, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&y, &data[offset], sizeof(float));
        offset += sizeof(float);
        std::memcpy(&NetID, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t); // SeqNum
        size += sizeof(float);
        
        size += sizeof(float);
        
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
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
        
        std::memcpy(&buffer[offset], &x, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &y, sizeof(float));
        offset += sizeof(float);
        
        std::memcpy(&buffer[offset], &NetID, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class MAINSERVER_SendSessionID : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_SENDSESSIONID;

    int32_t SessionID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&SessionID, &data[offset], sizeof(int32_t));
        offset += sizeof(int32_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(int32_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(int32_t));
        offset += sizeof(int32_t);
        
        

        return buffer;
    }
};
class MAINSERVER_OtherPlayerRoomJoin : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_OTHERPLAYERROOMJOIN;

    PlayerData OtherPlayer;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t OtherPlayerPacketSize;
        std::memcpy(&OtherPlayerPacketSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        OtherPlayer = PlayerData();
        OtherPlayer.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + OtherPlayerPacketSize));
        offset += OtherPlayerPacketSize;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += OtherPlayer.GetSize();
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t OtherPlayerPacketSize = OtherPlayer.GetSize();
        std::memcpy(&buffer[offset], &OtherPlayerPacketSize, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        auto OtherPlayerData = OtherPlayer.GetPacketSerialize();
        std::memcpy(&buffer[offset], OtherPlayerData.second.data(), OtherPlayerPacketSize);
        offset += OtherPlayerPacketSize;
        

        return buffer;
    }
};
class MAINSERVER_SendRoomList : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_SENDROOMLIST;

    PacketConvertList<string> RoomHashDatas;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        RoomHashDatas.Empty();
        int RoomHashDatasCount;
        std::memcpy(&RoomHashDatasCount, &data[offset], sizeof(int));
        offset += sizeof(int);
        for(int i = 0; i < RoomHashDatasCount; ++i)
        {
            uint16_t strLength;
            std::memcpy(&strLength, &data[offset], sizeof(uint16_t));
            offset += sizeof(uint16_t);
            std::string tempString(reinterpret_cast<const char*>(&data[offset]), strLength);
            offset += strLength;
            RoomHashDatas.Enter(tempString);
        }
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        
        size += sizeof(int);
        for(auto& item : RoomHashDatas.Values)
        {
            size += sizeof(uint16_t);
            size += static_cast<uint16_t>(item.size());
        }
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        int count = RoomHashDatas.GetCount();
        std::memcpy(&buffer[offset], &count, sizeof(int));
        offset += sizeof(int);
        for (auto& str : RoomHashDatas.Values)
        {
            uint16_t strLen = static_cast<uint16_t>(str.size());
            std::memcpy(&buffer[offset], &strLen, sizeof(uint16_t));
            offset += sizeof(uint16_t);
            std::memcpy(&buffer[offset], str.data(), strLen);
            offset += strLen;
        }
        
        

        return buffer;
    }
};
class MAINSERVER_JoinRoomSucc : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_JOINROOMSUCC;

    PlayerData MasterPlayer;
    string RoomHashData;
    uint16_t MaxPlayerCount;
    PacketConvertList<PlayerData> OtherPlayers;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t MasterPlayerPacketSize;
        std::memcpy(&MasterPlayerPacketSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        MasterPlayer = PlayerData();
        MasterPlayer.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + MasterPlayerPacketSize));
        offset += MasterPlayerPacketSize;
        
        uint16_t RoomHashDataLength;
        std::memcpy(&RoomHashDataLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        RoomHashData.assign(reinterpret_cast<const char*>(&data[offset]), RoomHashDataLength);
        offset += RoomHashDataLength;
        
        std::memcpy(&MaxPlayerCount, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        uint16_t OtherPlayersCount;
        std::memcpy(&OtherPlayersCount, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        OtherPlayers.Empty();
        
        for (int i = 0; i < OtherPlayersCount; i++)
        {
            uint16_t objectsize;
            std::memcpy(&objectsize, &data[offset], sizeof(uint16_t));
            offset += sizeof(uint16_t);
        
            PlayerData packet;
            packet.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + objectsize));
            OtherPlayers.Enter(packet);
        
            offset += objectsize;
        }
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += MasterPlayer.GetSize();
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(RoomHashData.size());
        size += sizeof(uint16_t);
        
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(sizeof(uint16_t) * (OtherPlayers.GetCount() == 0 ? 1 : OtherPlayers.GetCount()));
        for(auto& obj : OtherPlayers.Values)
        {
            size += obj.GetSize();
        }
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t MasterPlayerPacketSize = MasterPlayer.GetSize();
        std::memcpy(&buffer[offset], &MasterPlayerPacketSize, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        auto MasterPlayerData = MasterPlayer.GetPacketSerialize();
        std::memcpy(&buffer[offset], MasterPlayerData.second.data(), MasterPlayerPacketSize);
        offset += MasterPlayerPacketSize;
        uint16_t RoomHashDataLen = static_cast<uint16_t>(RoomHashData.size());
        std::memcpy(&buffer[offset], &RoomHashDataLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], RoomHashData.data(), RoomHashDataLen);
        offset += RoomHashDataLen;
        
        std::memcpy(&buffer[offset], &MaxPlayerCount, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t count = static_cast<uint16_t>(OtherPlayers.GetCount());
        std::memcpy(&buffer[offset], &count, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        for (auto& packet : OtherPlayers.Values)
        {
            uint16_t packetSize = packet.GetSize();
            std::memcpy(&buffer[offset], &packetSize, sizeof(uint16_t));
            offset += sizeof(uint16_t);
        
            auto data = packet.GetPacketSerialize();
            std::memcpy(&buffer[offset], data.second.data(), packetSize);
            offset += packetSize;
        }
        
        

        return buffer;
    }
};
class MAINSERVER_RoomMasterChanged : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_ROOMMASTERCHANGED;

    PlayerData RoomMasterData;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t RoomMasterDataPacketSize;
        std::memcpy(&RoomMasterDataPacketSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        RoomMasterData = PlayerData();
        RoomMasterData.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + RoomMasterDataPacketSize));
        offset += RoomMasterDataPacketSize;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += RoomMasterData.GetSize();
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t RoomMasterDataPacketSize = RoomMasterData.GetSize();
        std::memcpy(&buffer[offset], &RoomMasterDataPacketSize, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        auto RoomMasterDataData = RoomMasterData.GetPacketSerialize();
        std::memcpy(&buffer[offset], RoomMasterDataData.second.data(), RoomMasterDataPacketSize);
        offset += RoomMasterDataPacketSize;
        

        return buffer;
    }
};
class MAINSERVER_RoomExitSucc : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_ROOMEXITSUCC;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class MAINSERVER_OtherPlayerRoomExit : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_OTHERPLAYERROOMEXIT;

    PlayerData LeavePlayerData;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t LeavePlayerDataPacketSize;
        std::memcpy(&LeavePlayerDataPacketSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        LeavePlayerData = PlayerData();
        LeavePlayerData.Deserialize(std::vector<uint8_t>(data.begin() + offset, data.begin() + offset + LeavePlayerDataPacketSize));
        offset += LeavePlayerDataPacketSize;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += LeavePlayerData.GetSize();
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t LeavePlayerDataPacketSize = LeavePlayerData.GetSize();
        std::memcpy(&buffer[offset], &LeavePlayerDataPacketSize, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        auto LeavePlayerDataData = LeavePlayerData.GetPacketSerialize();
        std::memcpy(&buffer[offset], LeavePlayerDataData.second.data(), LeavePlayerDataPacketSize);
        offset += LeavePlayerDataPacketSize;
        

        return buffer;
    }
};
class MAINSERVER_NoRoomCallback : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_NOROOMCALLBACK;

    string ReturnHashID;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t ReturnHashIDLength;
        std::memcpy(&ReturnHashIDLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        ReturnHashID.assign(reinterpret_cast<const char*>(&data[offset]), ReturnHashIDLength);
        offset += ReturnHashIDLength;
        
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(ReturnHashID.size());
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t ReturnHashIDLen = static_cast<uint16_t>(ReturnHashID.size());
        std::memcpy(&buffer[offset], &ReturnHashIDLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], ReturnHashID.data(), ReturnHashIDLen);
        offset += ReturnHashIDLen;
        
        

        return buffer;
    }
};
class MAINSERVER_SendPeerUser : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_SENDPEERUSER;

    int32_t SessionID;
    string LocalAddress;
    string PublicAddress;
    int32_t Port;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&SessionID, &data[offset], sizeof(int32_t));
        offset += sizeof(int32_t);
        uint16_t LocalAddressLength;
        std::memcpy(&LocalAddressLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        LocalAddress.assign(reinterpret_cast<const char*>(&data[offset]), LocalAddressLength);
        offset += LocalAddressLength;
        
        uint16_t PublicAddressLength;
        std::memcpy(&PublicAddressLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        PublicAddress.assign(reinterpret_cast<const char*>(&data[offset]), PublicAddressLength);
        offset += PublicAddressLength;
        
        std::memcpy(&Port, &data[offset], sizeof(int32_t));
        offset += sizeof(int32_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(int32_t);
        
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(LocalAddress.size());
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(PublicAddress.size());
        size += sizeof(int32_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &SessionID, sizeof(int32_t));
        offset += sizeof(int32_t);
        
        uint16_t LocalAddressLen = static_cast<uint16_t>(LocalAddress.size());
        std::memcpy(&buffer[offset], &LocalAddressLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], LocalAddress.data(), LocalAddressLen);
        offset += LocalAddressLen;
        
        uint16_t PublicAddressLen = static_cast<uint16_t>(PublicAddress.size());
        std::memcpy(&buffer[offset], &PublicAddressLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], PublicAddress.data(), PublicAddressLen);
        offset += PublicAddressLen;
        
        std::memcpy(&buffer[offset], &Port, sizeof(int32_t));
        offset += sizeof(int32_t);
        
        

        return buffer;
    }
};
class MAINSERVER_GameStartSucc : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_GAMESTARTSUCC;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};
class MAINSERVER_SendPbEndPoint : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_SENDPBENDPOINT;

    string IPAddress;
    int32_t Port;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t IPAddressLength;
        std::memcpy(&IPAddressLength, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        IPAddress.assign(reinterpret_cast<const char*>(&data[offset]), IPAddressLength);
        offset += IPAddressLength;
        
        std::memcpy(&Port, &data[offset], sizeof(int32_t));
        offset += sizeof(int32_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        size += static_cast<uint16_t>(IPAddress.size());
        size += sizeof(int32_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        uint16_t IPAddressLen = static_cast<uint16_t>(IPAddress.size());
        std::memcpy(&buffer[offset], &IPAddressLen, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        std::memcpy(&buffer[offset], IPAddress.data(), IPAddressLen);
        offset += IPAddressLen;
        
        std::memcpy(&buffer[offset], &Port, sizeof(int32_t));
        offset += sizeof(int32_t);
        
        

        return buffer;
    }
};
class MAINSERVER_AllCompeletedReady : public PacketInterface
{
public:
    MsgID MsgIDValue = MsgID::MAINSERVER_ALLCOMPELETEDREADY;

    uint16_t timeout;
    

    virtual void Deserialize(const std::vector<uint8_t>& data) override
    {
        if (data.size() < GetSize())
            return;

        size_t offset = 0;

        uint16_t packetSize;
        std::memcpy(&packetSize, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t packetId;
        std::memcpy(&packetId, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&timeout, &data[offset], sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
    }

    virtual uint16_t GetSize() const override
    {
        uint16_t size = 0;
        size += sizeof(uint16_t); // PacketSize
        size += sizeof(uint16_t); // PacketID
        size += sizeof(uint16_t);
        
                
        
        return size;
    }

    virtual std::vector<uint8_t> Serialize() override
    {
        uint16_t size = GetSize();
        std::vector<uint8_t> buffer(size);

        size_t offset = 0;

        std::memcpy(&buffer[offset], &size, sizeof(uint16_t));
        offset += sizeof(uint16_t);

        uint16_t msgId = static_cast<uint16_t>(MsgIDValue);
        std::memcpy(&buffer[offset], &msgId, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        std::memcpy(&buffer[offset], &timeout, sizeof(uint16_t));
        offset += sizeof(uint16_t);
        
        

        return buffer;
    }
};