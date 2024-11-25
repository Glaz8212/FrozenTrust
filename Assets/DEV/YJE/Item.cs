using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType 
{
    Wood, Ore, Fruit, Meat, Weapon
}

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemType itemType;
    // public string itemName;
    // public Sprite itemImg;

    /// <summary>
    /// ������ ������ ��� �������θ� ��ȯ
    /// </summary>
    /// <returns></returns>
    public bool UseItme()
    {
        return false;
    }

    /// <summary>
    /// �������� �κ��丮�� �ִ� �Լ�
    /// </summary>
    public void SaveItem()
    {
        // TODO: ���� �����ϴ� ���� �ƴ� ������ �κ��丮�� ���� ������ ������ �ʿ�
        //       ����� Test������ �����ϰ� �ۼ�
        Debug.Log("�������� �����߽��ϴ�.");
        Destroy(gameObject);
    }
}
