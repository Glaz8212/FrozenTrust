using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] public List<ItemObj> itemPool = new List<ItemObj>();
    [SerializeField] public int poolSize; // ����Ʈ ������
    [SerializeField] ItemObj prefab; // ������ ������ ������
    [SerializeField] Transform itemPos;


    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ItemObj item = Instantiate(prefab);
            item.gameObject.SetActive(false);
            item.transform.parent = transform;
            itemPool.Add(item);
        }
        itemPos = gameObject.transform.GetComponentInParent<ResourceController>().startItem;
    }

    public ItemObj GetItemPool(Vector3 pos, Quaternion rotation, List<ItemObj> items)
    {
        if(items.Count > 0)
        {

        ItemObj showItem = items[items.Count - 1];
        showItem.transform.position = pos;
        showItem.transform.rotation = rotation;
        showItem.transform.parent = null;
        showItem.returnPool = this; // ItemObj.cs���� ��ȯ ������Ʈ ����
        return showItem;

        }
        else
        {
            ItemObj showItem = Instantiate(prefab, pos, rotation);
            return showItem;
        }
    }

    public void RetrunItemPool(ItemObj item, List<ItemObj> itemPool)
    {
        item.gameObject.SetActive(false);
        // item.transform.parent = itemPos;
        itemPool.Add(item);
    }
}
