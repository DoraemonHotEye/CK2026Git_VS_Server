#pragma once
#include <Windows.h>
#include <thread>
#include <atomic>
#include <string>
#include <deque>
#include <mutex>

class GUIScreen
{
private:
    static GUIScreen* inst;

public:
    static GUIScreen& Instance();

public:
    void Start();
    void Stop();

    void UpdateScreen();
    void RenderScreen();

    GUIScreen();
    ~GUIScreen();

    void EnterLog(const std::string& log);

private:
    void Run(); // 스레드 루프
    void GotoXY(int x, int y);
    int GetCPUUsage();
    int GetGPUUsage();
    void RenderLogs(wchar_t screen[][81], int startY, int width, int height);
    
public:
    bool isRunning();

private:
    std::thread _thread;
    std::atomic<bool> _running;

    std::deque<std::string> _logQueue;
    std::mutex _logMutex;
    const size_t _maxLogCount = 100;
};