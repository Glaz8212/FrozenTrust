using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedItem : MonoBehaviourPun
{
    [Header("���� ������ ��ư ����")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("������ ��ũ��Ʈ")]
    public GameSceneManager gameSceneManager;
    public PlayerInteraction playerInteraction;
    public BoxInventory boxInventory;
    public PlayerInventory playerInventory;

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
    }

    /// <summary>
    /// Player Inventory�� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - PlayerInventory���� Item Box�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void PlayerAddBox()
    {
        // ���� ��ư�� Ŭ���� ���� ������Ʈ
        nowClicked = EventSystem.current.currentSelectedGameObject;
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
        nowClicked = EventSystem.current.currentSelectedGameObject;
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();

        // TODO : ItemBox�� �ִ� boxInventory�� �����Ұ�
        Debug.Log("���õ� ���������� ItemData ����");
        ItemData curItem = FindItem(nowItemPrefab.itemNameText.text);
        Debug.Log("���� Inventory�� �߰�");
        playerInventory.AddItem(curItem.itemData.itemName, curItem.itemData.itemSprite, curItem.itemData.itemCount);

        // ������ �÷��̾�� �����Ͽ� ������ �ڽ��� �����ִ� �����̹Ƿ�
        Debug.Log("ItemBox�� ������ �Լ��� ����");
        photonView.RPC("SubBox", RpcTarget.All, nowItemPrefab.itemNameText.text);

        Debug.Log("�����ۻ���");
        PhotonNetwork.Destroy(curItem.itemData.gameObject);
    }

    private ItemData FindItem(string itemName)
    {
        ItemData itemData = null;
        // ���� �������� ���� -> ���� -> ����
        switch (itemName)
        {
            case "Wood":
                Item woodItem = PhotonNetwork.Instantiate("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                itemData = new ItemData(woodItem);
                return itemData;
            case "Ore":
                Debug.Log("������ �����");
                Item oreItem = PhotonNetwork.Instantiate("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                itemData = new ItemData(oreItem);
                return itemData;
            case "Fruit":
                Debug.Log("���Ÿ� �����");
                Item fruitItem = PhotonNetwork.Instantiate("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                itemData = new ItemData(fruitItem);
                return itemData;
            default:
                break;
        }
        return null;
    }
}
