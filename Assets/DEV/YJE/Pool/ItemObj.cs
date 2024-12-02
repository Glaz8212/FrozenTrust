using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    public ItemPool itemPool; // ItemPool.cs¿¬µ¿
    [Header("Item Pool")]
    [SerializeField] public List<ItemObj> woodPool;
    [SerializeField] public List<ItemObj> orePool;
    [SerializeField] public List<ItemObj> fruitPool;

    public void ReturnItem(ItemObj itemObj, List<ItemObj> returnItemPool)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        itemPool.ReturnItemPool(itemObj, returnItemPool);
    }
}
