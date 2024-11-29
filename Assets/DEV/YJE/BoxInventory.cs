using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun
{
    public List<ItemData> inventory = new List<ItemData>();// 아이템 박스 인벤토리
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    [SerializeField] GameObject itemPrefab; // 아이템 프리팹
    [SerializeField] int size; // 박스 사이즈
    GameSceneManager gameSceneManager;
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
        ItemData curItem = null;
        foreach (ItemData item in inventory)
        {
            // 같은 종류의 아이템을 추가하려는 경우
            if (item.itemData.itemName == itemName)
            {
                if (item.itemData.itemCount < 4)
                {
                    curItem = item;
                    break;
                }
            }
        }
        if (curItem != null) // 현재 아이템이 없으면
        {
            curItem.itemData.itemCount += 1;
            UpdateItem(curItem);
        }
        else if (inventory.Count <= size) // 아이템 박스 사이즈 보다 용량이 적은 경우
        {
            // 관련 아이템을 생성 -> 삽입 -> 삭제
            switch (itemName)
            {
                case "Wood":
                    Item woodItem = PhotonNetwork.Instantiate("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(woodItem);
                    PhotonNetwork.Destroy(woodItem.gameObject);
                    break;
                case "Ore":
                    Debug.Log("광석을 만들기");
                    Item oreItem = PhotonNetwork.Instantiate("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(oreItem);
                    PhotonNetwork.Destroy(oreItem.gameObject);
                    break;
                case "Fruit":
                    Debug.Log("열매를 만들기");
                    Item fruitItem = PhotonNetwork.Instantiate("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                    curItem = new ItemData(fruitItem);
                    PhotonNetwork.Destroy(fruitItem.gameObject);
                    break;
                default:
                    break;
            }
            inventory.Add(curItem); // 아이템 추가
            CreateItemUI(curItem); // 아이템 UI 추가
        }
        else // 인벤토리가 가득 찬 경우
        {
            return;
        }
    }
    /// <summary>
    /// 네트워크 동기함수로 SubBox() 생성
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void SubBox(string itemName)
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
        if (boxItem != null) // 아이템이 있으면
        {
            boxItem.itemData.itemCount -= 1; // 한개 제거
            if (boxItem.itemData.itemCount <= 0) // 0 이하인 경우
            {
                inventory.Remove(boxItem); // 리스트에서 아이템 제외
                PhotonNetwork.Destroy(boxItem.itemPrefab.gameObject); // UI를 모두의 화면에서 제거
                UpdateItem(boxItem);
            }
            else
            {
                UpdateItem(boxItem);
            }
        }
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
