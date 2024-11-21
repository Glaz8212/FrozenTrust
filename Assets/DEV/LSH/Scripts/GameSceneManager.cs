using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPun
{
    public static GameSceneManager Instance;

    public float gameTimer = 900f; // 15분 타이머
    public TMP_Text timerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
