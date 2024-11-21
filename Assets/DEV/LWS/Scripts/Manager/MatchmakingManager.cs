using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    /*

    [SerializeField] LobbyPanel lobbyPanel; // �� ����Ʈ ���� UI
    [SerializeField] RoomPanel roomPanel;  // �� ���� UI

    // �� ����
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    // ���� ��Ī
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // �� ���� �� ȣ��
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ����");
        roomPanel.UpdateRoomUI(); // �� ���� UI ����
    }

    // �濡�� �÷��̾� ���� ó��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} ����!");
        roomPanel.EnterPlayer(newPlayer); // UI ����
    }

    // �濡�� �÷��̾� ���� ó��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} ����!");
        roomPanel.ExitPlayer(otherPlayer); // UI ����
    }

    // �÷��̾� Ŀ���� ������Ƽ ������Ʈ
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        roomPanel.UpdatePlayerProperty(targetPlayer, changedProps); // UI ����
    }

    // �κ� ����
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� ����");
        lobbyPanel.ClearRoomEntries();
    }
    public override void OnLeftLobby()
    {
        Debug.Log("�κ� ���� ����");
        lobbyPanel.ClearRoomEntries();
    }

    // �� ����Ʈ ������Ʈ
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("�� ����Ʈ ������Ʈ");
        lobbyPanel.UpdateRoomList(roomList); // UI ����
    }

    // ���� ��Ī ���� �� �� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"���� ��Ī ����: {message}");
        CreateRoom($"Room_{Random.Range(1, 1000)}");
    }

    // �κ� ����
    */
}