using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>(); // 로컬 인벤토리 데이터
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    public GameObject itemPrefab; // 아이템 프리팹

    // 아이템 추가
    public void AddItem(string itemName, Sprite sprite, int quantity)
    {
        ItemData playerItem = null;
        foreach (ItemData item in inventory)
        {
            // 같은 아이템 존재시 종료
            if (item.itemData.itemName == itemName)
            {
                playerItem = item;
                break; 
            }
        }
        if (playerItem != null)
        {
            // 보유한 아이템이면 갯수만 증가
            playerItem.itemData.itemCount += quantity;
            UpdateItem(playerItem);
        }
        else
        {
            // 새 아이템 추가
            ItemTester newItem = new ItemTester(itemName, sprite, quantity);
            ItemData newItemData = new ItemData(newItem);
            inventory.Add(newItemData);
            CreateItemUI(newItemData);
        }
    }

    // 아이템 생성
    private void CreateItemUI(ItemData item)
    {
        // 프리팹을 인스턴스화하여 생성
        GameObject newItem = Instantiate(itemPrefab, itemContent);

        // UI 데이터를 업데이트
        ItemPrefab itemUI = newItem.GetComponent<ItemPrefab>();
        itemUI.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

        // UI 객체를 item에 연결
        item.itemPrefab = itemUI;
    }

    // 아이템 업데이트
    private void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }
}

// 아이템과 UI 요소를 함께 관리하는 클래스
public class ItemData
{
    public ItemTester itemData; // 아이템 데이터
    public ItemPrefab itemPrefab; // 아이템 UI 요소

    public ItemData(ItemTester item)
    {
        this.itemData = item;
    }
}
