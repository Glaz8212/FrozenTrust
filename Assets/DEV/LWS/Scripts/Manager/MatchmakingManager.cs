using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] LobbyScene lobbyScene;
    [SerializeField] LobbyPanel lobbyPanel; // 방 리스트 관리 UI
    [SerializeField] RoomPanel roomPanel;  // 방 내부 UI

    public override void OnConnectedToMaster()
    {
        Debug.Log("접속에 성공했다!");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"접속이 끊겼다. cause : {cause}");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Login);
    }

    // 방 입장 시 호출
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Room);
    }

    // 방에서 플레이어 입장 처리
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장!");
        roomPanel.EnterPlayer(newPlayer); // UI 갱신
    }

    // 방에서 플레이어 퇴장 처리
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} 퇴장!");
        roomPanel.ExitPlayer(otherPlayer); // UI 갱신
    }

    // 플레이어 커스텀 프로퍼티 업데이트
    public override void OnPlayerPropertiesUpdate  (Player targetPlayer, Hashtable changedProps)
    {
        roomPanel.UpdatePlayerProperty(targetPlayer, changedProps); // UI 갱신
    }

    public override void OnLeftRoom()
    {
        lobbyScene.SetActivePanel(LobbyScene.Panel.Menu);
    }

    // 로비 입장
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 성공");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Lobby);
    }
    public override void OnLeftLobby()
    {
        Debug.Log("로비 퇴장 성공");
        lobbyPanel.ClearRoomEntries();
        lobbyScene.SetActivePanel(LobbyScene.Panel.Menu);
    }

    // 방 리스트 업데이트
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("방 리스트 업데이트");
        lobbyPanel.UpdateRoomList(roomList); // UI 갱신
    }
}