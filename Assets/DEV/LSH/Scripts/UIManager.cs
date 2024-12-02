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

    // ü�� ���� ������Ʈ
    private void LateUpdate()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        // �÷��̾� ��� ������ �� �ִ�ü�� UI������Ʈ
        if (playerStatus.state == PlayerStatus.PlayerState.LackHunger)
        {
            // ��� ����
            hpBar.text = $"{playerStatus.playerHP}/{playerStatus.playerReducedHP}";
            hpBarfill.fillAmount = (float)playerStatus.playerHP / playerStatus.playerReducedHP;
        }
        else
        {
            // �Ϲ� ����
            hpBar.text = $"{playerStatus.playerHP}/{playerStatus.playerMaxHP}";
            hpBarfill.fillAmount = (float)playerStatus.playerHP / playerStatus.playerMaxHP;
        }

        // ���
        hungerBar.text = $"{playerStatus.hunger}/{playerStatus.hungerMax}";
        hungerBarfill.fillAmount = (float)playerStatus.hunger / playerStatus.hungerMax;

        // �±�
        heatBar.text = $"{playerStatus.warmth}/{playerStatus.warmthMax}";
        heatBarfill.fillAmount = (float)playerStatus.warmth / playerStatus.warmthMax;
    }

    public void OnSpawned()
    {
        playerStatus = GameSceneManager.Instance.nowPlayer.gameObject.GetComponent<PlayerStatus>();
        missionController = gameObject.GetComponent<MissionController>();

        // ���� ������Ʈ
        if (GameManager.Instance.playerRole == 1)
        {
            myRole.text = "�����";
            List<int> traitorIds = GameManager.Instance.GetTraitorIds();
            for (int i = 0; i < traitorIds.Count; i++)
            {
                int PlayerId = traitorIds[i];
                traitorText.text += $"�����\n";
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
        else
        {
            myRole.text = "������";
        }
    }

    private void MissionUI()
    {
        if (missionController == null)
            return;

        // �̼� ���¸� Ȯ���Ͽ� �ؽ�Ʈ ������Ʈ
        if (missionController.IsEndingClear)
        {
            missionText.text = "���� Ŭ����!";
        }
        else if (missionController.Is2Clear)
        {
            missionText.text = "�̼� 2 Ŭ���� �Ϸ�!";
        }
        else if (missionController.Is1Clear)
        {
            if (GameManager.Instance.playerRole == 0)
            {
                missionText.text = "�̼�1 Ŭ���� �Ϸ�!\n";
                missionText.text = "�̼�2 ������Ʈ�� ã�Ƽ� �����Ͻÿ�.!";
            }
        }
        else
        {
            if (GameManager.Instance.playerRole == 0)
                missionText.text = "�̼� �ڽ��� ã���ÿ�.";
            else
                missionText.text = "�������� Ż���� �����Ͻÿ�.";
        }
    }
}
