using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun//, IPunObservable
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// ������ �ڽ� �κ��丮
    public List<int> inventoryCount = new List<int>(); //*********************************
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefabObj; // ������ ������
    [SerializeField] int size; // �ڽ� ������
    GameSceneManager gameSceneManager;

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
        int curItemCount = 0;
        int index = 0; // i�� ���� ����
        if (inventory.Count != 0)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemData.itemName == itemName)
                {
                    if (inventoryCount[i] < 4) // inventoryCount ����Ʈ�� ���尳���� ���� ����
                    {
                        curItemData = inventory[i];
                        curItemCount = inventoryCount[i];
                        index = i;
                        break;
                    }
                }
            }
        }

        if (curItemData != null) // ���� �������� �̹� ������
        {
            Debug.LogWarning($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            curItemCount += 1;//***************************
            inventoryCount[index] = curItemCount; // �κ��丮�� ī��Ʈ ����
            Debug.LogWarning($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            UpdateItem(curItemData, curItemCount);//********************************************
        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            curItemCount = curItemData.itemData.itemCount; // �ʱ� �� ����
            Debug.LogError($"{curItemData.itemData.itemName} : {curItemCount}");
            Debug.Log("�κ��丮 �߰�");
            inventory.Add(curItemData); // ������ �߰�
            inventoryCount.Add(curItemCount); // �������� ���� �߰�
            Debug.Log("UI����");
            CreateItemUI(curItemData); // �� �� ¥�� ����
            Debug.Log("������ ���� ������Ʈ �ݳ�");
            DeleteItemObject(itemName);
            Debug.LogError("���� �Ϸ�");
        }
        else // �κ��丮�� ���� �� ���
        {
            return;
        }
    }
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
        int curItemCount = 0;
        int index = 0; // i�� ���� ����
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemData.itemName == itemName)
            {
                if (inventoryCount[i] <= 4) // �������� ���� ������ 4 ����
                {
                    curItemData = inventory[i];
                    curItemCount = inventoryCount[i];
                    index = i; // �κ��丮 ���� �� ���

                    Debug.LogWarning($"{inventory[i].itemData.itemCount}");
                    Debug.LogWarning($"{inventoryCount[i]}");
                    Debug.LogWarning($"{i}");
                    break;
                }
            }
        }

        if (curItemData != null) // �������� ������
        {
            Debug.Log("������ ���� ����");
            Debug.LogError($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            curItemCount -= 1; // �Ѱ� ����
            Debug.LogError($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            inventoryCount[index] = curItemCount; // �κ��丮�� ī��Ʈ ����
            if (curItemCount <= 0) // 0 ������ ���
            {
                Debug.Log(curItemData.itemData.gameObject.name);
                Destroy(curItemData.itemPrefab.gameObject);
                Debug.Log("�κ��丮���� ����");
                inventory.Remove(curItemData); // ����Ʈ���� ������ ����
                inventoryCount.RemoveAt(index); // ����Ʈ���� �ε��� ��ȣ�� ��ġ�� ���� ����****************
            }
            else
            {
                UpdateItem(curItemData, curItemCount);
                Debug.LogError($"�κ��丮 ������ ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            }
        }
    }

    /// <summary>
    /// ������UI�� ������Ʈ
    /// </summary>
    /// <param name="item"></param>
    /// <param name="curItemCount"></param>
    public void UpdateItem(ItemData item, int curItemCount)
    {
        if (item.itemPrefab != null)////////
        {
            Debug.LogError("������ ������Ʈ");

            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, curItemCount);
        }
    }

    /// <summary>
    /// ó�� ������ �������� UI�� ����
    /// </summary>
    /// <param name="item"></param>
    public void CreateItemUI(ItemData item)
    {

        GameObject itemUI = Instantiate(itemPrefabObj, itemContent);
        ItemPrefab itemPrefab = itemUI.GetComponent<ItemPrefab>();
        Debug.Log("UI �ڷ� ����");
        itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        item.itemPrefab = itemPrefab;
    }

    /// <summary>
    /// �̸��� �´� ������ ������ ����� ���ؼ� �б��Ͽ� ������Ʈ�� �������� �Լ�
    /// </summary>
    /// <param name="itemName"></param>
    public GameObject MakeItemObject(string itemName)
    {
        Debug.Log("�̸��� �´� ������ ������ ����");
        GameObject curObject = null;
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
