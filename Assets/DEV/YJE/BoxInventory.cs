using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>();// ������ �ڽ� �κ��丮
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefab; // ������ ������
    [SerializeField] int size; // �ڽ� ������
    /*[SerializeField] Item itemWood;
    [SerializeField] Item itemOre;
    [SerializeField] Item itemFruit;*/
    GameSceneManager gameSceneManager;
    /*
    private void Start()
    {
        Debug.Log("����ä��� ����");
        StartCoroutine(StartDelayRoutine());
        gameObject.SetActive(false);
    }
    IEnumerator StartDelayRoutine()
    {
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(5f);
        Debug.Log("������ ä������");
        SetItemList();
    }
    private void SetItemList()
    {
        for (int i = 0; i < size; i++)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    Debug.Log("���� �߰�");
                    //photonView.RPC("AddBox", RpcTarget.All, itemWood.itemName, itemWood.itemSprite, itemWood.itemCount);
                    AddBox(itemWood.itemName, itemWood.itemSprite, itemWood.itemCount);
                    break;
                case 1:
                    Debug.Log("�� �߰�");
                    //photonView.RPC("AddBox", RpcTarget.All, itemOre.itemName, itemOre.itemSprite, itemOre.itemCount);
                    AddBox(itemOre.itemName, itemOre.itemSprite, itemOre.itemCount);
                    break;
                case 2:
                    Debug.Log("���� �߰�");
                    //photonView.RPC("AddBox", RpcTarget.All, itemFruit.itemName, itemFruit.itemSprite, itemFruit.itemCount);
                    AddBox(itemFruit.itemName, itemFruit.itemSprite, itemFruit.itemCount);
                    break;
                default:
                    break;
            }
        }
    }
    */

    private void Awake()
    {
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
    }

    /// <summary>
    /// ��Ʈ��ũ �����Լ��� AddBox() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddBox(string itemName)
    {
        Debug.Log("AddBox����");
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ ������
            if (item.itemData.itemName == itemName)
            {
                Debug.Log("�̹��ִ� ������");
                if (item.itemData.itemCount < 4)
                {
                    curItem = item;
                    break;
                }
            }
        }
        if (curItem != null)
        {
            curItem.itemData.itemCount += 1;
            UpdateItem(curItem);
        }
        else if (inventory.Count <= size)
        {
            switch (itemName)
            {
                case "Wood":
                    Debug.Log("������ �����");
                    Item woodItem = PhotonNetwork.Instantiate("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(woodItem);
                    break;
                case "Ore":
                    Debug.Log("������ �����");
                    Item oreItem = PhotonNetwork.Instantiate("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(oreItem);
                    break;
                case "Fruit":
                    Debug.Log("���Ÿ� �����");
                    Item fruitItem = PhotonNetwork.Instantiate("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(fruitItem);
                    break;
                default:
                    break;
            }
            Debug.Log("������ ����Ʈ �߰�");
            inventory.Add(curItem);
            CreateItemUI(curItem);
        }
        else
        {
            Debug.Log("�κ��丮�� ���� á���ϴ�.");
        }
    }
    /*
    public void AddBox(string itemName, Sprite sprite, int quantity)
    {
        Debug.Log("AddBox����");
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // ���� ������ ������
            if (item.itemData.itemName == itemName)
            {
                Debug.Log("�̹��ִ� ������");
                if (item.itemData.itemCount < 4)
                {
                    curItem = item;
                    break;
                }
            }
        }
        if (curItem != null)
        {
            curItem.itemData.itemCount += quantity;
            UpdateItem(curItem);
        }
        else if (inventory.Count <= size)
        {
            Item newItem = new Item(itemName, sprite, quantity);
            ItemData newItemData = new ItemData(newItem);
            Debug.Log("������ ����Ʈ �߰�");
            inventory.Add(newItemData);
            CreateItemUI(newItemData);
        }
        else
        {
            Debug.Log("�κ��丮�� ���� á���ϴ�.");
        }
    }
    */

    [PunRPC]
    public bool SubBox(string itemName, int itemCount)
    {
        ItemData boxItem = null;
        // box�� �ִ� �������� ã��
        foreach (ItemData item in inventory)
        {
            if (item.itemData.itemName == itemName)
            {
                boxItem = item;
                break;
            }
        }
        if (boxItem != null)
        {
            boxItem.itemData.itemCount -= itemCount;
            if (boxItem.itemData.itemCount <= 0)
            {
                inventory.Remove(boxItem);
                PhotonNetwork.Destroy(boxItem.itemPrefab.gameObject);
            }
            else
            {
                UpdateItem(boxItem);
            }
            return true;
        }
        return false;

    }


    // ������ ������Ʈ
    public void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            Debug.Log("������ ������Ʈ");
            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }

    public void CreateItemUI(ItemData item)
    {
        GameObject newItem = Instantiate(itemPrefab, itemContent);

        ItemPrefab itemUI = newItem.GetComponent<ItemPrefab>();
        itemUI.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);

        item.itemPrefab = itemUI;
    }
}
