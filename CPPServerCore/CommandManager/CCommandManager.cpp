#include "CCommandManager.h"
#include "../ServerSupports/PrintLogThread.h"
#include <sstream>
#include <iostream>
#include <conio.h>

// static 초기화
CCommandManager* CCommandManager::inst = nullptr;

CCommandManager::CCommandManager()
    : running(false)
{
}

CCommandManager& CCommandManager::Instance()
{
    if (!inst)
        inst = new CCommandManager();
    return *inst;
}

const bool CCommandManager::isRunning() const
{
    return running;
}

std::string CCommandManager::GetCurrentInput()
{
    std::lock_guard<std::mutex> lock(_inputMutex);
    return _currentInput;
}

// 입력 루프
void CCommandManager::InputLoop()
{
    while (running)
    {
        if (_kbhit())
        {
            char c = _getch();

            std::lock_guard<std::mutex> lock(_inputMutex);

            if (c == '\r') // Enter
            {
                std::string cmd = _currentInput;
                _currentInput.clear();

                // 로그로 남기고 명령어 실행
                PrintLogThread::Instance().Log("[INPUT RECEIVED] " + cmd);
                ExecuteCommand(cmd);

                // 줄바꿈 + 프롬프트 복구
                std::cout << std::endl << "root-#command $: ";
                std::cout.flush();
            }
            else if (c == '\b') // Backspace
            {
                if (!_currentInput.empty())
                {
                    _currentInput.pop_back();
                    std::cout << "\b \b";
                    std::cout.flush();
                }
            }
            else
            {
                _currentInput.push_back(c);
                std::cout << c; // 즉시 출력
                std::cout.flush();
            }
        }

        // CPU 점유 방지
        std::this_thread::sleep_for(std::chrono::milliseconds(1));
    }
}

// 명령어 파싱
std::vector<std::string> CCommandManager::ParseCommand(const std::string& cmdLine)
{
    std::vector<std::string> tokens;
    std::istringstream stream(cmdLine);
    std::string token;
    bool inQuotes = false;
    std::string buffer;

    while (stream >> token)
    {
        if (token.front() == '"' && token.back() == '"') // "order by" 전체
        {
            token = token.substr(1, token.size() - 2);
            tokens.push_back(token);
        }
        else if (token.front() == '"') // 시작
        {
            buffer = token.substr(1);
            inQuotes = true;
        }
        else if (token.back() == '"' && inQuotes) // 끝
        {
            buffer += " " + token.substr(0, token.size() - 1);
            tokens.push_back(buffer);
            buffer.clear();
            inQuotes = false;
        }
        else if (inQuotes)
        {
            buffer += " " + token;
        }
        else
        {
            tokens.push_back(token);
        }
    }

    return tokens;
}

// 명령어 등록
void CCommandManager::RegisterCommand(const std::string& cmdName, std::function<void(const std::vector<std::string>&)> func)
{
    commandMap[cmdName] = func;
}

// 명령어 실행
void CCommandManager::ExecuteCommand(const std::string& cmdLine)
{
    auto tokens = ParseCommand(cmdLine);
    if (tokens.empty()) return;

    auto it = commandMap.find(tokens[0]);
    if (it != commandMap.end())
    {
        it->second(tokens); // tokens 전체 전달
    }
    else
    {
        PrintLogThread::Instance().Log("[ERROR] Unknown command: " + tokens[0]);
    }
}

// Start
void CCommandManager::Start()
{
    running = true;
    inputThread = std::thread(&CCommandManager::InputLoop, this);
    inputThread.detach();
}