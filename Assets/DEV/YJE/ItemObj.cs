using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    public ItemPool returnPool;

    public List<ItemObj> returnPoolList;
    public ItemObj returnObj;

    private void OnEnable()
    {
        returnObj = GameObject.Find("SM_Env_Tree_02_Snow").GetComponent<ResourceController>().spawnItem;
        returnPoolList = GameObject.Find("SM_Env_Tree_02_Snow").GetComponent<ResourceController>().spawnItemPool;

    }

    public void ReturnItem(ItemObj returnObj, List<ItemObj> itemPool)
    {
        Debug.Log("회수시작");
        returnPool.RetrunItemPool(returnObj, itemPool);
    }
}
