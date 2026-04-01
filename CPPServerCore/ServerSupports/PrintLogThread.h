#pragma once
#include <thread>
#include <atomic>
#include "LockFreeStack.h"
#include <string>
#include <vector>

#define LOG(msg) PrintLogThread::Instance().Log(msg)

class PrintLogThread
{
public:
    static PrintLogThread& Instance();

    void Start();
    void Stop();

    void Log(const std::string& msg);

private:
    PrintLogThread() = default;
    ~PrintLogThread();

    void Run();

private:
    LockFreeStack<std::string> _stack;
    std::vector<std::string> _tempBuffer;

    std::thread _thread;
    std::atomic<bool> _running = false;
};