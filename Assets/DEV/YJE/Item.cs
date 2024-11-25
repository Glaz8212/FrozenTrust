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
    /// 아이템 사용 성공여부를 반환
    /// </summary>
    /// <returns></returns>
    public bool Use()
    {
        return false;
    }
}
