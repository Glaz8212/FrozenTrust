using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    public ItemPool returnPool;
    [SerializeField] List<ItemObj> itemPool;

    [SerializeField] public List<ItemObj> woodItemPool;
    [SerializeField] public List<ItemObj> oreItemPool;
    [SerializeField] public List<ItemObj> fruitItemPool;
    [SerializeField] public List<ItemObj> meatItemPool;

    List<ItemObj> returnPoolList;
    ItemObj returnObj;

    private void OnEnable()
    {
        
    }
    public void ReturnFire(ItemObj returnObj, List<ItemObj> itemPool)
    {
        Debug.Log("회수시작");
        returnPool.RetrunItemPool(returnObj, itemPool);
    }
}
