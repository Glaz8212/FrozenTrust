using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>(); // ���� �κ��丮 ������
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    public GameObject itemPrefab; // ������ ������

    // ������ �߰�
    public void AddItem(string itemName, Sprite sprite, int quantity)
    {
        ItemData playerItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ ����� ����
            if (item.itemData.itemName == itemName)
            {
                playerItem = item;
                break; 
            }
        }
        if (playerItem != null)
        {
            // ������ �������̸� ������ ����
            playerItem.itemData.itemCount += quantity;
            UpdateItem(playerItem);
        }
        else
        {
            // �� ������ �߰�
            ItemTester newItem = new ItemTester(itemName, sprite, quantity);
            ItemData newItemData = new ItemData(newItem);
            inventory.Add(newItemData);
            CreateItemUI(newItemData);
        }
    }

    // ������ ����
    private void CreateItemUI(ItemData item)
    {
        // �������� �ν��Ͻ�ȭ�Ͽ� ����
        GameObject newItem = Instantiate(itemPrefab, itemContent);

        // UI �����͸� ������Ʈ
        ItemPrefab itemUI = newItem.GetComponent<ItemPrefab>();
        itemUI.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

        // UI ��ü�� item�� ����
        item.itemPrefab = itemUI;
    }

    // ������ ������Ʈ
    private void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }
}

// �����۰� UI ��Ҹ� �Բ� �����ϴ� Ŭ����
public class ItemData
{
    public ItemTester itemData; // ������ ������
    public ItemPrefab itemPrefab; // ������ UI ���

    public ItemData(ItemTester item)
    {
        this.itemData = item;
    }
}
