using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public enum GameState
    {
        Idle,
        InGame,
        End
    }

    // �̱��� ����
    public static GameManager Instance;

    [SerializeField] List<Player> players;    // Photon �÷��̾� ����Ʈ
    [SerializeField] GameState curState;  // ���� ����
    [SerializeField] int enemyCount;        // ����� ��
    [SerializeField] int playerCount;       // ������ ��
    [SerializeField] List<int> player;
    [SerializeField] List<int> enemy;


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
        PlayerEnemyReRoll();
    }

    public void PlayerEnemyReRoll()
    {
        int palyerList = PhotonNetwork.PlayerList.Length;

        // ����ڴ� 4��� 1�� �ּ� �ο� 4��
        enemyCount = Mathf.Max(1, playerCount / 4);
        playerCount = playerCount - enemyCount;

        List<int> playerId = new List<int>();

        // �÷��̾� ID ����
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerId.Add(player.ActorNumber);
        }

        // �������� ����� ���� ����
        for (int i = 0; i < enemyCount; i++)
        {
            int rand = Random.Range(0, playerCount);
            enemy.Add(rand);
            playerId.RemoveAt(rand);
        }

        // ������ �÷��̾ �����ڷ� ����
        foreach (int players in playerId)
        {
            player.Add(players);
        }
    }

    public void CheckWin()
    {
        if (enemyCount <= 0)// ������ �¸�����
        {
            GameStateChange(GameState.End);

        }
        else if (playerCount <= enemyCount)// ����� �¸�����
        {
            GameStateChange(GameState.End);
        }
    }

    public void GameStateChange(GameState state)
    {
        curState = state;

        // ���¸� ����ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StateChanged), RpcTarget.All, state);
        }
    }

    [PunRPC]
    private void StateChanged(GameState newState)
    {
        curState = newState; // ���ο� ���·� ������Ʈ

        // ���¿� ���� ���� ����
        if (newState == GameState.InGame)
        {
            Debug.Log("Game Started");
        }
        else if (newState == GameState.End)
        {
            Debug.Log("Game Ended");
        }
    }
}