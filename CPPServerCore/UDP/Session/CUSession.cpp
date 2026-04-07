#include "CUSession.h"
#include <MSWSock.h>
#include "../../Headers/Packets.h"
#include "../../ServerSupports/PrintLogThread.h"

void CUSession::Start(std::string ipAddress, u_short port)
{
    _socket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (_socket == INVALID_SOCKET) {
        LOG("Socket creation failed: " + std::to_string(WSAGetLastError()));
        return;
    }

    // Windows UDP 포트 에러(10054) 방지 옵션
    DWORD dwBytesReturned = 0;
    BOOL bNewBehavior = FALSE;
    WSAIoctl(_socket, SIO_UDP_CONNRESET, &bNewBehavior, sizeof(bNewBehavior), NULL, 0, &dwBytesReturned, NULL, NULL);

    SOCKADDR_IN addr;
    addr.sin_family = AF_INET;

    inet_pton(
        AF_INET,
        ipAddress.c_str(), // 수정된 부분
        &addr.sin_addr
    );

    addr.sin_port = htons(port);

    if (bind(_socket, (SOCKADDR*)&addr, sizeof(addr)) == SOCKET_ERROR) {
        LOG("Bind failed: " + std::to_string(WSAGetLastError()));
        closesocket(_socket);
        return;
    }

    RegisterUpdateMethod();

    _stopThread = false;
    _recvThread = std::thread(&CUSession::ReceiveLoop, this);

    LOG("UDP Server Started (Blocking Mode) on Port: " + std::to_string(port));
}

void CUSession::StartClient(const std::string& serverIP, u_short port)
{
    _socket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (_socket == INVALID_SOCKET) {
        LOG("Socket creation failed: " + std::to_string(WSAGetLastError()));
        return;
    }

    // [중요] 클라이언트 소켓 바인딩: 포트를 0으로 설정하면 OS가 자동으로 할당함
    SOCKADDR_IN localAddr = {};
    localAddr.sin_family = AF_INET;
    localAddr.sin_addr.s_addr = htonl(INADDR_ANY);
    localAddr.sin_port = htons(0); // 임의의 포트 할당

    if (bind(_socket, (SOCKADDR*)&localAddr, sizeof(localAddr)) == SOCKET_ERROR) {
        LOG("Client Bind failed: " + std::to_string(WSAGetLastError()));
        closesocket(_socket);
        return;
    }

    // Windows UDP 10054 방지 옵션
    DWORD dwBytesReturned = 0;
    BOOL bNewBehavior = FALSE;
    WSAIoctl(_socket, SIO_UDP_CONNRESET, &bNewBehavior, sizeof(bNewBehavior), NULL, 0, &dwBytesReturned, NULL, NULL);

    // 서버 주소 설정 (Send용)
    memset(&_serverAddr, 0, sizeof(_serverAddr));
    _serverAddr.sin_family = AF_INET;
    _serverAddr.sin_port = htons(port);
    InetPtonA(AF_INET, serverIP.c_str(), &_serverAddr.sin_addr);

    _stopThread = false;
    _recvThread = std::thread(&CUSession::ReceiveLoop, this);

    RegisterUpdateMethod();

    LOG("UDP Client Started. Local Port Assigned.");
}

void CUSession::ReceiveLoop()
{
    char recvBuffer[65535]; // UDP 최대 패킷 크기
    SOCKADDR_IN clientAddr;
    int addrLen = sizeof(clientAddr);

    while (!_stopThread)
    {
        // 데이터가 올 때까지 여기서 멈춤 (Blocking)
        int bytesReceived = recvfrom(_socket, recvBuffer, sizeof(recvBuffer), 0, (SOCKADDR*)&clientAddr, &addrLen);

        if (bytesReceived == SOCKET_ERROR) {
            int err = WSAGetLastError();
            if (_stopThread) break;
            LOG("recvfrom error: " + std::to_string(err));
            continue;
        }

        try {
            // 데이터 처리
            OnRecv(recvBuffer, bytesReceived, clientAddr);
        }
        catch (const std::exception& e) {
            LOG("Exception in OnRecv: " + std::string(e.what()));
        }
    }
}

void CUSession::SendTo(SOCKADDR_IN& addr, const char* data, int len)
{
    if (_socket == INVALID_SOCKET) return;

    int result = sendto(_socket, data, len, 0, (SOCKADDR*)&addr, sizeof(addr));
    if (result == SOCKET_ERROR) {
        LOG("sendto failed: " + std::to_string(WSAGetLastError()));
    }
}

void CUSession::Send(PacketInterface& packet, SOCKADDR_IN& addr, bool isUseRUDP)
{
    // 1. 패킷 직렬화 (이 작업은 락 밖에서 수행하여 성능 확보)
    std::vector<uint8_t> sendBytes = packet.Serialize();
    if (sendBytes.empty()) return;

    try
    {
        if (isUseRUDP)
        {
            // RUDP 헤더 위치 파악 (dataSize:2, packetId:2, seqNum:2 순서라고 가정)
            // 직렬화된 데이터의 4바이트 지점(seqNum)에 값을 덮어씌움
            int seqOffset = sizeof(uint16_t) + sizeof(uint16_t);

            if (sendBytes.size() >= seqOffset + sizeof(uint16_t))
            {
                // [seqNum:2] 생성 및 기록
                uint16_t seqNum = ++_sendSeq; // atomic 증가
                *reinterpret_cast<uint16_t*>(&sendBytes[seqOffset]) = seqNum;

                // 재전송을 위해 테이블에 추가 (멀티스레드 보호)
                {
                    std::lock_guard<std::mutex> lock(_tableMutex);
                    // 복사본을 전달하여 재전송 시 사용
                    sendTable.Add(seqNum, sendBytes, addr, this);
                }
            }
            else {
                LOG("Error: Packet size too small for RUDP seqNum");
            }
        }

        // 2. 실제 소켓 전송
        SendTo(addr, reinterpret_cast<const char*>(sendBytes.data()), (int)sendBytes.size());

        // 성공 로그/콜백 (필요 시)
        OnSend(true, addr);
    }
    catch (const std::exception& e)
    {
        LOG("Exception in Send: " + std::string(e.what()));
        OnSend(false, addr);
    }
    catch (...)
    {
        LOG("Unknown Error in Send");
        OnSend(false, addr);
    }
}

void CUSession::Update()
{
    // Update는 메인 스레드에서 호출되므로 맵 보호 필요
    std::lock_guard<std::mutex> lock(_tableMutex);

    sendTable.Update();
    for (auto& pair : RequestTables) {
        pair.second->Update();
    }
}

void CUSession::Stop()
{
    _stopThread = true;
    if (_socket != INVALID_SOCKET) {
        closesocket(_socket);
        _socket = INVALID_SOCKET;
    }
    if (_recvThread.joinable()) {
        _recvThread.join();
    }
}