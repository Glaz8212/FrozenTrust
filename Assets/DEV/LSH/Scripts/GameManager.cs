using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Tilemaps.Tilemap;

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
    public int playerRole;

    [SerializeField] GameObject survivorEnding;
    [SerializeField] GameObject traitorEnding;

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
            Debug.Log("반복문 실행");
            int rand = Random.Range(0, playerId.Count);
            traitor.Add(playerId[rand]);
            playerId.RemoveAt(rand);
            Debug.Log($"배신자는 : {traitor[0]}");
        }

        // 나머지 플레이어를 생존자로 설정
        foreach (int players in playerId)
        {
            survivor.Add(players);
        }

        // 모든 클라이언트에 역할 동기화
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int role;

            if (traitor.Contains(player.ActorNumber))
            {
                role = 1; // 배신자
            }
            else
            {
                role = 0; // 생존자
            }

            
            photonView.RPC(nameof(SynchRoles), RpcTarget.All, role, player.ActorNumber);
            
        }
        photonView.RPC(nameof(Synchtraitor), RpcTarget.All, traitor.ToArray());
    }

    [PunRPC]
    private void SynchRoles(int role, int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            playerRole = role;
            Debug.Log($"내 역할은 {(playerRole == 1 ? "배신자" : "생존자")}입니다.");
        }
    }

    [PunRPC]
    private void Synchtraitor(int[] traitorIds)
    {
        traitor = new List<int>(traitorIds);
    }

    public List<int> GetTraitorIds()
    {
        return traitor;
    }

    public void CheckWin(bool checkwin)
    {
        StopAllCoroutines();
        // 배에 올라탔을 때 호출 함수
        if (checkwin == true)// 생존자 승리조건
        {
            GameStateChange(GameState.End);
            // TODO : UI 승리 코루틴 추가
            Debug.Log("승리");
            StartCoroutine(SurvivorEndingPanel());
        }
        else if (checkwin == false)// 배신자 승리조건
        {
            GameStateChange(GameState.End);
            Debug.Log("패배");
            StartCoroutine(TraitorEndingPanel());
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
    private IEnumerator SurvivorEndingPanel()
    {
        survivorEnding.gameObject.SetActive(true);
        yield return new WaitForSeconds(20f);
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }
    private IEnumerator TraitorEndingPanel()
    {
        traitorEnding.gameObject.SetActive(true);
        yield return new WaitForSeconds(20f);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }

    public void GameStateChange(GameState state)
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
    }
}