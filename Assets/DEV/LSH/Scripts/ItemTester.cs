using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{
    public string itemName; // 아이템 이름
    public Sprite itemSprite; // 아이템 스프라이트
    public int quantity; // 아이템 갯수

    public ItemTester(string name, Sprite sprite, int qty)
    {
        itemName = name;
        itemSprite = sprite;
        quantity = qty;
    }
}
