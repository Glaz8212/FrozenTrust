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
    private float gameTimer = 900f; // 15�� Ÿ�̸�
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
        yield return new WaitForSeconds(1f); // ��Ʈ��ũ �غ� �ʿ��� �ð� ��¦ �ֱ�
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

            // RPC �Լ� Ÿ�̸� ����ȭ
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
            controller.enabled = false; // CharacterController ��Ȱ��ȭ
            nowPlayer.transform.position = randomPos;
            controller.enabled = true;  // CharacterController ��Ȱ��ȭ

            moveTime = Time.time;

            //NickNameEvent();
        }
    }

/*    private void NickNameEvent()
    {
        Debug.Log("�г��Ӻ����̺�Ʈ");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // �÷��̾� ���� �г��� ����
            originalNickname[i] = PhotonNetwork.PlayerList[i].NickName;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // �÷��̾� �г��� ����
            player.NickName = "???";
        }

        // ���� �ð� ���� ��� �� ����
        StartCoroutine(ReSetNickName(10f));
    }

    IEnumerator ReSetNickName(float delay)
    {
        // ������ �ð� ���� ���
        yield return new WaitForSeconds(delay);

        // �г��� ����
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PhotonNetwork.PlayerList[i].NickName = originalNickname[i];
        }
    }*/

    // Ŭ���̾�Ʈ Ÿ�̸� UI ������Ʈ
    [PunRPC]
    public void UpdateTimerUI(float time)
    {
        int min = (int)(time / 60f);
        int sce = (int)(time % 60f);
        timerText.text = $"{min:00}:{sce:00}"; // ���� �ð��� MM:SS �������� ǥ��
    }
}