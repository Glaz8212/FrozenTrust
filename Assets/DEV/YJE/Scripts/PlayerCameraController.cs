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
    /// �������ڸ��� �ٷ� �Ǵ� ���, ����� �ȵ� �� ������ �ణ�� �����̸� �ֱ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
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
