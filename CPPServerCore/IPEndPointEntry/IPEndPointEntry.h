#include <chrono>
class IPEndPointEntry
{
public:
    SOCKADDR_IN Address;

    std::chrono::steady_clock::time_point LastResponseTime;
    std::chrono::steady_clock::time_point LastPingTime;

    IPEndPointEntry(const SOCKADDR_IN& addr)
        : Address(addr)
    {
        auto now = std::chrono::steady_clock::now();
        LastResponseTime = now;
        LastPingTime = now;
    }

    double GetSinceLastResponse() const
    {
        return std::chrono::duration<double>(
            std::chrono::steady_clock::now() - LastResponseTime).count();
    }

    double GetSinceLastPing() const
    {
        return std::chrono::duration<double>(
            std::chrono::steady_clock::now() - LastPingTime).count();
    }

    void OnPingSent()
    {
        LastPingTime = std::chrono::steady_clock::now();
    }

    void OnResponse()
    {
        LastResponseTime = std::chrono::steady_clock::now();
    }
};