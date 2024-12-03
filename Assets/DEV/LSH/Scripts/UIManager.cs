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
    [SerializeField] private Text missionText;

    [SerializeField] private Image hpBarfill;
    [SerializeField] private Image hungerBarfill;
    [SerializeField] private Image heatBarfill;

    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] MissionController missionController;

    [SerializeField] GameObject traitorTeam;

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
        MissionUI();
    }

    private void UIUpdate()
    {
        if (playerStatus == null)
            return;

        // 플레이어 허기 상태일 때 최대체력 UI업데이트
        if (playerStatus.state == PlayerStatus.PlayerState.LackHunger || playerStatus.state == PlayerStatus.PlayerState.LackEverything)
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
        missionController = GameSceneManager.Instance.missionController.gameObject.GetComponent<MissionController>();

        // 역할 업데이트
        if (GameManager.Instance.playerRole == 1)
        {
            myRole.text = "배신자";

            List<int> traitorIds = GameManager.Instance.GetTraitorIds();
            if(traitorIds.Count >= 2)
            {
                traitorTeam.SetActive(true);
                traitorText.text += $"배신자\n";
                for (int i = 0; i < traitorIds.Count; i++)
                {
                    int PlayerId = traitorIds[i];
                    traitorText.text += $"배신자\n";
                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if (player.ActorNumber == PlayerId)
                        {
                            string traitorName = player.NickName;
                            traitorText.text += $"{traitorName}\n";
                        }
                    }
                }
            }
            
        }
        else
        {
            myRole.text = "생존자";
        }
    }

    private void MissionUI()
    {
        if (missionController == null)
            return;

        // 미션 상태를 확인하여 텍스트 업데이트
        if (missionController.IsEndingClear)
        {
            missionText.text = "엔딩 클리어!";
        }
        else if (missionController.Is2Clear)
        {
            if(GameManager.Instance.playerRole == 0)
            {
                missionText.text = "미션 2 클리어 완료!\n";
                missionText.text += "이제 탈출만이 남았습니다. 탈출 장소로 향하십시오.";
            }
            else
            {
                missionText.text = "생존자가 모든 미션을 클리어했습니다.\n";
                missionText.text += "그들이 탈출하지 못하게 막으십시오.";
            }
            
        }
        else if (missionController.Is1Clear)
        {
            if (GameManager.Instance.playerRole == 0)
            {
                missionText.text = "미션1 클리어 완료!\n";
                missionText.text += "미션2 오브젝트를 찾아서 수행하시오.!";
            }
            else
            {
                missionText.text = "생존자가 미션1을 클리어 했습니다!\n";
                missionText.text += "미션2를 해결하지 못하게 방해하십시오.";
            }
        }
        else
        {
            if (GameManager.Instance.playerRole == 0)
                missionText.text = "미션 박스를 찾으시오.";
            else
                missionText.text = "생존자의 탈출을 방해하시오.";
        }
    }
}
