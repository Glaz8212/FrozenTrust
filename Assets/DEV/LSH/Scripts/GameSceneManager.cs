using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameSceneManager : MonoBehaviourPun
{
    public static GameSceneManager Instance;
    public MissionController missionController;
    public GameObject nowPlayer;
    [SerializeField] PlayerStatus playerStatus;

    private float moveTime = -10f;
    private float teleportCooldown = 10f;
    private float gameTimer = 900f; // 15분 타이머
    public TMP_Text timerText;
    public string[] originalNickname;

    public UnityEvent OnPlayerSpawned = new UnityEvent();

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
        nowPlayer = PhotonNetwork.Instantiate("JHS/Player01", randomPos, Quaternion.identity);
        playerStatus = nowPlayer.gameObject.GetComponent<PlayerStatus>();
        OnPlayerSpawned?.Invoke();        
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameTimerCoroutine());
        }
    }

    private IEnumerator GameTimerCoroutine()
    {
        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;

            // RPC 함수 타이머 동기화
            photonView.RPC(nameof(UpdateTimerUI), RpcTarget.All, gameTimer);

            TeleportEvent();


            yield return null;
        }

        GameManager.Instance.CheckWin(false);
    }

    private void EventManager(int eventNum)
    {
        if(eventNum == 1)
        {
            TeleportEvent();
        }
        else
        {
            //NickNameEvent();
        }        
    }

    private void TeleportEvent()
    {
        if (gameTimer <= 880 && gameTimer >= 879 && (Time.time - moveTime >= teleportCooldown) && playerStatus.environment != PlayerStatus.SurroundingEnvironment.Warm)
        {
            Vector3 randomPos = new Vector3(Random.Range(-10f, 10f), 3, Random.Range(-10f, 10f));
            CharacterController controller = nowPlayer.GetComponent<CharacterController>();
            controller.enabled = false; // CharacterController 비활성화
            nowPlayer.transform.position = randomPos;
            controller.enabled = true;  // CharacterController 재활성화

            moveTime = Time.time;

            //NickNameEvent();
        }
    }

/*    private void NickNameEvent()
    {
        Debug.Log("닉네임변경이벤트");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // 플레이어 원래 닉네임 저장
            originalNickname[i] = PhotonNetwork.PlayerList[i].NickName;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 플레이어 닉네임 변경
            player.NickName = "???";
        }

        // 지정 시간 동안 대기 후 복구
        StartCoroutine(ReSetNickName(10f));
    }

    IEnumerator ReSetNickName(float delay)
    {
        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(delay);

        // 닉네임 복구
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PhotonNetwork.PlayerList[i].NickName = originalNickname[i];
        }
    }*/

    // 클라이언트 타이머 UI 업데이트
    [PunRPC]
    public void UpdateTimerUI(float time)
    {
        int min = (int)(time / 60f);
        int sce = (int)(time % 60f);
        timerText.text = $"{min:00}:{sce:00}"; // 남은 시간을 MM:SS 형식으로 표시
    }
}