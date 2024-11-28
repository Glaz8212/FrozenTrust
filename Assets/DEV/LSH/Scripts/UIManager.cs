using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviourPun
{
    /*public static UIManager Instance;

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
    }*/

    [SerializeField] private Text hpBar;
    [SerializeField] private Text hungerBar;
    [SerializeField] private Text heatBar;
    [SerializeField] private Text myRole;

    [SerializeField] private Image hpBarfill;
    [SerializeField] private Image hungerBarfill;
    [SerializeField] private Image heatBarfill;

    [SerializeField] PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = GameSceneManager.Instance.nowPlayer.GetComponent<PlayerStatus>();

        // 역할 업데이트
        myRole.text = GameManager.Instance.playerRole == 1 ? "배신자" : "생존자";
       

    }

    // 체력 상태 업데이트
    private void LateUpdate()
    {
        hpBar.text = $"{playerStatus.playerHP}/{playerStatus.playerMaxHP}";
        hpBarfill.fillAmount = playerStatus.playerHP/playerStatus.playerMaxHP;
        
        hungerBar.text = $"{playerStatus.hunger}/{playerStatus.hungerMax}";
        hungerBarfill.fillAmount = playerStatus.hunger / playerStatus.hungerMax;

        heatBar.text = $"{playerStatus.warmth}/{playerStatus.warmthMax}";
        heatBarfill.fillAmount = playerStatus.warmth / playerStatus.warmthMax;

    }
}
