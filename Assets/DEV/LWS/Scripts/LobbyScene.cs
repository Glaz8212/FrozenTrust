using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room }

    [SerializeField] LogInPanel loginPanel;
    [SerializeField] MainPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] LobbyPanel lobbyPanel;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        UpdatePanel();
    }

    private void UpdatePanel()
    {
        if (PhotonNetwork.InRoom)
        {
            SetActivePanel(Panel.Room);
        }
        else if (PhotonNetwork.InLobby)
        {
            SetActivePanel(Panel.Lobby);
        }
        else if (PhotonNetwork.IsConnected)
        {
            SetActivePanel(Panel.Menu);
        }
        /*
        else
        {
            SetActivePanel(Panel.Login);
        }
        */
    }

    public void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject.SetActive(panel == Panel.Login);
        menuPanel.gameObject.SetActive(panel == Panel.Menu);
        roomPanel.gameObject.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
    }
}
