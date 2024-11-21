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

    // 싱글톤 생성
    public static GameManager Instance;

    public List<Player> players;    // Photon 플레이어 리스트
    public GameState curState;  // 게임 상태
    public int enemyCount;        // 배신자 수
    public int playerCount;       // 생존자 수
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

        // 배신자는 4명당 1명 최소 인원 4명
        enemyCount = Mathf.Max(1, playerCount / 4);
        playerCount = playerCount - enemyCount;

        List<int> playerId = new List<int>();

        // 플레이어 ID 저장
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerId.Add(player.ActorNumber);
        }

        // 랜덤으로 배신자 역할 배정
        for (int i = 0; i < enemyCount; i++)
        {
            int rand = Random.Range(0, playerCount);
            enemy.Add(rand);
            playerId.RemoveAt(rand);
        }

        // 나머지 플레이어를 생존자로 설정
        foreach (int players in playerId)
        {
            player.Add(players);
        }
    }
}