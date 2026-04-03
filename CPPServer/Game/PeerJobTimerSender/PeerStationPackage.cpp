#include "PeerStationPackage.h"
#include "../Player/CPlayer.h"
#include "../../Session/ClientSession.h"


void PeerStationPackage::EnterPlayers(const std::vector<Player*>& players)
{
    std::lock_guard<std::mutex> lock(mtx);
    BroadCastList.insert(BroadCastList.end(), players.begin(), players.end());
}

void PeerStationPackage::EnterPeers(const std::vector<PeerDataPack>& peers)
{
    std::lock_guard<std::mutex> lock(mtx);
    for (const auto& peer : peers)
        SendIPEndPointContainer.push(peer);
}

void PeerStationPackage::BroadCastMessage(PacketInterface& message)
{
    std::vector<Player*> snapshot;
    {
        std::lock_guard<std::mutex> lock(mtx);
        snapshot = BroadCastList; // snapshot 복사
    }

    for (Player* pl : snapshot)
    {
        if (pl && pl->_session)
            pl->_session->Send(message); // Lock 밖에서 호출
    }
}

bool PeerStationPackage::BroadCast()
{
    PeerDataPack endPoint;
    {
        std::lock_guard<std::mutex> lock(mtx);
        if (SendIPEndPointContainer.empty())
            return true;

        endPoint = SendIPEndPointContainer.front();
        SendIPEndPointContainer.pop();
    }

    if (endPoint.PublicAddress.empty())
        return false;

    MAINSERVER_SendPeerUser peer;
    peer.PublicAddress = endPoint.PublicAddress;
    peer.LocalAddress = endPoint.LocalAddress;
    peer.SessionID = endPoint.SessionID;
    peer.Port = endPoint.Port;

    BroadCastMessage(peer);
    return false;
}