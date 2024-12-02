using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun//, IPunObservable
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// ������ �ڽ� �κ��丮
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefabObj; // ������ ������
    [SerializeField] int size; // �ڽ� ������
    GameSceneManager gameSceneManager;
    /*
    [SerializeField] GameObject woodItem;
    [SerializeField] GameObject oreItem;
    [SerializeField] GameObject fruitItem;
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

        if (curItemData != null) // ���� �������� �̹� ������
        {
            curItemData.itemData.itemCount += 1;
            Debug.Log($"�κ��丮 ������ ���� : {curItemData.itemData.itemName} : {curItemData.itemData.itemCount} ");
            UpdateItem(curItemData);
        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            Debug.Log("�κ��丮 �߰�");
            inventory.Add(curItemData); // ������ �߰�
            Debug.Log("UI����");
            CreateItemUI(curItemData);
            //photonView.RPC("CreateItemUI", RpcTarget.All, itemName);
            Debug.Log("������ ���� ������Ʈ �ݳ�");
            DeleteItemObject(itemName);
            Debug.LogError("���� �Ϸ�");
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

    /// <summary>
    /// ��Ʈ��ũ �����Լ��� SubBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void SubBox(string itemName)
    {
        // box�� �ִ� �������� ã��
        Debug.LogError("SubBox RPC�Լ� ���� ����");
        ItemData curItemData = null;
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

        if (curItemData != null) // �������� ������
        {
            Debug.Log("������ ���� ����");
            curItemData.itemData.itemCount -= 1; // �Ѱ� ����
            Debug.Log($"�κ��丮 ������ ���� : {curItemData.itemData.itemName} : {curItemData.itemData.itemCount} ");
            if (curItemData.itemData.itemCount <= 0) // 0 ������ ���
            {
                inventory.Remove(curItemData); // ����Ʈ���� ������ ����
                Debug.Log("�κ��丮���� ����");
                Destroy(curItemData.itemPrefab.gameObject);
                UpdateItem(curItemData);
            }
            else
            {
                UpdateItem(curItemData);
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
            Debug.LogError("������ ������Ʈ");
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }

    public void CreateItemUI(ItemData item)
    {

        GameObject itemUI = Instantiate(itemPrefabObj, itemContent);
        ItemPrefab itemPrefab = itemUI.GetComponent<ItemPrefab>();
        Debug.Log("UI �ڷ� ����");
        itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

    }

    /// <summary>
    /// �̸��� �´� ������ ������ ����� ���ؼ� �б��Ͽ� ������Ʈ�� �������� �Լ�
    /// </summary>
    /// <param name="itemName"></param>
    public GameObject MakeItemObject(string itemName)
    {
        Debug.Log("�̸��� �´� ������ ������ ����");
        GameObject curObject = null;
        Item curItem = null;
        ItemData curItemData = null;

        GameObject newItemUI = null;
        ItemPrefab itemUI = null;

        switch (itemName)
        {
            case "Wood":
                Debug.Log("���縦 �����");
                curObject = itemController.MakeWoodItem();
                return curObject;

            case "Ore":
                Debug.Log("������ �����");
                curObject = itemController.MakeOreItem();
                return curObject;

            case "Fruit":
                Debug.Log("���Ÿ� �����");
                curObject = itemController.MakeFruitItem();
                return curObject;

            default:
                return null;
        }
    }
    /// <summary>
    /// �̸��� �´� ������ ������ ������ ���ؼ� �б��Ͽ� ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="itemName"></param>
    public void DeleteItemObject(string itemName)
    {
        Debug.Log("������Ʈ ���� �Լ�");
        switch (itemName)
        {
            case "Wood":
                Debug.Log("���� ������ ����");
                itemController.ResetWoodItem();
                break;
            case "Ore":
                Debug.Log("���� ������ ����");
                itemController.ResetOreItem();
                break;
            case "Fruit":
                Debug.Log("���� ������ ����");
                itemController.ResetFruitItem();
                break;
            default:
                break;
        }
    }
}
