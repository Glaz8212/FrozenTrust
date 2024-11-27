using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviourPunCallbacks
{
    // public Camera cam;
    public CinemachineVirtualCamera cinemachineVirtualCamera; // �ó׸ӽ� ī�޶�
    [SerializeField] TestGameScene testGameScene;
    // List<GameObject> playerList = new List<GameObject>();

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        /* Player = photonView.gameObject.transform;
        cinemachineVirtualCamera.Follow = Player;
        cinemachineVirtualCamera.LookAt = Player;
        */
    }
    /// <summary>
    /// �������ڸ��� �ٷ� �Ǵ� ���, ����� �ȵ� �� ������ �ణ�� �����̸� �ֱ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(3f);
        SetCam();
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    private void SetCam()
    {
        cinemachineVirtualCamera.Follow = testGameScene.nowPlayer.transform;
        cinemachineVirtualCamera.LookAt = testGameScene.nowPlayer.transform;

    }
    /* ���� �� PhotonNetwork���� PlayerList�� ���̸�ŭ ���ο� ����Ʈ�� ����� player�� �����ϴ� �� - ���� 
    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }
    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // �÷��̾� �±װ� ���� �÷��̾� ���ӿ�����Ʈ�� ����Ʈ�� ����
            player = GameObject.FindGameObjectWithTag("Player");
            playerList.Add(player);
        }
        SetCam();
    }
    private void SetCam()
    {
        for (int i = 0; i < playerList.Count; i++) // ����� ����Ʈ�� ���鼭
        {
            if (playerList[i].GetComponent<PhotonView>().IsMine) // �������� ���� �÷��̾� ���� ������Ʈ�� ���
            {
                Debug.Log($"�������� ���� {i}��° {playerList[i]}");
                cinemachineVirtualCamera.Follow = player.transform;
                cinemachineVirtualCamera.LookAt = player.transform;
            }
            else
            {
                Debug.Log($"�������� ���� {i}��° {playerList[i]}");
                continue;
            }

        }
    }*/

    /* ������Ʈ ������ �±׷� �޾ƿ��� - ����
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        photonView = player.GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            cinemachineVirtualCamera.Follow = player;
            cinemachineVirtualCamera.LookAt = player;
        }
        else return;
    }
    */

    /* ������Ʈ ������ �÷��̾� ����Ʈ �� �÷��̾� ������ �޾ƿ��� - ����
    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                players.Add(PhotonNetwork.PlayerList[i]);
            }

            foreach (Player p in players)
            {
                if(p.IsLocal)
                {
                    photonView = 
                }
            }
        }
    }
    */

    /* �濡 ������ �÷��̾ �����÷��̾��ΰ�� ���� - ����
    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(5f);
        SetCam();
    }

    private void SetCam()
    {
        foreach(Player pl in PhotonNetwork.PlayerList)
        {
            if (pl.IsLocal)
            {
                photonView = GetComponent<PhotonView>();
                Player = photonView.gameObject.transform;
                cinemachineVirtualCamera.Follow = Player;
                cinemachineVirtualCamera.LookAt = Player;
            }
            else return;
        }
    }
    */

    /* �濡 ������ �÷��̾ �������� ������� ���� - ����
    /// <summary>
    /// �������ڸ��� �ٷ� �Ǵ� ���, ����� �ȵ� �� ������ �ణ�� �����̸� �ֱ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(3f);
        SetCam();
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    private void SetCam()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            Player = photonView.transform;
            cinemachineVirtualCamera.Follow = Player;
            cinemachineVirtualCamera.LookAt = Player;
        }
    }
    */

    /*
     private void Update()
     {
         if (PhotonNetwork.InRoom)
         {
             List<Player> players = new List<Player>();
             for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
             {
                 players.Add(PhotonNetwork.PlayerList[i]);
             }

             foreach(Player p in players)
             {
                 Debug.Log(p.NickName);
                 Debug.Log(p.IsLocal);
             }
         }
     }*/
}
