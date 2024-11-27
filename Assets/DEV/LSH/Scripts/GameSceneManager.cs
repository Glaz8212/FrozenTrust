using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPun
{
    public static GameSceneManager Instance;
    public MissionController missionController;

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

        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f); // 네트워크 준비에 필요한 시간 살짝 주기
        PlayerSpawn();
    }

    private void PlayerSpawn()
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            PhotonNetwork.Instantiate("JHS/Player01", randomPos, Quaternion.identity);
        }

    private void Start()
    {
        missionController = GetComponent<MissionController>();
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameTimerCoroutine());
        }
    }

    private void Update()
    {
        if (missionController.IsEndingClear == true)
        {
            GameManager.Instance.CheckWin(true);
        }

        if (false)// player = deathplayer
        {
            GameManager.Instance.CheckWin(false);
        }
    }

    private IEnumerator GameTimerCoroutine()
    {
        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;

            // RPC 함수 타이머 동기화
            photonView.RPC(nameof(UpdateTimerUI), RpcTarget.All, gameTimer);

            yield return null;
        }
    }


    // 클라이언트 타이머 UI 업데이트
    [PunRPC]
    public void UpdateTimerUI(float time)
    {
        int min = (int)(time / 60f);
        int sce = (int)(time % 60f);
        timerText.text = $"{min:00}:{sce:00}"; // 남은 시간을 MM:SS 형식으로 표시
    }
}