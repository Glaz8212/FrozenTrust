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

    // �̱��� ����
    public static GameManager Instance;

    [SerializeField] List<Player> survivors;    // Photon �÷��̾� ����Ʈ
    [SerializeField] GameState curState;  // ���� ����
    [SerializeField] int traitorCount;        // ����� ��
    [SerializeField] int survivorCount;       // ������ ��
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
            Debug.Log("�ݺ��� ����");
            int rand = Random.Range(0, playerId.Count);
            traitor.Add(playerId[rand]);
            playerId.RemoveAt(rand);
            Debug.Log($"����ڴ� : {traitor[0]}");
        }

        // ������ �÷��̾ �����ڷ� ����
        foreach (int players in playerId)
        {
            survivor.Add(players);
        }

        // ��� Ŭ���̾�Ʈ�� ���� ����ȭ
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int role;

            if (traitor.Contains(player.ActorNumber))
            {
                role = 1; // �����
            }
            else
            {
                role = 0; // ������
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
            Debug.Log($"�� ������ {(playerRole == 1 ? "�����" : "������")}�Դϴ�.");
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
        // �迡 �ö����� �� ȣ�� �Լ�
        if (checkwin == true)// ������ �¸�����
        {
            GameStateChange(GameState.End);
            // TODO : UI �¸� �ڷ�ƾ �߰�
            Debug.Log("�¸�");
            StartCoroutine(SurvivorEndingPanel());
        }
        else if (checkwin == false)// ����� �¸�����
        {
            GameStateChange(GameState.End);
            Debug.Log("�й�");
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