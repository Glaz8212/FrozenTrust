using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] public ItemPool itemPool;
    [SerializeField] public ItemObj spawnItem; // ������ ������
    [SerializeField] public List<ItemObj> spawnItemPool; // ������ ������ ����Ʈ

    private void Awake()
    {
        itemPool = GameObject.Find("WoodPool").GetComponent<ItemPool>();
        spawnItemPool = GameObject.Find("WoodPool").GetComponent<ItemPool>().itemPool;
    }

    public void WoodSpawn(Transform startItem)
    {
        Debug.Log("������ƮǮ���� ����");
        Vector3 itemPos = new Vector3(startItem.position.x, startItem.position.y + 1, startItem.position.y); 
        // ������Ʈ�� ��ġ�� ����� �������� ����
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
