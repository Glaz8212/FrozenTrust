using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class NamePrint : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Vector3 nameOffset;
    private Player player;


    private void Start()
    {
        // �����÷��̾� �϶��� �۵�
        if (photonView.IsMine)
        {
            LocalPlayerName();
        }

        // �г��� �ؽ�Ʈ�� ������Ʈ
        UpdateName();
    }

    // �г��� ��ġ ���� 
    // 
    private void LateUpdate()
    {
        // �г����� �׻� ī�޶� ���ϵ��� ȸ��
        if (Camera.main != null)
        {
            nameText.transform.position = transform.position + nameOffset;
            // LookAt���� �г����� �׻� ī�޶� �ٶ󺸰� ����
            nameText.transform.LookAt(Camera.main.transform);
            // �ؽ�Ʈ ���� ����
            nameText.transform.Rotate(0, 180, 0); 
        }
    }
    
    private void LocalPlayerName()
    {
        // ���� �÷��̾��� CustomProperties�� �г��� ����
        // ���� �г����� ����
        PhotonHashtable playerProperties = new PhotonHashtable();
        playerProperties["PlayerName"] = PhotonNetwork.NickName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    private void UpdateName()
    {
        // Player CustomProperties���� �г��� ��������
        if (photonView.Owner.CustomProperties.TryGetValue("PlayerName", out object playerName))
        {
            nameText.text = playerName.ToString();
        }
        else
        {
            nameText.text = "Unknown";
        }
    }
}

