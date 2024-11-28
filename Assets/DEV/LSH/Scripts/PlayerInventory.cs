using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>(); // ���� �κ��丮 ������
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefab; // ������ ������

    // ������ �߰�
    public void AddItem(string itemName, Sprite sprite, int quantity)
    {
        ItemData playerItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ ����� ����
            if (item.itemData.itemName == itemName)
            {
                if(item.itemData.itemCount < 4)
                {
                    playerItem = item;
                    break;
                }               
            }
        }
        if (playerItem != null)
        {
            // ������ �������̸� ������ ����
            playerItem.itemData.itemCount += quantity;
            UpdateItem(playerItem);
        }
        else if(inventory.Count <= 4)
        {
            // �� ������ �߰�
            Item newItem = new Item(itemName, sprite, quantity);
            ItemData newItemData = new ItemData(newItem);
            inventory.Add(newItemData);
            //CreateItemUI(newItemData);
        }
        else
        {
            Debug.Log("�κ��丮�� ���� á���ϴ�.");
        }
    }

    public bool RemoveItem(string itemName, int itemCount)
    {
        ItemData playerItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ ����� ������ ���� �����ϰ� ����
            if (item.itemData.itemName == itemName)
            {
                playerItem = item;
                break;
            }
        }

        // �κ��� �������� ���� �� 1�� ����
        if (playerItem != null)
        {
            playerItem.itemData.itemCount -= itemCount;

            if (playerItem.itemData.itemCount <= 0)
            {
                // ������ 0 ������ ��� ����
                inventory.Remove(playerItem);
                // UI ����
                Destroy(playerItem.itemPrefab.gameObject); 
            }
            else
            {
                UpdateItem(playerItem);
            }
            // ���� ����
            return true;
        }
        // ���� ����
        return false;
    }

    // ������ ����
    private void CreateItemUI(ItemData item)
    {
        // �������� �ν��Ͻ�ȭ�Ͽ� ����
        GameObject newItem = Instantiate(itemPrefab, itemContent);

        // UI �����͸� ������Ʈ
        ItemPrefab itemUI = newItem.GetComponent<ItemPrefab>();
        itemUI.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

        // UI ��ü�� ����
        item.itemPrefab = itemUI;
    }

    // ������ ������Ʈ
    public void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }
}

// �����۰� UI ��Ҹ� �����ϴ� Ŭ����
[System.Serializable]
public class ItemData
{
    // ������ ������
    public Item itemData;
    // ������ UI ���
    public ItemPrefab itemPrefab;

    public ItemData(Item item)
    {
        this.itemData = item;
    }
}
