using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>();// ������ �ڽ� �κ��丮
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefab; // ������ ������
    [SerializeField] int size; // �ڽ� ������
    GameSceneManager gameSceneManager;
    /*
    [SerializeField] ItemData woodItemData;
    [SerializeField] ItemData oreItemData;
    [SerializeField] ItemData fruitItemData;
    */
    private void Awake()
    {
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
    }
    /// <summary>
    /// ��Ʈ��ũ �����Լ��� AddBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddBox(string itemName)
    {
        Debug.Log("AddBox  RPC�Լ� ���� ����");
        ItemData curItemData = null;
        if (inventory.Count != 0)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemData.itemName == itemName)
                {
                    if (inventory[i].itemData.itemCount < 4)
                    {
                        curItemData = inventory[i];
                    }
                }
            }
        }

        if (curItemData != null) // ���� �������� ������
        {
            curItemData.itemData.itemCount += 1; /////////
            Debug.LogError("�ڽ��κ��丮 ���� ���濩�� Ȯ�� �ʼ�");
            UpdateItem(curItemData);

        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            // �Լ��� ���ÿ� �����ϵ�, ������ ���常
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject curObject = null;
                Item curItem = null;
                Debug.Log("���� �÷��̾ ������Ŭ���̾�Ʈ");
                // ���� �������� ���� -> ���� -> ����
                switch (itemName)
                {
                    // boxItem �����տ� PhotonView �߰� �� ����
                    case "Wood":
                        Debug.Log("���縦 �����");
                        curObject = PhotonNetwork.InstantiateRoomObject("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity);
                        curItem = curObject.GetComponent<Item>();
                        curItemData = new ItemData(curItem);
                        //woodItemData;
                        break;
                    case "Ore":
                        Debug.Log("������ �����");
                        curObject = PhotonNetwork.InstantiateRoomObject("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity);
                        curItem = curObject.GetComponent<Item>();
                        curItemData = new ItemData(curItem);
                        //oreItemData;
                        break;
                    case "Fruit":
                        Debug.Log("���Ÿ� �����");
                        curObject = PhotonNetwork.InstantiateRoomObject("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity);
                        curItem = curObject.GetComponent<Item>();
                        curItemData = new ItemData(curItem);
                        //fruitItemData;
                        break;
                    default:
                        break;
                }
                inventory.Add(curItemData); // ������ �߰�
                CreateItemUI(curItemData); // ������ UI �߰�
                PhotonNetwork.Destroy(curObject);
            }
        }
        else // �κ��丮�� ���� �� ���
        {
            return;
        }
    }

    /* 20241130 ver.
    /// <summary>
    /// ��Ʈ��ũ �����Լ��� AddBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddBox(string itemName)
    {
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ �������� �߰��Ϸ��� ���
            if (item.itemData.itemName == itemName)
            {
                if (item.itemData.itemCount < 4)
                {
                    curItem = item;
                    break;
                }
            }
        }
        if (curItem != null) // ���� �������� ������
        {
            curItem.itemData.itemCount += 1; /////////
            UpdateItem(curItem);

        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            // ���� �������� ���� -> ���� -> ����
            switch (itemName)
            {
                case "Wood":
                    curItem = woodItemData;
                    break;
                case "Ore":
                    Debug.Log("������ �����");
                    curItem = oreItemData;
                    break;
                case "Fruit":
                    Debug.Log("���Ÿ� �����");
                    curItem = fruitItemData;
                    break;
                default:
                    break;
            }
            inventory.Add(curItem); // ������ �߰�
            CreateItemUI(curItem); // ������ UI �߰�
        }
        else // �κ��丮�� ���� �� ���
        {
            return;
        }
    }
    */

    /*
    /// <summary>
    /// ��Ʈ��ũ �����Լ��� AddBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddBox(string itemName)
    {
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ �������� �߰��Ϸ��� ���
            if (item.itemData.itemName == itemName)
            {
                if (item.itemData.itemCount < 4)
                {
                    curItem = item;
                    break;
                }
            }
        }
        if (curItem != null) // ���� �������� ������
        {
            curItem.itemData.itemCount += 1;
            UpdateItem(curItem);
        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            // ���� �������� ���� -> ���� -> ����
            switch (itemName)
            {
                case "Wood":
                    Item woodItem = PhotonNetwork.Instantiate("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(woodItem);
                    PhotonNetwork.Destroy(woodItem.gameObject);
                    break;
                case "Ore":
                    Debug.Log("������ �����");
                    Item oreItem = PhotonNetwork.Instantiate("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(oreItem);
                    PhotonNetwork.Destroy(oreItem.gameObject);
                    break;
                case "Fruit":
                    Debug.Log("���Ÿ� �����");
                    Item fruitItem = PhotonNetwork.Instantiate("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(fruitItem);
                    PhotonNetwork.Destroy(fruitItem.gameObject);
                    break;
                default:
                    break;
            }
            inventory.Add(curItem); // ������ �߰�
            CreateItemUI(curItem); // ������ UI �߰�
        }
        else // �κ��丮�� ���� �� ���
        {
            return;
        }
    }
    */

    /// <summary>
    /// ��Ʈ��ũ �����Լ��� SubBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void SubBox(string itemName)
    {
        // box�� �ִ� �������� ã��
        Debug.LogError("SubBox RPC�Լ� ���� ����");
        ItemData curItemDate = null;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemData.itemName == itemName)
            {
                if (inventory[i].itemData.itemCount < 4)
                {
                    curItemDate = inventory[i];
                }
            }
        }

        if (curItemDate != null) // �������� ������
        {
            Debug.Log("������ ���� ����");
            curItemDate.itemData.itemCount -= 1; // �Ѱ� ����
            Debug.Log($"�κ��丮 ������ ���� : {curItemDate.itemData.itemName} : {curItemDate.itemData.itemCount} ");
            Debug.Log($"�κ��丮 ������ ���� : {inventory[0].itemData.itemName} : {inventory[0].itemData.itemCount} ");
            if (curItemDate.itemData.itemCount <= 0) // 0 ������ ���
            {
                inventory.Remove(curItemDate); // ����Ʈ���� ������ ����
                //Destroy(boxItem.itemPrefab.gameObject); 
                // UI�� ����� ȭ�鿡�� ����/////////
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("������Ŭ���̾�Ʈ��");
                    Debug.Log("������ ������ UI ����");
                    PhotonNetwork.Destroy(curItemDate.itemPrefab.gameObject); ////
                }
                UpdateItem(curItemDate);
            }
            else
            {
                UpdateItem(curItemDate);
            }
        }
    }

    /* 20241130 ver.
    /// <summary>
    /// ��Ʈ��ũ �����Լ��� SubBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void SubBox(string itemName)
    {
        ItemData boxItem = null;
        // box�� �ִ� �������� ã��
        foreach (ItemData item in inventory)
        {
            if (item.itemData.itemName == itemName)
            {
                boxItem = item;
                break;
            }
        }
        if (boxItem != null) // �������� ������
        {
            boxItem.itemData.itemCount -= 1; // �Ѱ� ����
            if (boxItem.itemData.itemCount <= 0) // 0 ������ ���
            {
                inventory.Remove(boxItem); // ����Ʈ���� ������ ����
                Destroy(boxItem.itemPrefab.gameObject); // UI�� ����� ȭ�鿡�� ����/////////
                UpdateItem(boxItem);
            }
            else
            {
                UpdateItem(boxItem);
            }
        }
    }
    */

    // ������ ������Ʈ
    public void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            Debug.Log("������ ������Ʈ");
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }

    public void CreateItemUI(ItemData item)
    {
        GameObject newItem = Instantiate(itemPrefab, itemContent);

        ItemPrefab itemUI = newItem.GetComponent<ItemPrefab>();
        itemUI.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

        item.itemPrefab = itemUI;
    }
}
