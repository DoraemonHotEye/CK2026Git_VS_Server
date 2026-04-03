#include "PrintLogThread.h"
#include <iostream>
#include <chrono>
#include <thread>
#include "../CommandManager/CCommandManager.h"

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
            _tempBuffer.push_back(node->data);
            continue;
        }

        // 큐가 비었을 때 flush
        if (!_tempBuffer.empty())
        {
            bool usePrompt = CCommandManager::Instance().isRunning();

            // 현재 입력 가져오기
            std::string currentInput;
            if (usePrompt)
                currentInput = CCommandManager::Instance().GetCurrentInput(); // 새로 추가된 함수

            if (usePrompt)
            {
                // 입력 라인 삭제 (ANSI 사용 가능 시 더 깔끔)
                std::cout << "\r" << std::string(120, ' ') << "\r";
            }

            // 로그 출력 (FIFO)
            for (auto it = _tempBuffer.rbegin(); it != _tempBuffer.rend(); ++it)
            {
                std::cout << (*it) << std::endl;
            }
            _tempBuffer.clear();

            if (usePrompt)
            {
                // 프롬프트 + 입력 복구
                std::cout << "root-#command $: " << currentInput;
                std::cout.flush();
            }
        }

        // CPU 점유 방지
        std::this_thread::sleep_for(std::chrono::milliseconds(1));
    }

    while (true)
    {
        auto node = _stack.Pop();
        if (!node)
            break;

        _tempBuffer.push_back(node->data);
    }

    if (!_tempBuffer.empty())
    {
        for (auto it = _tempBuffer.rbegin(); it != _tempBuffer.rend(); ++it)
        {
            std::cout << (*it) << std::endl;
        }
        _tempBuffer.clear();
    }
}