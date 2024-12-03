using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemController : MonoBehaviour
{
    private ItemPool itemPool = new ItemPool();
    [Header("ItemObj.cs")]
    public ItemObj nowWoodItemObj;
    public ItemObj nowOreItemObj;
    public ItemObj nowFruitItemObj;

    [Header("Item Pool List")]
    [SerializeField] public List<ItemObj> woodPool;
    [SerializeField] public List<ItemObj> orePool;
    [SerializeField] public List<ItemObj> fruitPool;
    private void Start()
    {
        woodPool = GameObject.FindGameObjectWithTag("WoodPool").GetComponent<ItemPool>().itemPools;
        orePool = GameObject.FindGameObjectWithTag("OrePool").GetComponent<ItemPool>().itemPools;
        fruitPool = GameObject.FindGameObjectWithTag("FruitPool").GetComponent<ItemPool>().itemPools;
    }

    public ItemObj MakeItem(ItemObj newItem, List<ItemObj> itemPool)
    {
        newItem.gameObject.SetActive(true);
        itemPool.RemoveAt(itemPool.Count - 1);
        return newItem;
    }
    public GameObject MakeWoodItem()
    {
        nowWoodItemObj = itemPool.MakeItemPool(Vector3.zero, woodPool);
        GameObject curItme = MakeItem(nowWoodItemObj, woodPool).gameObject;
        return curItme;
    }
    public void ResetWoodItem()
    {
        nowWoodItemObj.GetComponent<ItemObj>().ReturnItem(nowWoodItemObj, woodPool);
    }

    public GameObject MakeOreItem()
    {
        nowOreItemObj = itemPool.MakeItemPool(Vector3.zero, orePool);
        GameObject curItme = MakeItem(nowOreItemObj, orePool).gameObject;
        return curItme;
    }
    public void ResetOreItem()
    {
        nowOreItemObj.GetComponent<ItemObj>().ReturnItem(nowOreItemObj, orePool);
    }

    public GameObject MakeFruitItem()
    {
        nowFruitItemObj = itemPool.MakeItemPool(Vector3.zero, fruitPool);
        GameObject curItme = MakeItem(nowFruitItemObj, fruitPool).gameObject;
        return curItme;
    }
    public void ResetFruitItem()
    {
        nowFruitItemObj.GetComponent<ItemObj>().ReturnItem(nowFruitItemObj, fruitPool);
    }
}
