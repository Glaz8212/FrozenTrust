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


    private void Start()
    {
        if (photonView.IsMine)
        {
            LocalPlayerName();
        }

        StartCoroutine(DelayedUpdateName(2f));
    }

    // 닉네임 위치 고정 
    // 
    private void LateUpdate()
    {
        // 닉네임이 항상 카메라를 향하도록 회전
        if (Camera.main != null)
        {
            nameText.transform.position = transform.position + nameOffset;
            // LookAt으로 닉네임이 항상 카메라를 바라보게 설정
            nameText.transform.LookAt(Camera.main.transform);
            // 텍스트 반전 방지
            nameText.transform.Rotate(0, 180, 0); 
        }
    }
    
    private void LocalPlayerName()
    {
        // 로컬 플레이어의 CustomProperties에 닉네임 저장
        // 각자 닉네임을 공유
        PhotonHashtable playerProperties = new PhotonHashtable();
        playerProperties["PlayerName"] = PhotonNetwork.NickName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    private void UpdateName()
    {
        // Player CustomProperties에서 닉네임 가져오기
        if (photonView.Owner.CustomProperties.TryGetValue("PlayerName", out object playerName))
        {
            nameText.text = playerName.ToString();
        }
        else
        {
            nameText.text = "Unknown";
        }
    }
    private IEnumerator DelayedUpdateName(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateName();
    }

    // 포톤 콜백 메서드 (Photon 플레이어 객체, 변경된 CustomProperties의 키와 값을 포함한 Hashtable)
    // 플레이어 닉네임 변동 시 

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == photonView.Owner && changedProps.ContainsKey("PlayerName"))
        {
            UpdateName();
        }
    }
}

