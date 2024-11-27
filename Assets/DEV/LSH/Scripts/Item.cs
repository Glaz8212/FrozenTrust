using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // ������ �̸�
    public Sprite itemSprite; // ������ ��������Ʈ
    public int itemCount; // ������ ����

    public Item(string name, Sprite sprite, int count)
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
