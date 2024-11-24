using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomPanel : UIBinder
{
    [Header("룸 패널")]
    [SerializeField] PlayerEntry[] playerEntries;
    [SerializeField] Button startButton;

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        AddEvent("StartButton", EventType.Click, StartGame);
        AddEvent("LeaveButton", EventType.Click, LeaveRoom);
    }

    private void OnEnable()
    {
        UpdatePlayers();

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        PhotonNetwork.LocalPlayer.SetReady(false);
    }

    public void UpdatePlayers()
    {
        foreach (PlayerEntry entry in playerEntries)
        {
            entry.SetEmpty();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            playerEntries[number].SetPlayer(player);
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startButton.interactable = CheckAllReady();
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void EnterPlayer(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장!");
        UpdatePlayers();
    }

    public void ExitPlayer(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} 퇴장!");
        UpdatePlayers();
    }

    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        // 레디 커스텀 프로퍼티를 변경한 경우면 READY 키가 있음
        if (properties.ContainsKey(CustomProperty.READY))
        {
            UpdatePlayers();
        }
    }

    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady() == false)
                return false;
        }

        return true;
    }

    public void StartGame(PointerEventData eventData)
    {
        PhotonNetwork.LoadLevel("GameScene");
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void LeaveRoom(PointerEventData eventData)
    {
        PhotonNetwork.LeaveRoom();
    }
}
