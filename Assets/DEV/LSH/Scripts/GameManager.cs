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

    // �̱��� ����
    public static GamaManager Instance; 

    public List<Player> players;    // Photon �÷��̾� ����Ʈ
    public GameState curState;  // ���� ����
    public int enemyCount;        // ����� ��
    public int playerCount;       // ������ ��

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
        if (true)// ������ �¸�����
        {
            // TODO 
        }
        else if (true)// ����� �¸�����
        {
            // TODO 
        }
    }

    public void PlayerEnemyReRoll()
    {
        // TODO ���� �й� ���� �ۼ�
    }

    public void GameStateChange()
    {
        // TODO ���� ���� ���� ����
    }
}
