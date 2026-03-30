#include "PrintLogThread.h"
#include <iostream>
#include <chrono>
#include <thread>

PrintLogThread& PrintLogThread::Instance()
{
    static PrintLogThread instance;
    return instance;
}

PrintLogThread::~PrintLogThread()
{
    Stop();
}

void PrintLogThread::Start()
{
    _running = true;
    _thread = std::thread(&PrintLogThread::Run, this);
}

void PrintLogThread::Stop()
{
    _running = false;

    if (_thread.joinable())
        _thread.join();
}

void PrintLogThread::Log(const std::string& msg)
{
    _stack.Push(msg);
}

void PrintLogThread::Run()
{
    while (_running)
    {
        auto node = _stack.Pop();

        if (node)
        {
            std::cout << node->data << std::endl;
            delete node;
        }
        else
        {
            // 바쁘게 도는거 방지 (CPU 100% 방지)
            std::this_thread::sleep_for(std::chrono::milliseconds(1));
        }
    }

    // 종료 시 남은 로그 flush
    while (true)
    {
        auto node = _stack.Pop();
        if (!node)
            break;

        std::cout << node->data << std::endl;
        delete node;
    }
}