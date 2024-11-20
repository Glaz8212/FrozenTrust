using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviourPunCallbacks
{
    public enum GameState
    {
        Idle,
        InGame,
        End
    }

    // 싱글톤 생성
    public static GamaManager Instance; 

    public List<Player> players;    // Photon 플레이어 리스트
    public GameState curState;  // 게임 상태
    public int enemyCount;        // 배신자 수
    public int playerCount;       // 생존자 수

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

    public void CheckWin()
    {
        if (true)// 생존자 승리조건
        {
            // TODO 
        }
        else if (true)// 배신자 승리조건
        {
            // TODO 
        }
    }

    public void PlayerEnemyReRoll()
    {
        // TODO 역할 분배 로직 작성
    }

    public void GameStateChange()
    {
        // TODO 게임 상태 변경 로직
    }
}
