using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera cinemachineVirtualCamera; // �ó׸ӽ� ī�޶�
    [SerializeField] TestGameScene testGameScene;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// �濡 ���� �� ����
    /// </summary>
    public override void OnJoinedRoom()
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
        yield return new WaitForSeconds(2f);
        SetCam();
    }

    private void SetCam()
    {
        cinemachineVirtualCamera.LookAt = testGameScene.nowPlayer.transform;
    }
}
