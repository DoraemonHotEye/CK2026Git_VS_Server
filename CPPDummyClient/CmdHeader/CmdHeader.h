#pragma once

#include <string>
#include <vector>
#include "../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../../CPPServerCore/CommandManager/CCommandManager.h"

CSession* CurrentHead = nullptr;

void ShowIpEndpoints(const std::vector<std::string>& params)
{
    int step = 0;
    int step2 = 0;
    bool useCount = false; // count 모드 여부

    auto isNumber = [](const std::string& s) -> bool {
        if (s.empty()) return false;
        for (char c : s)
            if (!isdigit(static_cast<unsigned char>(c))) return false;
        return true;
        };

    if (params.size() < 2) return;
    if (params[1] != "ipendpoints") return;

    size_t i = 2;
    while (i < params.size())
    {
        if (params[i] == "count" && (i + 2) < params.size() && params[i + 1] == "by")
        {
            if (isNumber(params[i + 2]))
            {
                useCount = true;
                step2 = std::stoi(params[i + 2]);
                i += 3;
                continue;
            }
            else
            {
                LOG("[ERROR] Invalid parameter after 'count by': " + params[i + 2]);
                return;
            }
        }
        else if (params[i] == "order" && (i + 2) < params.size() && params[i + 1] == "by")
        {
            if (isNumber(params[i + 2]))
            {
                step2 = std::stoi(params[i + 2]);
                i += 3;
                continue;
            }
            else
            {
                LOG("[ERROR] Invalid parameter after 'order by': " + params[i + 2]);
                return;
            }
        }
        else if (isNumber(params[i]))
        {
            step = std::stoi(params[i]);
            ++i;
            continue;
        }
        else
        {
            LOG("[ERROR] Unexpected parameter: " + params[i]);
            return;
        }
    }

    std::vector<std::string> endpoints;
    if (useCount)
        endpoints = SessionManager::Instance().GetEndPointCount(step, step2);
    else
        endpoints = SessionManager::Instance().GetEndPointsOrderBy(step, step2);

    if (endpoints.empty())
    {
        LOG("[INFO] No endpoints available.");
    }
    else
    {
        for (size_t i = 0; i < endpoints.size(); ++i)
        {
            LOG((useCount ? "Count " : "Endpoint ") + std::to_string(step + i) + ": " + endpoints[i]);
        }
    }
}

void UseSessionCommand(const std::vector<std::string>& params)
{
    if (params.size() < 3)
    {
        LOG("[ERROR] Usage: use session <ID>");
        return;
    }

    if (params[1] != "session")
    {
        LOG("[ERROR] Unknown use target: " + params[1]);
        return;
    }

    // 세션 ID 파싱
    int sessionID = 0;
    try
    {
        sessionID = std::stoi(params[2]);
    }
    catch (const std::exception& e)
    {
        LOG("[ERROR] Invalid session ID: " + params[2]);
        return;
    }

    // 세션 가져오기
    CSession* sess = SessionManager::Instance().Get(sessionID);
    if (!sess)
    {
        LOG("[ERROR] Session ID " + std::to_string(sessionID) + " not found.");
        return;
    }

    CurrentHead = sess;
    LOG("[INFO] Now using session ID " + std::to_string(sessionID));
}

void CreateRoomCallPacket(const std::vector<std::string>& params)
{
    if (params.size() < 2)
    {
        LOG("[ERROR] Invalid command line args 2");
        return;
    }
    if (params[1] == "room")
    {
        if (CurrentHead == nullptr)
        {
            LOG("[ERROR] 현재 사용 중인 세션이 없음");
        }

        GAMECLIENT_CreateRoomCall roomcall = GAMECLIENT_CreateRoomCall();
        roomcall.timeout = 10;
        CurrentHead->Send(roomcall);

        LOG("[INFO] Execute Send Packet PacketName: GAMECLIENT_CreateRoomCall");
    }
}

void RoomCommands(const std::vector<std::string>& params) 
{
    if (params.size() < 2)
    {
        LOG("[ERROR] Invalid command line args 2");
        return;
    }
    if (params[1] == "join")
    {
        if (params.size() < 3)
        {
            LOG("[ERROR] Invalid command line args 3");
            return;
        }

        if (CurrentHead == nullptr)
        {
            LOG("[ERROR] 현재 사용 중인 세션이 없음");
        }

        try
        {
            std::string roomName = params[2];

            GAMECLIENT_JoinRoomCall roomcall = GAMECLIENT_JoinRoomCall();
            roomcall.RoomHashID = roomName;
            CurrentHead->Send(roomcall);

            LOG("[INFO] Execute Send Packet PacketName: GAMECLIENT_JoinRoomCall");
        }
        catch (const std::exception& e)
        {
            LOG("[ERROR] Invalid session ID: " + params[2]);
            return;
        }
    }
    else if (params[1] == "start") 
    {
        if (CurrentHead == nullptr)
        {
            LOG("[ERROR] 현재 사용 중인 세션이 없음");
        }

        try
        {
            GAMECLIENT_GameStart roomcall = GAMECLIENT_GameStart();
            roomcall.timeout = 10;
            CurrentHead->Send(roomcall);

            LOG("[INFO] Execute Send Packet PacketName: GAMECLIENT_GameStart");
        }
        catch (const std::exception& e)
        {
            LOG("[ERROR] Invalid session ID: " + params[2]);
            return;
        }
    }
    else if (params[1] == "leave") 
    {
        if (CurrentHead == nullptr)
        {
            LOG("[ERROR] 현재 사용 중인 세션이 없음");
        }

        try
        {
            GAMECLIENT_RoomExitCall roomcall = GAMECLIENT_RoomExitCall();
            roomcall.timeout = 10;
            CurrentHead->Send(roomcall);

            LOG("[INFO] Execute Send Packet PacketName: GAMECLIENT_RoomExitCall");
        }
        catch (const std::exception& e)
        {
            LOG("[ERROR] Invalid session ID: " + params[2]);
            return;
        }
    }
    else if (params[1] == "ready")
    {
        if (CurrentHead == nullptr)
        {
            LOG("[ERROR] 현재 사용 중인 세션이 없음");
        }

        try
        {
            GAMECLIENT_GameStartReady ready = GAMECLIENT_GameStartReady();
            ready.timeout = 10;
            CurrentHead->Send(ready);

            LOG("[INFO] Execute Send Packet PacketName: GAMECLIENT_GameStartReady");
        }
        catch (const std::exception& e)
        {
            LOG("[ERROR] Invalid session ID: " + params[2]);
            return;
        }
    }
}

void RegisterCommand() 
{
    auto& cmdMgr = CCommandManager::Instance();
    cmdMgr.RegisterCommand("show", ShowIpEndpoints);
    cmdMgr.RegisterCommand("use", UseSessionCommand);
    cmdMgr.RegisterCommand("create", CreateRoomCallPacket);
    cmdMgr.RegisterCommand("room", RoomCommands);

    cmdMgr.Start();
}