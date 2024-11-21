using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    /*

    [SerializeField] LobbyPanel lobbyPanel; // 방 리스트 관리 UI
    [SerializeField] RoomPanel roomPanel;  // 방 내부 UI

    // 방 생성
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    // 랜덤 매칭
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // 방 입장 시 호출
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공");
        roomPanel.UpdateRoomUI(); // 방 내부 UI 갱신
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
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        roomPanel.UpdatePlayerProperty(targetPlayer, changedProps); // UI 갱신
    }

    // 로비 입장
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 성공");
        lobbyPanel.ClearRoomEntries();
    }
    public override void OnLeftLobby()
    {
        Debug.Log("로비 퇴장 성공");
        lobbyPanel.ClearRoomEntries();
    }

    // 방 리스트 업데이트
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("방 리스트 업데이트");
        lobbyPanel.UpdateRoomList(roomList); // UI 갱신
    }

    // 랜덤 매칭 실패 시 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"랜덤 매칭 실패: {message}");
        CreateRoom($"Room_{Random.Range(1, 1000)}");
    }

    // 로비 퇴장
    */
}