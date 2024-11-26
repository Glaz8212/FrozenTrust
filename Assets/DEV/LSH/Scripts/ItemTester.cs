using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{
    public string itemName; // 아이템 이름
    public Sprite itemSprite; // 아이템 스프라이트
    public int itemCount; // 아이템 갯수

    public ItemTester(string name, Sprite sprite, int count)
    {
        itemName = name;
        itemSprite = sprite;
        itemCount = count;
    }

    public void interaction(PlayerInventory playerInventory)
    {
        playerInventory.AddItem(itemName, itemSprite, itemCount);

        Destroy(gameObject);
    }
}
