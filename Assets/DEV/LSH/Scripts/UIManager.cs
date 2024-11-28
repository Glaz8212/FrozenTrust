using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text hpBar;
    [SerializeField] private Text hungerBar;
    [SerializeField] private Text heatBar;
    [SerializeField] private Text myRole;

    [SerializeField] private Image hpBarfill;
    [SerializeField] private Image hungerBarfill;
    [SerializeField] private Image heatBarfill;

    [SerializeField] PlayerStatus playerStatus;

    private void OnDisable()
    {
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.OnPlayerSpawned.RemoveListener(OnSpawned);
        }
    }

    private void Start()
    {
        if (GameSceneManager.Instance != null)
        {
            Debug.Log("없음");
            GameSceneManager.Instance.OnPlayerSpawned.AddListener(OnSpawned);

        }
        // 역할 업데이트
        if (GameManager.Instance.playerRole == 1)
        {
            myRole.text = "배신자";
        }
        else
        {
            myRole.text = "생존자";
        }
    }

    // 체력 상태 업데이트
    private void LateUpdate()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        hpBar.text = $"{playerStatus.playerHP}/{playerStatus.playerMaxHP}";
        hpBarfill.fillAmount = (float)playerStatus.playerHP / playerStatus.playerMaxHP;

        hungerBar.text = $"{playerStatus.hunger}/{playerStatus.hungerMax}";
        hungerBarfill.fillAmount = (float)playerStatus.hunger / playerStatus.hungerMax;

        heatBar.text = $"{playerStatus.warmth}/{playerStatus.warmthMax}";
        heatBarfill.fillAmount = (float)playerStatus.warmth / playerStatus.warmthMax;
    }

    public void OnSpawned()
    {
        Debug.Log("액션실행");
        playerStatus = GameSceneManager.Instance.nowPlayer.gameObject.GetComponent<PlayerStatus>();
    }
}
