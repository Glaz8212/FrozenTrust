using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerStatus;

public class PlayerClickedItem : MonoBehaviour
{
    [Header("���� ������ ��ư ����")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("������ ��ũ��Ʈ")]
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] BoxController boxController;
    private PlayerStatus playerStatus;
    // ItmeBoxList ������Ʈ�� �ִ� BoxInventroyList.cs
    [SerializeField] BoxInventoryList boxInventoryList;

    [SerializeField] BoxInventory boxInventory;
    [SerializeField] PhotonView photonView;
    [SerializeField] PlayerInventory playerInventory;

    private void Start()
    {
        StartCoroutine(StartDelayRoutine());
    }
    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(3f);
        SetPlayerInventory();
    }
    /// <summary>
    /// ������ �÷��̾��κ��丮�ϰ� ��ȣ�ۿ��ؾ��ϹǷ� PlayerInventroy.cs ����
    /// </summary>
    private void SetPlayerInventory()
    {
        // �ڽ� ��Ʈ�ѷ��� �ҷ����� ���� ������ �÷��̾��� PlayerInteration.cs ����
        playerInteraction = GameSceneManager.Instance.nowPlayer.GetComponent<PlayerInteraction>();

        playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();

        // ������ �θ� ������Ʈ�� �ִ� BoxInventoryList.cs����
        boxInventoryList = GameObject.Find("ItemBoxList").GetComponent<BoxInventoryList>();

        playerStatus = GameSceneManager.Instance.nowPlayer.GetComponent<PlayerStatus>();
    }

    /// <summary>
    /// Player Inventory�� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - PlayerInventory���� Item Box�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void PlayerAddBox()
    {
        // ���� ��ư�� Ŭ���� ���� ������Ʈ
        nowClicked = EventSystem.current.currentSelectedGameObject.gameObject;
        // ������Ʈ�� ItemPrefab.cs ����
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();

        // player�� ��ȣ�ۿ��� BoxConroller.cs ����
        boxController = playerInteraction.boxController;

        Debug.LogError(boxController.gameObject.transform.GetSiblingIndex());
        boxInventory = boxInventoryList.boxInventorylist[boxController.gameObject.transform.GetSiblingIndex()];
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController ������ �� ���
        {
            // Box�� �κ��丮 UI�� �����ִ� ���
            if (boxController.IsUIOpen == false)
            {
                if (nowItemPrefab.itemNameText.text == "Fruit" || nowItemPrefab.itemNameText.text == "Meat" )
                {
                    playerStatus.HealHunger(200f);
                    Debug.LogError("�÷��̾��� ��⸦ ����");
                    playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                }

                Debug.Log("���� ����� �� �����ϴ�.");
                return;
            }
            // Box�� �κ��丮 UI�� �����ִ� ��� �߰�
            else if (boxController.IsUIOpen == true)
            {
                // BoxInventory.cs�� RPC�Լ��� AddBox����
                photonView.RPC("AddBox", RpcTarget.All, nowItemPrefab.itemNameText.text);
                playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                return;
            }
        }
        else // ������ BoxController.cs�� ���� ���
        {
            return;
        }
    }


}
