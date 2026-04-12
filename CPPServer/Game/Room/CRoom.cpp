#include "CRoom.h"
#include "CRoomManager.h"
#include <algorithm>
#include "../Player/CPlayer.h"
#include "../../Session/ClientSession.h"
#include "../../../CPPServerCore/ServerSupports/PrintLogThread.h"
#include "../PeerJobTimerSender/PeerJobTimerSender.h"
#include "../PeerJobTimerSender/PeerStationPackage.h"

Room::Room(unsigned short maxPlayerCount, std::string initHashID)
    : _MaxPlayerCount(maxPlayerCount), RoomHashData(initHashID)
{
}

void Room::BroadCast(PacketInterface& message)
{
    std::vector<Player*> snapshot;
    {
        std::lock_guard<std::mutex> lock(_lock);
        snapshot = playerList; // 복사 (Snapshot)
    }

    auto data = message.Serialize();
    if (data.empty()) return;

    for (Player* pl : snapshot)
    {
        if (pl && pl->_session) {
            // 락 밖에서 Send 호출 (데드락 방지 핵심)
            pl->_session->Send(message);
        }
    }
}

void Room::Ready()
{
    std::vector<Player*> snapshotPlayers;
    bool shouldRegister = false;

    {
        LOG("Room Hash: " + RoomHashData + " Game Ready 1 call");
        std::lock_guard<std::mutex> lock(_lock);
        ReadyCount++;

        if (ReadyCount >= static_cast<int>(playerList.size())) {
            LOG("Room Hash: " + RoomHashData + " Game Can Start");
            snapshotPlayers = playerList; // snapshot 복사
            shouldRegister = true;
        }
    }

    if (shouldRegister && !snapshotPlayers.empty())
    {
        LOG("Room Hash: " + RoomHashData + " Registered");
        // PeerStationPackage 생성
        auto package = std::make_shared<PeerStationPackage>();
        package->HashRoomID = RoomHashData;
        package->EnterPlayers(snapshotPlayers);

        std::vector<PeerDataPack> datas;
        datas.reserve(snapshotPlayers.size());

        for (auto p : snapshotPlayers)
        {
            if (!p || !p->_session)
                continue;

            PeerDataPack datapack;

            datapack.LocalAddress = p->_localAddress;

            // IP 변환 (string일 경우)
            char ip[INET_ADDRSTRLEN];
            inet_ntop(AF_INET, &p->_peerEndPoint.sin_addr, ip, INET_ADDRSTRLEN);
            datapack.PublicAddress = ip;

            // 포트 변환
            datapack.Port = ntohs(p->_peerEndPoint.sin_port);

            datapack.SessionID = p->_session->sessionID;

            datas.emplace_back(datapack);
        }

        package->EnterPeers(datas);

        // PeerJobTimerSender에 등록 (스레드 안전)
        PeerJobTimerSender::Instance().RegisterSender(package);
    }
}

void Room::GameStart()
{
    MAINSERVER_GameStartSucc start;
    start.timeout = 10;
    BroadCast(start);

    LOG("[Room] -> # Started Room: " + RoomHashData);
}

Room* Room::Enter(Player* player)
{
    if (player == nullptr || player->_session == nullptr) return nullptr;

    std::vector<Player*> snapshot;
    Player* currentMaster = nullptr;
    bool notifyJoin = false;

    {
        std::lock_guard<std::mutex> lock(_lock);

        // 중복 체크
        auto it = std::find(playerList.begin(), playerList.end(), player);
        if (it != playerList.end()) return nullptr;

        if (playerList.size() >= _MaxPlayerCount) return nullptr;

        notifyJoin = !playerList.empty();

        player->CurrentRoom = this;
        playerList.push_back(player);

        if (playerList.size() == 1)
            RoomMaster = player;

        snapshot = playerList;
        currentMaster = RoomMaster;
    }

    // --- Lock 밖 ---

    if (notifyJoin) {
        MAINSERVER_OtherPlayerRoomJoin join;
        join.OtherPlayer.SessionID = player->_session->sessionID;
        BroadCast(join);
    }

    MAINSERVER_JoinRoomSucc succ;
    succ.RoomHashData = RoomHashData;
    succ.MaxPlayerCount = _MaxPlayerCount;
    succ.MasterPlayer.SessionID = currentMaster->_session->sessionID;

    for (Player* pl : snapshot) {
        PlayerData pd;
        pd.SessionID = pl->_session->sessionID;
        succ.OtherPlayers.Enter(pd);
    }

    player->_session->Send(succ);
    return this;
}

bool Room::Remove(Player* player)
{
    if (player == nullptr) return false;

    std::vector<Player*> snapshot;
    bool masterChanged = false;
    Player* newMaster = nullptr;
    bool shouldDeleteRoom = false;

    {
        std::lock_guard<std::mutex> lock(_lock);

        auto it = std::find(playerList.begin(), playerList.end(), player);
        if (it == playerList.end()) return false;

        playerList.erase(it);
        player->CurrentRoom = nullptr;

        if (playerList.empty()) {
            shouldDeleteRoom = true;
        }
        else if (player == RoomMaster) {
            RoomMaster = playerList[0];
            newMaster = RoomMaster;
            masterChanged = true;
        }

        LOG("Room: " + RoomHashData + " :: Session :: " + std::to_string(player->_session->sessionID) + " :: Leave Room Call Point");

        snapshot = playerList;
    }

    // --- Lock 밖 ---

    if (shouldDeleteRoom) {
        // RoomManager의 락과 Room의 락이 꼬이지 않도록 락 밖에서 호출
        RoomManager::GetInstance().Remove(this);
        return true;
    }

    if (masterChanged && newMaster) {
        MAINSERVER_RoomMasterChanged msg;
        msg.RoomMasterData.SessionID = newMaster->_session->sessionID;
        BroadCast(msg);
    }

    MAINSERVER_OtherPlayerRoomExit exit;
    exit.LeavePlayerData.SessionID = player->_session->sessionID;
    BroadCast(exit);

    return true;
}

const int Room::GetMasterSessionID()
{
    Player* master = nullptr;

    {
        std::lock_guard<std::mutex> lock(_lock);
        master = RoomMaster;
    }

    if (master == nullptr || master->_session == nullptr)
        return -1;

    return master->_session->sessionID;
}
