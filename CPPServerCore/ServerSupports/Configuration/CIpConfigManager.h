#pragma once

#include <string>

struct StartUpIpEndPoint
{
    std::string IPAddress;
    int Port;
};

struct IPConfig
{
    StartUpIpEndPoint TCPIPEndPoint;
    StartUpIpEndPoint UDPIPEndPoint;
};

class IpConfigManager
{
private:
    std::string IPConfigFile = "ipConfig.config";
    static IpConfigManager* inst;

    IpConfigManager();

public:
    static IpConfigManager& Instance();

    IPConfig ipConfig;
};