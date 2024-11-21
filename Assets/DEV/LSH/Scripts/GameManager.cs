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

    public List<Player> players;    // Photon �÷��̾� ����Ʈ
    public GameState curState;  // ���� ����
    public int enemyCount;        // ����� ��
    public int playerCount;       // ������ ��
    public List<int> player;
    public List<int> enemy;


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
}