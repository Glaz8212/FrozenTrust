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
                PhotonNetwork.Destroy(boxItem.itemPrefab.gameObject); // UI�� ����� ȭ�鿡�� ����
                UpdateItem(boxItem);
            }
            else
            {
                UpdateItem(boxItem);
            }
        }
    }


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
