using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Wood, Ore, Fruit, Weapon
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImg;

    /// <summary>
    /// ������ ��� �������θ� ��ȯ
    /// </summary>
    /// <returns></returns>
    public bool Use()
    {
        return false;
    }
}
