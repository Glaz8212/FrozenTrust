using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOADREADY = "LoadReady";

    public static void SetReady(this Player player, bool ready)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[READY] = ready;
        player.SetCustomProperties(customProperty);
    }

    public static bool GetReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(READY))
        {
            return (bool)customProperty[READY];
        }
        else
        {
            return false;
        }
    }

    // 게임 씬 로딩 이후 Ready 상태
    public static void SetInGameReady(this Player player, bool ready)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[LOADREADY] = ready;
        player.SetCustomProperties(customProperty);
    }

    public static bool GetInGameReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(LOADREADY))
        {
            return (bool)customProperty[LOADREADY];
        }
        else
        {
            return false;
        }
    }
}