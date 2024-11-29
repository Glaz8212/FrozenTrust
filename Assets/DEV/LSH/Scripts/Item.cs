using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // ������ �̸�
    public Sprite itemSprite; // ������ ��������Ʈ
    public int itemCount; // ������ ����

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
            // RPC ������ Ŭ���̾�Ʈ�� ViewID ����
            photonView.RPC(nameof(DestroyItem), RpcTarget.MasterClient, photonView.ViewID);
        }
        
        //PhotonNetwork.Destroy(gameObject);
        // Destroy(gameObject);
    }

    [PunRPC]
    public void DestroyItem(int viewID)
    {
        // ������ Ŭ���̾�Ʈ�� ������ ��û
        if (PhotonNetwork.IsMasterClient)
        {
            // ����� ID�� ������� ������Ʈ �˻�
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                PhotonNetwork.Destroy(view.gameObject);
                Debug.Log($"�������� ���������� �����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogError($"{viewID}�� ã�� �� �����ϴ�.");
            }
        }
    }
}
