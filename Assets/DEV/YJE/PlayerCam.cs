using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviourPunCallbacks
{
    // public Camera cam;
    public CinemachineVirtualCamera cinemachineVirtualCamera; // 시네머신 카메라
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
    /// 시작하자마자 바로 되는 경우, 제대로 안될 수 있으니 약간의 딜레이를 주기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDelayRoutine()
    {
        // 네트워크 준비에 필요한 시간 설정
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
    /* 입장 시 PhotonNetwork에서 PlayerList의 길이만큼 새로운 리스트를 만들어 player를 참조하는 것 - 실패 
    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }
    IEnumerator StartDelayRoutine()
    {
        // 네트워크 준비에 필요한 시간 설정
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // 플레이어 태그가 붇은 플레이어 게임오브젝트를 리스트에 저장
            player = GameObject.FindGameObjectWithTag("Player");
            playerList.Add(player);
        }
        SetCam();
    }
    private void SetCam()
    {
        for (int i = 0; i < playerList.Count; i++) // 저장된 리스트를 돌면서
        {
            if (playerList[i].GetComponent<PhotonView>().IsMine) // 소유권을 가진 플레이어 게임 오브젝트인 경우
            {
                Debug.Log($"소유권을 가진 {i}번째 {playerList[i]}");
                cinemachineVirtualCamera.Follow = player.transform;
                cinemachineVirtualCamera.LookAt = player.transform;
            }
            else
            {
                Debug.Log($"소유권이 없는 {i}번째 {playerList[i]}");
                continue;
            }

        }
    }*/

    /* 업데이트 문에서 태그로 받아오기 - 실패
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

    /* 업데이트 문에서 플레이어 리스트 중 플레이어 정보로 받아오기 - 실패
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

    /* 방에 입장한 플레이어가 로컬플레이어인경우 참조 - 실패
    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        // 네트워크 준비에 필요한 시간 설정
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

    /* 방에 입장한 플레이어가 소유권을 가진경우 참조 - 실패
    /// <summary>
    /// 시작하자마자 바로 되는 경우, 제대로 안될 수 있으니 약간의 딜레이를 주기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDelayRoutine()
    {
        // 네트워크 준비에 필요한 시간 설정
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
