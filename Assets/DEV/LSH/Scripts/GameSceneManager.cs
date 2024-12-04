using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameSceneManager : MonoBehaviourPun
{
    public static GameSceneManager Instance;
    [SerializeField] public MissionController missionController;
    [SerializeField] public GameObject nowPlayer;
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] List<NamePrint> namePrints = new List<NamePrint>();

    private float moveTime = -10f;
    private float teleportCooldown = 10f;
    private float gameTimer = 900f; // 15�� Ÿ�̸�
    public TMP_Text timerText;

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
    }

    private void Start()
    {
         StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f); // ��Ʈ��ũ �غ� �ʿ��� �ð� ��¦ �ֱ�
        PhotonNetwork.LocalPlayer.SetInGameReady(true); // "InGameReady" ���� true ����
        Debug.Log("�÷��̾� InGameReady ����");

        StartCoroutine(WaitForAllPlayersReady());
    }

    private IEnumerator WaitForAllPlayersReady()
    {
        Debug.Log("��� �÷��̾� �غ� ���� Ȯ�� ��...");

        while (!AllPlayersReady())
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("��� �÷��̾� �غ� �Ϸ�. ���� ����");

        PlayerSpawn();

        // ������ Ŭ���̾�Ʈ�� Ÿ�̸� ����
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameTimerCoroutine());
        }
    }

    private bool AllPlayersReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.GetInGameReady())
            {
                return false; // ���� �غ���� ���� �÷��̾ ����
            }
        }
        return true; // ��� �÷��̾ �غ��
    }


    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(-51f + Random.Range(-5f, 5f), 8f, -6.5f + Random.Range(-5f, 5f));
        nowPlayer = PhotonNetwork.Instantiate("JHS/Player01", randomPos, Quaternion.identity);
        playerStatus = nowPlayer.gameObject.GetComponent<PlayerStatus>();
        OnPlayerSpawned?.Invoke();
    }


    private IEnumerator GameTimerCoroutine()
    {
        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;

            // RPC �Լ� Ÿ�̸� ����ȭ
            photonView.RPC(nameof(UpdateTimerUI), RpcTarget.All, gameTimer);

            if (gameTimer <= 880 && gameTimer >= 879 && (Time.time - moveTime >= teleportCooldown))
            {
                EventManager(Random.Range(1, 3));
            }

            yield return null;
        }

        GameManager.Instance.CheckWin(false);
    }

    private void EventManager(float eventNum)
    {
        if (eventNum == 1)
        {
            photonView.RPC(nameof(TeleportEvent), RpcTarget.All);
        }
        else
        {
            photonView.RPC(nameof(NickNameEvent), RpcTarget.All);
        }
    }

    [PunRPC]
    private void TeleportEvent()
    {
        if ((Time.time - moveTime >= teleportCooldown) && playerStatus.environment != PlayerStatus.SurroundingEnvironment.Warm)
        {
            Vector3 randomPos = new Vector3(Random.Range(-10f, 10f), 3, Random.Range(-10f, 10f));
            CharacterController controller = nowPlayer.GetComponent<CharacterController>();

            controller.enabled = false; // CharacterController ��Ȱ��ȭ
            nowPlayer.transform.position = randomPos;
            controller.enabled = true;  // CharacterController ��Ȱ��ȭ

            moveTime = Time.time;

            photonView.RPC(nameof(NickNameEvent), RpcTarget.All);
        }
    }

    [PunRPC]
    private void NickNameEvent()
    {
        FindNameUI();

        foreach (NamePrint name in namePrints)
        {
                // �г��� ��Ȱ��ȭ
                name.gameObject.SetActive(false);
        }

        // ���� �ð� ���� ��� �� ����
        StartCoroutine(ReSetNickName(30f));
    }

    IEnumerator ReSetNickName(float delay)
    {        
        // ������ �ð� ���� ���
        yield return new WaitForSeconds(delay);

        foreach (NamePrint name in namePrints)
        {
            Debug.Log("for����");

            // �г��� Ȱ��ȭ
            name.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        namePrints.Clear();
    }

    private void FindNameUI()
    {
        namePrints.AddRange(FindObjectsOfType<NamePrint>());
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