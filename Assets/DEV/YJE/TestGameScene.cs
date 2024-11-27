using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��� �׽�Ʈ�� ���� �ӽ� �׽�Ʈ ��ũ��Ʈ
/// - �ٷ� ���� ���� ����� �� �ֵ��� �ϴ� ��ũ��Ʈ
/// </summary>
public class TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";

    /// <summary>
    /// ���� �������ڸ��� ������ ����
    /// </summary>
    private void Start()
    {
        // �÷��̾��� �̸��� ����
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// �������ڸ��� ���� �ɼ��� ���ϰ� ���� �����ϴ� ��û�� ����
    /// </summary>
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    /// <summary>
    /// �濡 ������ ��Ȳ
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
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }
    public void TestGameStart()
    {
        Debug.Log("�׽�Ʈ�� ���� ����");

        // ���� �������ڸ��� �÷��̾ ����
        PlayerSpawn();
    }

    /// <summary>
    /// �÷��̾ �����ϴ� �Լ�
    ///  - ���������� �÷��̾ ������ ��, �������� �׻� Resource ������ �־�� ��
    ///  - ��ΰ� �ʿ��Ѱ�� (Resource) ������/�����ո� ���� ��� ����
    /// </summary>
    private void PlayerSpawn()
    {
        //�÷��̾ ������ ������ ��ġ
        Vector3 randomPos = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("JHS/Player01", randomPos, Quaternion.identity);
    }
}
