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

    [SerializeField] List<Player> survivors;    // Photon �÷��̾� ����Ʈ
    [SerializeField] GameState curState;  // ���� ����
    [SerializeField] int traitorCount;        // ����� ��
    [SerializeField] int survivorCount;       // ������ ��
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
        
        // ����ڴ� 4��� 1�� �ּ� �ο� 4��
        traitorCount = Mathf.Max(1, playerList / 4);
        survivorCount = playerList - traitorCount;
        
        List<int> playerId = new List<int>();

        // �÷��̾� ID ����
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerId.Add(player.ActorNumber);
        }

        // �������� ����� ���� ����
        for (int i = 0; i < traitorCount; i++)
        {
            int rand = Random.Range(0, survivorCount);
            traitor.Add(playerId[rand]);
            playerId.RemoveAt(rand);
        }

        // ������ �÷��̾ �����ڷ� ����
        foreach (int players in playerId)
        {
            survivor.Add(players);
        }
    }*/

    public void CheckWin(bool checkwin)
    {
        // �迡 �ö����� �� ȣ�� �Լ�
        if (checkwin == true)// ������ �¸�����
        {
            //GameStateChange(GameState.End);
            Debug.Log("�¸�");
        }
        else if (checkwin == false)// ����� �¸�����
        {
            //GameStateChange(GameState.End);
            Debug.Log("�й�");
        }
    }

    /*public void GameStateChange(GameState state)
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
    }*/
}