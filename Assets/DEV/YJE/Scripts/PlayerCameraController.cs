using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviourPunCallbacks
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    // private GameSceneManager gameSceneManager;
    public GameObject nowPlayer;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        // gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
    }

    private void Start()
    {
        StartCoroutine(StartDelayRoutine());
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

    private void SetCam()
    {
        nowPlayer = GameSceneManager.Instance.nowPlayer;
        cinemachineVirtualCamera.Follow = nowPlayer.transform;
        cinemachineVirtualCamera.LookAt = nowPlayer.transform;
    }
}
