using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    [SerializeField] public List<ItemObj> oreList; 
    [SerializeField] public ItemObj spawnItem; // 스폰할 아이템
    [SerializeField] public List<ItemObj> spawnItemPool; // 스폰할 아이템 리스트
    public void OreSpawn()
    {

    }
}
