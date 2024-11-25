using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] public List<ItemObj> woodList;
    [SerializeField] public ItemObj spawnItem; // ������ ������
    [SerializeField] public List<ItemObj> spawnItemPool; // ������ ������ ����Ʈ

    private void Start()
    {
        woodList = GameObject.Find("WoodPool").GetComponent<ItemPool>().itemPool;
    }

    public void WoodSpawn()
    {

    }

}
