using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{
    public string itemName; // ������ �̸�
    public Sprite itemSprite; // ������ ��������Ʈ
    public int quantity; // ������ ����

    public ItemTester(string name, Sprite sprite, int qty)
    {
        itemName = name;
        itemSprite = sprite;
        quantity = qty;
    }
}
