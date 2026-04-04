#include <iostream>
#include <string>

#ifdef _WIN32
#include <winsock2.h>
#include <ws2tcpip.h>
#pragma comment(lib, "ws2_32.lib")
#else
#include <arpa/inet.h>
#include <unistd.h>
#endif

std::string GetLocalIP() {
#ifdef _WIN32
    WSADATA wsa;
    WSAStartup(MAKEWORD(2, 2), &wsa);
#endif

    std::string result = "0.0.0.0";

    int sock = socket(AF_INET, SOCK_DGRAM, 0);
    if (sock < 0) return result;

    sockaddr_in serv{};
    serv.sin_family = AF_INET;
    serv.sin_port = htons(53);

#ifdef _WIN32
    InetPtonA(AF_INET, "8.8.8.8", &serv.sin_addr);
#else
    inet_pton(AF_INET, "8.8.8.8", &serv.sin_addr);
#endif

    connect(sock, (sockaddr*)&serv, sizeof(serv));

    sockaddr_in name{};
#ifdef _WIN32
    int namelen = sizeof(name);
#else
    socklen_t namelen = sizeof(name);
#endif

    if (getsockname(sock, (sockaddr*)&name, &namelen) == 0) {
        char buffer[INET_ADDRSTRLEN] = { 0 };

#ifdef _WIN32
        InetNtopA(AF_INET, &name.sin_addr, buffer, INET_ADDRSTRLEN);
#else
        inet_ntop(AF_INET, &name.sin_addr, buffer, INET_ADDRSTRLEN);
#endif

        result = buffer;
    }

#ifdef _WIN32
    closesocket(sock);
    WSACleanup();
#else
    close(sock);
#endif

    return result;
}