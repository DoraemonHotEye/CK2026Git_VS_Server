#pragma once
#include <functional>
#include <winsock2.h>
#include <vector>

class CSession;

using OnAcceptCallback = std::function<CSession* (SOCKET)>;
using OnConnectCallback = std::function<CSession* (SOCKET)>;

using OnUDPSendCallback = std::function<void* (SOCKADDR_IN&)>;
