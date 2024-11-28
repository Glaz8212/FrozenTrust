using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>(); // 로컬 인벤토리 데이터
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    [SerializeField] GameObject itemPrefab; // 아이템 프리팹

    // 아이템 추가
    public void AddItem(string itemName, Sprite sprite, int quantity)
    {
        ItemData playerItem = null;
        foreach (ItemData item in inventory)
        {
            // 같은 아이템 존재시 종료
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
            // 보유한 아이템이면 갯수만 증가
            playerItem.itemData.itemCount += quantity;
            UpdateItem(playerItem);
        }
        else if(inventory.Count <= 4)
        {
            // 새 아이템 추가
            Item newItem = new Item(itemName, sprite, quantity);
            ItemData newItemData = new ItemData(newItem);
            inventory.Add(newItemData);
            //CreateItemUI(newItemData);
        }
        else
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
        }
    }

    public bool RemoveItem(string itemName, int itemCount)
    {
        ItemData playerItem = null;
        foreach (ItemData item in inventory)
        {
            // 같은 아이템 존재시 삭제를 위해 대입하고 종료
            if (item.itemData.itemName == itemName)
            {
                playerItem = item;
                break;
            }
        }

        // 인벤에 아이템이 있을 시 1개 제거
        if (playerItem != null)
        {
            playerItem.itemData.itemCount -= itemCount;

            if (playerItem.itemData.itemCount <= 0)
            {
                // 수량이 0 이하일 경우 제거
                inventory.Remove(playerItem);
                // UI 제거
                Destroy(playerItem.itemPrefab.gameObject); 
            }
            else
            {
                UpdateItem(playerItem);
            }
            // 제거 성공
            return true;
        }
        // 제거 실패
        return false;
    }

    // 아이템 생성
    private void CreateItemUI(ItemData item)
    {
        // 프리팹을 인스턴스화하여 생성
        GameObject newItem = Instantiate(itemPrefab, itemContent);

        // UI 데이터를 업데이트
        ItemPrefab itemUI = newItem.GetComponent<ItemPrefab>();
        itemUI.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

        // UI 객체를 연결
        item.itemPrefab = itemUI;
    }

    // 아이템 업데이트
    public void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }
}

// 아이템과 UI 요소를 관리하는 클래스
[System.Serializable]
public class ItemData
{
    // 아이템 데이터
    public Item itemData;
    // 아이템 UI 요소
    public ItemPrefab itemPrefab;

    public ItemData(Item item)
    {
        this.itemData = item;
    }
}
