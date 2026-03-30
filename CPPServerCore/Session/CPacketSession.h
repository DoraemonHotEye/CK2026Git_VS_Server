#include "CSession.h"
#include <iostream>

class CPacketSession : public CSession
{
public:
    static const int HeaderSize = 2;

    virtual int OnRecv(char* buffer, int len) override
    {
        int processLen = 0;

        while (true)
        {
            if (len < HeaderSize)
                break;

            uint16_t dataSize = *(uint16_t*)buffer;

            if (len < dataSize)
                break;

            OnRecvPacket(buffer, dataSize);

            buffer += dataSize;
            len -= dataSize;
            processLen += dataSize;
        }

        return processLen;
    }

    virtual void OnRecvPacket(char* buffer, int len) = 0;
};