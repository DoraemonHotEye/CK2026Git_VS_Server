#pragma once
#include <vector>
#include <cstdint>
#include <cstring> // memcpy
#include <tuple>

// ---------------- PacketInterface ----------------
class PacketInterface
{
public:
    virtual ~PacketInterface() = default;
    virtual std::vector<uint8_t> Serialize() = 0;
    virtual void Deserialize(const std::vector<uint8_t>& data) = 0;
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

// ---------------- MsgID ----------------
enum class MsgID : uint16_t
{
    DGRAM_SERVER_REQUESTACK = 1
};

// ---------------- DGRAM_SERVER_RequestAck ----------------
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