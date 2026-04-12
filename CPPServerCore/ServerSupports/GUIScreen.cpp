#define WIN32_LEAN_AND_MEAN
#include <winsock2.h>
#include <windows.h>

#include "GUIScreen.h"
#include <iostream>
#include <locale.h>
#include <io.h>
#include <fcntl.h>
#include <pdh.h>
#include "../Managers/SessionManager.h"
#include "../Session/CSession.h"
#include "../Managers/UIPEndPointManager.h"

#pragma comment(lib, "pdh.lib")

using namespace std;

std::wstring Utf8ToWide(const std::string& str)
{
    if (str.empty()) return L"";

    int size_needed = MultiByteToWideChar(
        CP_UTF8, 0,
        str.c_str(), (int)str.size(),
        NULL, 0
    );

    std::wstring wstr(size_needed, 0);

    MultiByteToWideChar(
        CP_UTF8, 0,
        str.c_str(), (int)str.size(),
        &wstr[0], size_needed
    );

    return wstr;
}

GUIScreen* GUIScreen::inst = nullptr;

GUIScreen& GUIScreen::Instance() 
{
    if (inst == nullptr)
        inst = new GUIScreen();

    return *inst;
}

void GUIScreen::GotoXY(int x, int y)
{
    COORD pos = { (SHORT)x, (SHORT)y };
    SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), pos);
}

int GUIScreen::GetCPUUsage()
{
    static FILETIME prevIdle = {}, prevKernel = {}, prevUser = {};

    FILETIME idle, kernel, user;
    if (!GetSystemTimes(&idle, &kernel, &user))
        return -1;

    // FILETIME → uint64 변환 람다
    auto toUint64 = [](FILETIME ft) -> uint64_t {
        return ((uint64_t)ft.dwHighDateTime << 32) | ft.dwLowDateTime;
        };

    uint64_t idleDiff = toUint64(idle) - toUint64(prevIdle);
    uint64_t kernelDiff = toUint64(kernel) - toUint64(prevKernel);
    uint64_t userDiff = toUint64(user) - toUint64(prevUser);

    prevIdle = idle;
    prevKernel = kernel;
    prevUser = user;

    uint64_t total = kernelDiff + userDiff;
    if (total == 0) return 0;

    // kernel 시간에 idle이 포함되어 있어서 빼줘야 함
    int usage = (int)(100 * (total - idleDiff) / total);
    return usage;
}

int GUIScreen::GetGPUUsage()
{
    static PDH_HQUERY   query = nullptr;
    static PDH_HCOUNTER counter = nullptr;
    static bool initialized = false;

    if (!initialized)
    {
        PdhOpenQuery(nullptr, 0, &query);

        // GPU 엔진 사용률 카운터 (DirectX 기준, 범용)
        PdhAddEnglishCounterW(
            query,
            L"\\GPU Engine(*)\\Utilization Percentage",
            0,
            &counter
        );

        PdhCollectQueryData(query); // 첫 샘플 (기준값)
        initialized = true;
        return 0; // 첫 호출은 기준값이라 0 반환
    }

    PdhCollectQueryData(query);

    PDH_FMT_COUNTERVALUE value;
    PdhGetFormattedCounterValue(counter, PDH_FMT_DOUBLE, nullptr, &value);

    return (int)value.doubleValue;
}

void GUIScreen::RenderLogs(wchar_t screen[][81], int startY, int width, int height)
{
    std::lock_guard<std::mutex> lock(_logMutex);
    int logAreaHeight = height - startY;
    int logCount = (int)_logQueue.size();
    int startIndex = max(0, logCount - logAreaHeight);
    int y = startY;
    for (int i = startIndex; i < logCount && y < height; i++, y++)
    {
        const std::wstring log = Utf8ToWide(_logQueue[i]);
        int x = 2;
        for (int j = 0; j < (int)log.size() && x < width - 1; j++, x++)
        {
            screen[y][x] = log[j];
        }
        // 로그 끝 이후 ~ 오른쪽 끝까지 공백으로 덮어쓰기
        for (; x < width - 1; x++)
        {
            screen[y][x] = L' ';
        }
    }

    // 로그가 줄어든 경우 남은 행도 공백으로 정리
    for (; y < height; y++)
    {
        for (int x = 2; x < width - 1; x++)
            screen[y][x] = L' ';
    }
}

