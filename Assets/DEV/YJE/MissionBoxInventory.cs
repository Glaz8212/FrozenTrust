using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class MissionBoxInventory : MonoBehaviour
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// ������ �ڽ� �κ��丮
    public List<int> inventoryCount = new List<int>(); // ������ ������ �����ϴ� ����Ʈ
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefabObj; // ������ ������
    int size = 3; // �ڽ� ������

    [SerializeField] PlayerInventory playerInventory;

    public int missionWoodCount; // ���� �̼ǿ� �ʿ��� ���� - ������ �������� �ٸ��� ���
    public int missionOreCount; // ���� �̼ǿ� �ʿ��� ����
    public int missionFruitCount; // ���� �̼ǿ� �ʿ��� ����

    public bool IsEnterChecked = false; 

    // �������� �̼� ���� ����
    private void Start()
    {
        int num = Random.Range(0, 5);
        missionWoodCount = num;
        num = Random.Range(0, 5);
        missionOreCount = num;
        num = Random.Range(0, 5);
        missionFruitCount = num;
        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();
    }

    /// <summary>
    /// ��Ʈ��ũ �����Լ��� AddBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddMission(string itemName)
    {
        Debug.Log("AddMission RPC�Լ� ���� ����");
        ItemData curItemData = null;
        int curItemCount = 0;
        int index = 0; // i�� ���� �����ϴ� index
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
                        break; // ������ �������� ã�� ��� for�� ����
                    }
                }
            }
        }

        if (curItemData != null) // ���� �������� �̹� ������
        {
            // �� ������ ���� �ʿ��� ������ ������ ���� á���� Ȯ���� �ʿ�
            switch (curItemData.itemData.itemName)
            {
                case "Wood":
                    if (curItemCount == missionWoodCount)
                    {
                        Debug.Log("�� �̻� ���� �� �����ϴ�.");
                        IsEnterChecked = true; // ���� �� �ִ� ���
                        //playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1); //�߰��� �����Ǵ� ���� ����
                        return;
                    }
                    else
                        break;
                case "Ore":
                    if (curItemCount == missionOreCount)
                    {
                        Debug.Log("�� �̻� ���� �� �����ϴ�.");
                        IsEnterChecked = true; // ���� �� �ִ� ���
                        //playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1); //�߰��� �����Ǵ� ���� ����
                        return;
                    }
                    else
                        break;
                case "Fruit":
                    if (curItemCount == missionFruitCount)
                    {
                        Debug.Log("�� �̻� ���� �� �����ϴ�.");
                        IsEnterChecked = true; // ���� �� �ִ� ���
                        //playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1); //�߰��� �����Ǵ� ���� ����
                        return;
                    }
                    else
                        break;
                default:
                    break;
            }
            // �� ���� �� ������ �߰�
            curItemCount += 1;
            inventoryCount[index] = curItemCount; // �κ��丮�� ī��Ʈ ����
            Debug.LogWarning($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            UpdateItem(curItemData, curItemCount); // UI ������Ʈ
        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            curItemCount = curItemData.itemData.itemCount; // ������ �ʱ� �� ����
            Debug.Log($"{curItemData.itemData.itemName} : {curItemCount}");
            Debug.Log("�κ��丮 �߰�");
            inventory.Add(curItemData); // ������ �߰�
            inventoryCount.Add(curItemCount); // �������� ���� �߰�
            Debug.Log("UI����");
            CreateItemUI(curItemData); // �� �� ¥�� ����
            Debug.Log("������ ���� ������Ʈ �ݳ�");
            DeleteItemObject(itemName);
            Debug.Log("���� �Ϸ�");
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
    public void MissionSteal(string itemName)
    {
        // box�� �ִ� �������� ã��
        Debug.LogError("MissionSteal RPC�Լ� ���� ����");
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
                    break;
                }
            }
        }

        if (curItemData != null) // �������� ������
        {
            Debug.Log("������ ���� ����");
            curItemCount -= 1; // �Ѱ� ����
            Debug.Log($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            inventoryCount[index] = curItemCount; // �κ��丮�� ī��Ʈ ����
            Debug.Log($"{inventoryCount[index]}");
            if (inventoryCount[index] <= 0) // 0 ������ ���
            {
                Debug.Log(curItemData.itemData.gameObject.name);
                Destroy(curItemData.itemPrefab.gameObject);
                Debug.Log("�κ��丮���� ����");
                inventory.Remove(curItemData); // ����Ʈ���� ������ ����
                inventoryCount.RemoveAt(index); // ����Ʈ���� �ε��� ��ȣ�� ��ġ�� ���� ����
            }
            else
            {
                Debug.Log("UI ����");
                UpdateItem(curItemData, curItemCount);
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
            Debug.Log("������UI ������Ʈ");

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
