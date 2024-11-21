using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPun
{
    public static GameSceneManager Instance;

    public float gameTimer = 900f; // 15�� Ÿ�̸�
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

    private IEnumerator GameTimerCoroutine()
    {
        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;

            // RPC �Լ� Ÿ�̸� ����ȭ
            photonView.RPC(nameof(UpdateTimerUI), RpcTarget.All, gameTimer);

            yield return null;
        }
    }


    // Ŭ���̾�Ʈ Ÿ�̸� UI ������Ʈ
    [PunRPC]
    public void UpdateTimerUI(float time)
    {
        int min = (int)(time / 60f);
        int sce = (int)(time % 60f);
        timerText.text = $"{min:00}:{sce:00}"; // ���� �ð��� MM:SS �������� ǥ��
    }
}