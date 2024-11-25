using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] public List<ItemObj> woodList;
    [SerializeField] public ItemObj spawnItem; // 스폰할 아이템
    [SerializeField] public List<ItemObj> spawnItemPool; // 스폰할 아이템 리스트

    private void Start()
    {
        woodList = GameObject.Find("WoodPool").GetComponent<ItemPool>().itemPool;
    }

    public void WoodSpawn()
    {

    }

}
