#include "RecvBuffer.h"

uint8_t* RecvBuffer::ReadPtr()
{
    return _buffer.data() + _readPos;
}


uint8_t* RecvBuffer::WritePtr()
{
    return _buffer.data() + _writePos;
}

void RecvBuffer::Clean()
{
    size_t dataSize = DataSize();

    if (dataSize == 0)
    {
        _readPos = 0;
        _writePos = 0;
    }
    else
    {
        std::memmove(
            _buffer.data(),
            _buffer.data() + _readPos,
            dataSize
        );

        _readPos = 0;
        _writePos = dataSize;
    }
}

bool RecvBuffer::OnRead(size_t numOfBytes)
{
    if (numOfBytes > DataSize())
        return false;

    _readPos += numOfBytes;
    return true;
}

bool RecvBuffer::OnWrite(size_t numOfBytes)
{
    if (numOfBytes > FreeSize())
        return false;

    _writePos += numOfBytes;
    return true;
}
