using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] public List<ItemObj> fruitList; 
    [SerializeField] public ItemObj spawnItem; // 스폰할 아이템
    [SerializeField] public List<ItemObj> spawnItemPool; // 스폰할 아이템 리스트
    public void FruitSpawn()
    {

    }
}
