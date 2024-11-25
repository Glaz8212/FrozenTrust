using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType 
{
    Wood, Ore, Fruit, Meat, Weapon
}

// [System.Serializable]
public class Item : MonoBehaviour
{
    public ItemType itemType;
    private ItemObj itemObj;
    [SerializeField] ItemObj returnObj;
    [SerializeField] List<ItemObj> returnPoolList;
    // public string itemName;
    // public Sprite itemImg;

    private void Start()
    {
        itemObj = gameObject.GetComponent<ItemObj>();
        returnObj= itemObj.returnObj;
        returnPoolList = itemObj.returnPoolList;
    }

    /// <summary>
    /// ������ ������ ��� �������θ� ��ȯ
    /// </summary>
    /// <returns></returns>
    public bool UseItme()
    {
        return false;
    }

    public void Update()
    {
        // ���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveItem();
        }
    }
    /// <summary>
    /// �������� �κ��丮�� �ִ� �Լ�
    /// </summary>
    public void SaveItem()
    {
        // TODO: ���� �����ϴ� ���� �ƴ� ������ �κ��丮�� ���� ������ ������ �ʿ�
        //       ����� Test������ �����ϰ� �ۼ�
        Debug.Log("�������� �����߽��ϴ�.");
        itemObj.ReturnItem(returnObj, returnPoolList); 
        // ù��°�� ������Ʈ Ǯ���� �����ϴ� ������Ʈ�� returnObj, returnPoolList�� ����� �������� ����.
        // Destroy(gameObject);
    }
}
