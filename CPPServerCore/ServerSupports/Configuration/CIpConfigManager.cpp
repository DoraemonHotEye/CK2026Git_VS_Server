#include "CIpConfigManager.h"
#include <fstream>
#include <iostream>

// nlohmann/json
#include <nlohmann/json.hpp>

using json = nlohmann::json;

IpConfigManager* IpConfigManager::inst = nullptr;

// JSON 변환 정의
void to_json(json& j, const StartUpIpEndPoint& p)
{
    j = json{
        {"IPAddress", p.IPAddress},
        {"Port", p.Port}
    };
}

void from_json(const json& j, StartUpIpEndPoint& p)
{
    j.at("IPAddress").get_to(p.IPAddress);
    j.at("Port").get_to(p.Port);
}

void to_json(json& j, const IPConfig& p)
{
    j = json{
        {"TCPIPEndPoint", p.TCPIPEndPoint},
        {"UDPIPEndPoint", p.UDPIPEndPoint}
    };
}

void from_json(const json& j, IPConfig& p)
{
    j.at("TCPIPEndPoint").get_to(p.TCPIPEndPoint);
    j.at("UDPIPEndPoint").get_to(p.UDPIPEndPoint);
}

// 싱글톤
IpConfigManager& IpConfigManager::Instance()
{
    if (inst == nullptr)
        inst = new IpConfigManager();

    return *inst;
}

// 생성자
IpConfigManager::IpConfigManager()
{
    std::ifstream infile(IPConfigFile);

    if (!infile.good())
    {
        // 파일 없으면 생성
        ipConfig.TCPIPEndPoint.IPAddress = "127.0.0.1";
        ipConfig.TCPIPEndPoint.Port = 5555;

        ipConfig.UDPIPEndPoint.IPAddress = "127.0.0.1";
        ipConfig.UDPIPEndPoint.Port = 5556;

        json j = ipConfig;

        std::ofstream outfile(IPConfigFile);
        outfile << j.dump(4);
        outfile.close();
    }
    else
    {
        // 파일 있으면 읽기
        json j;
        infile >> j;
        ipConfig = j.get<IPConfig>();
    }
}