bool GUIScreen::isRunning()
{
    return _running;
}

void GUIScreen::UpdateScreen()
{
    // 나중에 값 갱신용 (현재는 비워둠)
}

void GUIScreen::RenderScreen()
{
    const int width = 80;
    const int height = 25;
    const int boxHeight = 8;
    wchar_t screen[25][81];

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
            screen[y][x] = L' ';
        screen[y][width] = L'\0';
    }

    screen[0][0] = L'┌';
    screen[0][width - 1] = L'┐';
    for (int x = 1; x < width - 1; x++)
        screen[0][x] = L'─';
    screen[boxHeight][0] = L'└';
    screen[boxHeight][width - 1] = L'┘';
    for (int x = 1; x < width - 1; x++)
        screen[boxHeight][x] = L'─';
    for (int y = 1; y < boxHeight; y++)
    {
        screen[y][0] = L'│';
        screen[y][width - 1] = L'│';
    }

    auto PutString = [&](int y, int x, const wchar_t* str)
        {
            if (y < 0 || y >= height) return;
            if (x < 0 || x >= width) return;
            for (int i = 0; str[i] != 0; i++)
            {
                if (x + i >= width - 1) break;
                screen[y][x + i] = str[i];
            }
        };

    int clientSessionCount = SessionManager::Instance().GetSessionCount();
    int uiPeerCount = UIPEndPointManager::Instance().GetEndPointCount();
    int cpuUsage = GetCPUUsage();
    int gpuUsage = GetGPUUsage();
    wchar_t buffer[64];

    PutString(1, 2, L"[ Server Status ]");
    swprintf_s(buffer, _countof(buffer), L"ClientSession : %d", clientSessionCount);
    PutString(3, 2, buffer);
    swprintf_s(buffer, _countof(buffer), L"UIPEndPointPeer : %d", uiPeerCount);
    PutString(4, 2, buffer);
    swprintf_s(buffer, _countof(buffer), L"CPU : %d%%", cpuUsage);
    PutString(3, 40, buffer);
    swprintf_s(buffer, _countof(buffer), L"GPU : %d%%", gpuUsage);
    PutString(4, 40, buffer);

    for (int x = 0; x < width; x++)
        screen[boxHeight + 1][x] = L'─';
    PutString(boxHeight + 2, 2, L"[ Logs ]");

    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    DWORD written;

    int logStartY = boxHeight + 3;
    RenderLogs(screen, logStartY, width, height);

    // 여기가 수정된 부분 : wcslen → width 고정, \n 제거, 행마다 커서 직접 이동
    for (int y = 0; y < height; y++)
    {
        COORD pos = { 0, (SHORT)y };
        SetConsoleCursorPosition(hConsole, pos);
        WriteConsoleW(hConsole, screen[y], width, &written, NULL);
    }
}

void GUIScreen::Start()
{
    if (_running) return;

    _running = true;
    _thread = std::thread(&GUIScreen::Run, this);
}

void GUIScreen::Stop()
{
    if (_running == false) return;

    _running = false;

    if (_thread.joinable())
        _thread.join();
}

void GUIScreen::Run()
{
    while (_running)
    {
        UpdateScreen();
        RenderScreen();

        std::this_thread::sleep_for(std::chrono::milliseconds(100)); // 10 FPS
    }
}

GUIScreen::GUIScreen()
{
    _running = false;

    CONSOLE_CURSOR_INFO info;
    info.dwSize = 1;
    info.bVisible = FALSE;
    SetConsoleCursorInfo(GetStdHandle(STD_OUTPUT_HANDLE), &info);

    SetConsoleOutputCP(CP_UTF8);
    SetConsoleCP(CP_UTF8);
}

GUIScreen::~GUIScreen()
{
    Stop();
}

void GUIScreen::EnterLog(const std::string& log)
{
    std::lock_guard<std::mutex> lock(_logMutex);

    _logQueue.push_back(log);

    if (_logQueue.size() > _maxLogCount)
        _logQueue.pop_front(); // 오래된 로그 제거
}
