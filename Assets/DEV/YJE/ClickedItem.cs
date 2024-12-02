using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedItem : MonoBehaviourPun
{
    [Header("���� ������ ��ư ����")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("������ ��ũ��Ʈ")]
    [SerializeField] GameSceneManager gameSceneManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] BoxController boxController;
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
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        // �ڽ� ��Ʈ�ѷ��� �ҷ����� ���� ������ �÷��̾��� PlayerInteration.cs ����
        playerInteraction = gameSceneManager.nowPlayer.GetComponent<PlayerInteraction>();
        playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();
        /*
        // player�� ��ȣ�ۿ��� BoxConroller.cs ����
        boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        photonView = boxInventory.GetComponent<PhotonView>();*/
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
        BoxController boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController ������ �� ���
        {
            // Box�� �κ��丮 UI�� �����ִ� ��� �۵� x
            if (boxController.IsUIOpen == false)
            {
                if (nowItemPrefab.itemNameText.text == "Fruit")
                {
                    // TODO: �÷��̾��� ��⸦ �÷��ش�
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

    /*
    /// <summary>
    /// Player Inventory�� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - PlayerInventory���� Item Box�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void PlayerAddBox()
    {
        // ���� ��ư�� Ŭ���� ���� ������Ʈ
        nowClicked = EventSystem.current.currentSelectedGameObject;
        // ������Ʈ�� ItemPrefab.cs ����
        nowItemUI = nowClicked.GetComponent<ItemPrefab>();

        // player�� ��ȣ�ۿ��� BoxConroller.cs ����
        BoxController boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController ������ �� ���
        {
            // Box�� �κ��丮 UI�� �����ִ� ��� �۵� x
            if (boxController.IsUIOpen == false)
            {
                return;
            }
            // Box�� �κ��丮 UI�� �����ִ� ��� �߰�
            else if (boxController.IsUIOpen == true)
            {
                // BoxInventory.cs�� RPC�Լ��� AddBox����
                photonView.RPC("AddBox", RpcTarget.All, nowItemUI.itemNameText.text);
                playerInventory.RemoveItem(nowItemUI.itemNameText.text, 1);
                return;
            }
        }
        else // ������ BoxController.cs�� ���� ���
        {
            return;
        }
    }
   */
    /// <summary>
    /// Box�� �ִ� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - Item Box���� PlayerInventory�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void BoxAddPlayer()
    {
        Debug.Log("��ư Ŭ��");
        // ���� ��ư�� Ŭ���� ���� ������Ʈ
        nowClicked = EventSystem.current.currentSelectedGameObject.gameObject; ////
        // ������Ʈ�� ItemPrefab.cs ����
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();

        // ��ȣ�ۿ��� ��ư�� �θ� ���� BoxInventroy.cs ����
        boxInventory = nowClicked.GetComponentInParent<BoxInventory>(); ////
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        GameObject curObject = boxInventory.MakeItemObject(nowItemPrefab.itemNameText.text);
        Item curItem = curObject.GetComponent<Item>();
        ItemData curItemData = new ItemData(curItem);

        /*
                // TODO : ItemBox�� �ִ� boxInventory�� �����Ұ�
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("������Ŭ���̾�Ʈ �ѻ����");
                    Debug.Log("���õ� ���������� ItemData ����");
                    curObject = FindItem(nowItemPrefab.itemNameText.text);
                }
        */

        Debug.Log("���� Inventory�� �߰�");
        playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, curItemData.itemData.itemCount);

        // ������ �÷��̾�� �����Ͽ� ������ �ڽ��� �����ִ� �����̹Ƿ�
        Debug.Log("ItemBox�� ������ �Լ��� ����");
        photonView.RPC("SubBox", RpcTarget.All, curItemData.itemData.itemName);
        boxInventory.DeleteItemObject(nowItemPrefab.itemNameText.text);
        /*
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("������Ŭ���̾�Ʈ �ѻ����");
                    Debug.Log("�����ۻ���");
                    PhotonNetwork.Destroy(curObject);
                }
        */
    }
    /*
    private GameObject FindItem(string itemName)
    {
        GameObject curObject = null;
        // ���� �������� ���� -> ���� -> ����
        switch (itemName)
        {
            // boxItem �����տ� PhotonView �߰� �� ����
            case "Wood":
                Debug.Log("���縦 �����");
                curObject = PhotonNetwork.InstantiateRoomObject("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity);
                //woodItemData;
                return curObject;
            case "Ore":
                Debug.Log("������ �����");
                curObject = PhotonNetwork.InstantiateRoomObject("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity);
                //oreItemData;
                return curObject;
            case "Fruit":
                Debug.Log("���Ÿ� �����");
                curObject = PhotonNetwork.InstantiateRoomObject("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity);
                //fruitItemData;
                return curObject;
            default:
                break;
        }
        return null;
    }
    */
}
