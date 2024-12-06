using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebSocketSharp;

public class MainPanel : UIBinder
{
    [Header("메인 패널")]
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] Text roomNameInputField;
    [SerializeField] Text maxPlayerInputField;

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        AddEvent("CreateRoomButton", EventType.Click, CreateRoomMenu);
        AddEvent("RandomMatchingButton", EventType.Click, RandomMatching);
        AddEvent("LobbyButton", EventType.Click, JoinLobby);
        AddEvent("LogOutButton", EventType.Click, Logout);

        AddEvent("CreateRoomConfirmButton", EventType.Click, CreateRoomConfirm);
        AddEvent("CreateRoomCancleButton", EventType.Click, CreateRoomCancle);
    }

    public void CreateRoomMenu(PointerEventData eventData)
    {
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomConfirm(PointerEventData eventData)
    {
        string roomName = roomNameInputField.text;
        Debug.Log(maxPlayerInputField);
        int.TryParse(maxPlayerInputField.text, out int maxPlayer);
        Debug.Log(maxPlayer);
        maxPlayer = Mathf.Clamp(maxPlayer, 4, 8);
        
        if (roomName == " ")
        {
            Debug.Log("방 이름 x");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayer;

        createRoomPanel.SetActive(false);
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void CreateRoomCancle(PointerEventData eventData)
    {
        createRoomPanel.SetActive(false);
    }

    public void RandomMatching(PointerEventData eventData)
    {
        string name = $"Room {UnityEngine.Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions() { MaxPlayers = 8 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: options);
    }

    public void JoinLobby(PointerEventData eventData)
    {
        PhotonNetwork.JoinLobby();
    }

    public void Logout(PointerEventData eventData)
    {
        PhotonNetwork.Disconnect();
    }
}
