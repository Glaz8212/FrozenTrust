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

    [SerializeField] List<Player> survivors;    // Photon 플레이어 리스트
    [SerializeField] GameState curState;  // 게임 상태
    [SerializeField] int traitorCount;        // 배신자 수
    [SerializeField] int survivorCount;       // 생존자 수
    [SerializeField] List<int> survivor;
    [SerializeField] List<int> traitor;

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

    /*private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerEnemyReRoll();
        }
    }

    public void PlayerEnemyReRoll()
    {
        int playerList = PhotonNetwork.PlayerList.Length;
        
        // 배신자는 4명당 1명 최소 인원 4명
        traitorCount = Mathf.Max(1, playerList / 4);
        survivorCount = playerList - traitorCount;
        
        List<int> playerId = new List<int>();

        // 플레이어 ID 저장
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerId.Add(player.ActorNumber);
        }

        // 랜덤으로 배신자 역할 배정
        for (int i = 0; i < traitorCount; i++)
        {
            int rand = Random.Range(0, survivorCount);
            traitor.Add(playerId[rand]);
            playerId.RemoveAt(rand);
        }

        // 나머지 플레이어를 생존자로 설정
        foreach (int players in playerId)
        {
            survivor.Add(players);
        }
    }*/

    public void CheckWin(bool checkwin)
    {
        // 배에 올라탔을 때 호출 함수
        if (checkwin == true)// 생존자 승리조건
        {
            //GameStateChange(GameState.End);
            Debug.Log("승리");
        }
        else if (checkwin == false)// 배신자 승리조건
        {
            //GameStateChange(GameState.End);
            Debug.Log("패배");
        }
    }

    /*public void GameStateChange(GameState state)
    {
        curState = state;

        // 상태를 동기화
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StateChanged), RpcTarget.All, state);
        }
    }

    [PunRPC]
    private void StateChanged(GameState newState)
    {
        curState = newState; // 새로운 상태로 업데이트

        // 상태에 따라 동작 결정
        if (newState == GameState.InGame)
        {
            Debug.Log("Game Started");
        }
        else if (newState == GameState.End)
        {
            Debug.Log("Game Ended");
        }
    }*/
}