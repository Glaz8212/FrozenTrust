using Photon.Pun;
using Photon.Realtime;
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
    [SerializeField] private Text traitorText;

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
            GameSceneManager.Instance.OnPlayerSpawned.AddListener(OnSpawned);
        }
    }

    // 체력 상태 업데이트
    private void LateUpdate()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        // 플레이어 허기 상태일 때 최대체력 UI업데이트
        if (playerStatus.state == PlayerStatus.PlayerState.LackHunger)
        {
            // 허기 상태
            hpBar.text = $"{playerStatus.playerHP}/{playerStatus.playerReducedHP}";
            hpBarfill.fillAmount = (float)playerStatus.playerHP / playerStatus.playerReducedHP;
        }
        else
        {
            // 일반 상태
            hpBar.text = $"{playerStatus.playerHP}/{playerStatus.playerMaxHP}";
            hpBarfill.fillAmount = (float)playerStatus.playerHP / playerStatus.playerMaxHP;
        }

        // 허기
        hungerBar.text = $"{playerStatus.hunger}/{playerStatus.hungerMax}";
        hungerBarfill.fillAmount = (float)playerStatus.hunger / playerStatus.hungerMax;

        // 온기
        heatBar.text = $"{playerStatus.warmth}/{playerStatus.warmthMax}";
        heatBarfill.fillAmount = (float)playerStatus.warmth / playerStatus.warmthMax;
    }

    public void OnSpawned()
    {
        playerStatus = GameSceneManager.Instance.nowPlayer.gameObject.GetComponent<PlayerStatus>();

        // 역할 업데이트
        if (GameManager.Instance.playerRole == 1)
        {
            myRole.text = "배신자";
            List<int> traitorIds = GameManager.Instance.GetTraitorIds();
            for (int i = 0; i < traitorIds.Count; i++)
            {
                int PlayerId = traitorIds[i];

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.ActorNumber == PlayerId)
                    {
                        string traitorName = player.NickName;
                        traitorText.text += $"배신자\n{traitorName}\n";
                    }
                }
            }
        }
        else
        {
            myRole.text = "생존자";
        }
    }
}
