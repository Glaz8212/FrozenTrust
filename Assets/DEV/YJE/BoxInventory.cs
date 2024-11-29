using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>();// 아이템 박스 인벤토리
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    [SerializeField] GameObject itemPrefab; // 아이템 프리팹
    [SerializeField] int size; // 박스 사이즈
    /*[SerializeField] Item itemWood;
    [SerializeField] Item itemOre;
    [SerializeField] Item itemFruit;*/
    GameSceneManager gameSceneManager;
    /*
    private void Start()
    {
        Debug.Log("상자채우기 실행");
        StartCoroutine(StartDelayRoutine());
        gameObject.SetActive(false);
    }
    IEnumerator StartDelayRoutine()
    {
        // 네트워크 준비에 필요한 시간 설정
        yield return new WaitForSeconds(5f);
        Debug.Log("쉬었음 채우기시작");
        SetItemList();
    }
    private void SetItemList()
    {
        for (int i = 0; i < size; i++)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    Debug.Log("나무 추가");
                    //photonView.RPC("AddBox", RpcTarget.All, itemWood.itemName, itemWood.itemSprite, itemWood.itemCount);
                    AddBox(itemWood.itemName, itemWood.itemSprite, itemWood.itemCount);
                    break;
                case 1:
                    Debug.Log("돌 추가");
                    //photonView.RPC("AddBox", RpcTarget.All, itemOre.itemName, itemOre.itemSprite, itemOre.itemCount);
                    AddBox(itemOre.itemName, itemOre.itemSprite, itemOre.itemCount);
                    break;
                case 2:
                    Debug.Log("열매 추가");
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
    /// 네트워크 동기함수로 AddBox() 생성
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddBox(string itemName)
    {
        Debug.Log("AddBox실행");
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // 같은 종류의 아이템
            if (item.itemData.itemName == itemName)
            {
                Debug.Log("이미있는 아이템");
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
                    Debug.Log("나무를 만들기");
                    Item woodItem = PhotonNetwork.Instantiate("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(woodItem);
                    break;
                case "Ore":
                    Debug.Log("광석을 만들기");
                    Item oreItem = PhotonNetwork.Instantiate("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(oreItem);
                    break;
                case "Fruit":
                    Debug.Log("열매를 만들기");
                    Item fruitItem = PhotonNetwork.Instantiate("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(fruitItem);
                    break;
                default:
                    break;
            }
            Debug.Log("아이템 리스트 추가");
            inventory.Add(curItem);
            CreateItemUI(curItem);
        }
        else
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
        }
    }
    /*
    public void AddBox(string itemName, Sprite sprite, int quantity)
    {
        Debug.Log("AddBox실행");
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // 같은 종류의 아이템
            if (item.itemData.itemName == itemName)
            {
                Debug.Log("이미있는 아이템");
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
            Debug.Log("아이템 리스트 추가");
            inventory.Add(newItemData);
            CreateItemUI(newItemData);
        }
        else
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
        }
    }
    */

    [PunRPC]
    public bool SubBox(string itemName, int itemCount)
    {
        ItemData boxItem = null;
        // box에 있는 아이템을 찾기
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


    // 아이템 업데이트
    public void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            Debug.Log("아이템 업데이트");
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
