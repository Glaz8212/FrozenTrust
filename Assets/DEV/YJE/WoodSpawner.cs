using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] public ItemPool itemPool;
    [SerializeField] public ItemObj spawnItem; // 스폰할 아이템
    [SerializeField] public List<ItemObj> spawnItemPool; // 스폰할 아이템 리스트

    private void Awake()
    {
        itemPool = GameObject.Find("WoodPool").GetComponent<ItemPool>();
        spawnItemPool = GameObject.Find("WoodPool").GetComponent<ItemPool>().itemPool;
    }

    public void WoodSpawn(Transform startItem)
    {
        Debug.Log("오브젝트풀에서 생성");
        Vector3 itemPos = new Vector3(startItem.position.x, startItem.position.y + 1, startItem.position.y); 
        // 오브젝트의 위치가 제대로 생성되지 않음
        spawnItem = itemPool.GetItemPool(itemPos, Quaternion.identity, spawnItemPool);
        spawnItem.gameObject.SetActive(true);
        spawnItemPool.RemoveAt(spawnItemPool.Count - 1);
        /*
        spawnItem.gameObject.SetActive(true);
        spawnItem.gameObject.transform.position = new Vector3(startItem.position.x, startItem.position.y + 2, startItem.position.z);
        spawnItemPool.RemoveAt(spawnItemPool.Count - 1);
        */
    }

}
