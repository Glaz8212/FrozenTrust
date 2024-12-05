using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] LobbyScene lobbyScene;
    [SerializeField] LobbyPanel lobbyPanel; // �� ����Ʈ ���� UI
    [SerializeField] RoomPanel roomPanel;  // �� ���� UI

    public override void OnConnectedToMaster()
    {
        Debug.Log("���ӿ� �����ߴ�!");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"������ �����. cause : {cause}");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Login);
    }

    // �� ���� �� ȣ��
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ����");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Room);
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
    public override void OnPlayerPropertiesUpdate  (Player targetPlayer, Hashtable changedProps)
    {
        roomPanel.UpdatePlayerProperty(targetPlayer, changedProps); // UI ����
    }

    public override void OnLeftRoom()
    {
        lobbyScene.SetActivePanel(LobbyScene.Panel.Menu);
    }

    // �κ� ����
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� ����");
        lobbyScene.SetActivePanel(LobbyScene.Panel.Lobby);
    }
    public override void OnLeftLobby()
    {
        Debug.Log("�κ� ���� ����");
        lobbyPanel.ClearRoomEntries();
        lobbyScene.SetActivePanel(LobbyScene.Panel.Menu);
    }

    // �� ����Ʈ ������Ʈ
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("�� ����Ʈ ������Ʈ");
        lobbyPanel.UpdateRoomList(roomList); // UI ����
    }
}