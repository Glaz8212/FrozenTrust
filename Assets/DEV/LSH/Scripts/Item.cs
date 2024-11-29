using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // 아이템 이름
    public Sprite itemSprite; // 아이템 스프라이트
    public int itemCount; // 아이템 갯수

    public Item(string name, Sprite sprite, int count)
    {
        itemName = name;
        itemSprite = sprite;
        itemCount = count;
    }

    public void interaction(PlayerInventory playerInventory)
    {

        playerInventory.AddItem(itemName, itemSprite, itemCount);

        PhotonView photonView = GetComponent<PhotonView>();

        if(photonView != null)
        {
            // RPC 마스터 클라이언트에 ViewID 전송
            photonView.RPC(nameof(DestroyItem), RpcTarget.MasterClient, photonView.ViewID);
        }
        
        //PhotonNetwork.Destroy(gameObject);
        // Destroy(gameObject);
    }

    [PunRPC]
    public void DestroyItem(int viewID)
    {
        // 마스터 클라이언트에 삭제를 요청
        if (PhotonNetwork.IsMasterClient)
        {
            // 포톤뷰 ID를 기반으로 오브젝트 검색
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                PhotonNetwork.Destroy(view.gameObject);
                Debug.Log($"아이템이 정상적으로 삭제되었습니다.");
            }
            else
            {
                Debug.LogError($"{viewID}를 찾을 수 없습니다.");
            }
        }
    }
}
