#pragma once
#include <functional>
#include <winsock2.h>

class CSession;

using OnAcceptCallback = std::function<CSession* (SOCKET)>;

using OnConnectCallback = std::function<CSession* (SOCKET)>;