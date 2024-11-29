using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviourPunCallbacks
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private GameSceneManager gameSceneManager;
    public GameObject nowPlayer;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
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
        nowPlayer = gameSceneManager.nowPlayer;
        cinemachineVirtualCamera.LookAt = nowPlayer.transform;
    }
}
