#pragma once
#include <vector>
#include <cstdint>
#include <cstring>

class RecvBuffer
{
public:
    // [r][][w][][][][][]
    RecvBuffer(size_t bufferSize)
        : _buffer(bufferSize), _readPos(0), _writePos(0)
    {
    }

public:
    size_t DataSize() const { return _writePos - _readPos; }
    size_t FreeSize() const { return _buffer.size() - _writePos; }

    uint8_t* ReadPtr();
    uint8_t* WritePtr();

public:
    void Clean();

public:
    bool OnRead(size_t numOfBytes);
    bool OnWrite(size_t numOfBytes);

private:
    std::vector<uint8_t> _buffer;
    size_t _readPos = 0;
    size_t _writePos = 0;
};