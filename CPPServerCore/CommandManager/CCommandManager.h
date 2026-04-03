#pragma once
#include <thread>
#include <mutex>
#include <string>
#include <vector>
#include <map>
#include <functional>

class CCommandManager
{
private:
    static CCommandManager* inst;

    std::thread inputThread;
    bool running;
    std::mutex coutMutex;

    std::string _currentInput;
    std::mutex _inputMutex;

    // 명령어 맵: 키 = command string, 값 = 멤버 함수 포인터
    std::map<std::string, std::function<void(const std::vector<std::string>&)>> commandMap;

private:
    CCommandManager();
    void InputLoop();
    std::vector<std::string> ParseCommand(const std::string& cmdLine);

public:
    static CCommandManager& Instance();
    std::string GetCurrentInput();

    void Start();
    const bool isRunning() const;

    // 명령어 등록
    void RegisterCommand(const std::string& cmdName, std::function<void(const std::vector<std::string>&)> func);

    // 명령어 실행
    void ExecuteCommand(const std::string& cmdLine);
};