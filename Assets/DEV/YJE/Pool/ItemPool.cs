using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] public List<ItemObj> itemPools = new List<ItemObj>();
    public int size;
    [SerializeField] ItemObj itemPrefab;

    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            ItemObj item = Instantiate(itemPrefab);
            item.gameObject.SetActive(false);
            item.transform.parent = transform;
            itemPools.Add(item);
        }
    }

    public ItemObj MakeItemPool(Vector3 position, List<ItemObj> itemPool)
    {
        if (itemPool.Count > 0)
        {
            ItemObj makeItem = itemPool[itemPool.Count - 1];
            makeItem.transform.position = position;
            makeItem.transform.parent = null;
            makeItem.itemPool = this;
            return makeItem;
        }
        else
        {
            return Instantiate(itemPrefab);
        }
    }

    public void ReturnItemPool(ItemObj item, List<ItemObj> returnItemPool)
    {
        item.gameObject.SetActive(false);
        returnItemPool.Add(item);
    }
}
