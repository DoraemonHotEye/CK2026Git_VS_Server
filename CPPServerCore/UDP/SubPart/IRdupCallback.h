#pragma once
#include <vector>
#include <cstdint>
#include <winsock2.h>

class IRudpCallback
{
public:
    virtual ~IRudpCallback() = default;
    virtual void OnResend(const std::vector<uint8_t>& buffer, uint32_t seqNum, SOCKADDR_IN& addr) = 0;
};