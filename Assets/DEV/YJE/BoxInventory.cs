using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun//, IPunObservable
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// 아이템 박스 인벤토리
    public List<int> inventoryCount = new List<int>(); //*********************************
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    [SerializeField] GameObject itemPrefabObj; // 아이템 프리팹
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
        Debug.Log("AddBox  RPC함수 정상 실행");
        ItemData curItemData = null;
        int curItemCount = 0;
        int index = 0; // i의 값을 저장
        if (inventory.Count != 0)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemData.itemName == itemName)
                {
                    if (inventoryCount[i] < 4) // inventoryCount 리스트에 저장개수만 따로 저장
                    {
                        curItemData = inventory[i];
                        curItemCount = inventoryCount[i];
                        index = i;
                        break;
                    }
                }
            }
        }

        if (curItemData != null) // 현재 아이템이 이미 있으면
        {
            Debug.LogWarning($"인벤토리 아이템 수정 전 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            curItemCount += 1;//***************************
            inventoryCount[index] = curItemCount; // 인벤토리에 카운트 저장
            Debug.LogWarning($"인벤토리 아이템 수정 후 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            UpdateItem(curItemData, curItemCount);//********************************************
        }
        else if (inventory.Count <= size) // 아이템 박스 사이즈 보다 용량이 적은 경우
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            curItemCount = curItemData.itemData.itemCount; // 초기 값 지정
            Debug.LogError($"{curItemData.itemData.itemName} : {curItemCount}");
            Debug.Log("인벤토리 추가");
            inventory.Add(curItemData); // 아이템 추가
            inventoryCount.Add(curItemCount); // 아이템의 개수 추가
            Debug.Log("UI생성");
            CreateItemUI(curItemData); // 한 개 짜리 생성
            Debug.Log("생성한 게임 오브젝트 반납");
            DeleteItemObject(itemName);
            Debug.LogError("삭제 완료");
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
        // box에 있는 아이템을 찾기
        Debug.LogError("SubBox RPC함수 정상 실행");
        ItemData curItemData = null;
        int curItemCount = 0;
        int index = 0; // i의 값을 저장
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemData.itemName == itemName)
            {
                if (inventoryCount[i] <= 4) // 아이템의 저장 개수가 4 이하
                {
                    curItemData = inventory[i];
                    curItemCount = inventoryCount[i];
                    index = i; // 인벤토리 삭제 시 사용

                    Debug.LogWarning($"{inventory[i].itemData.itemCount}");
                    Debug.LogWarning($"{inventoryCount[i]}");
                    Debug.LogWarning($"{i}");
                    break;
                }
            }
        }

        if (curItemData != null) // 아이템이 있으면
        {
            Debug.Log("아이템 갯수 감소");
            Debug.LogError($"인벤토리 아이템 수정 전 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            curItemCount -= 1; // 한개 제거
            Debug.LogError($"인벤토리 아이템 수정 후 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            inventoryCount[index] = curItemCount; // 인벤토리에 카운트 저장
            if (curItemCount <= 0) // 0 이하인 경우
            {
                Debug.Log(curItemData.itemData.gameObject.name);
                Destroy(curItemData.itemPrefab.gameObject);
                Debug.Log("인벤토리에서 삭제");
                inventory.Remove(curItemData); // 리스트에서 아이템 제외
                inventoryCount.RemoveAt(index); // 리스트에서 인덱스 번호의 위치의 개수 제외****************
            }
            else
            {
                UpdateItem(curItemData, curItemCount);
                Debug.LogError($"인벤토리 아이템 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            }
        }
    }

    /// <summary>
    /// 아이템UI의 업데이트
    /// </summary>
    /// <param name="item"></param>
    /// <param name="curItemCount"></param>
    public void UpdateItem(ItemData item, int curItemCount)
    {
        if (item.itemPrefab != null)////////
        {
            Debug.LogError("아이템 업데이트");

            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, curItemCount);
        }
    }

    /// <summary>
    /// 처음 생성한 아이템의 UI를 생성
    /// </summary>
    /// <param name="item"></param>
    public void CreateItemUI(ItemData item)
    {

        GameObject itemUI = Instantiate(itemPrefabObj, itemContent);
        ItemPrefab itemPrefab = itemUI.GetComponent<ItemPrefab>();
        Debug.Log("UI 자료 세팅");
        itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        item.itemPrefab = itemPrefab;
    }

    /// <summary>
    /// 이름에 맞는 아이템 데이터 사용을 위해서 분기하여 오브젝트를 빌려오는 함수
    /// </summary>
    /// <param name="itemName"></param>
    public GameObject MakeItemObject(string itemName)
    {
        Debug.Log("이름에 맞는 아이템 데이터 생성");
        GameObject curObject = null;
        switch (itemName)
        {
            case "Wood":
                Debug.Log("목재를 만들기");
                curObject = itemController.MakeWoodItem();
                return curObject;
     
            case "Ore":
                Debug.Log("광석을 만들기");
                curObject = itemController.MakeOreItem();
                return curObject;

            case "Fruit":
                Debug.Log("열매를 만들기");
                curObject = itemController.MakeFruitItem();
                return curObject;

            default:
                return null;
        }
    }
    /// <summary>
    /// 이름에 맞는 아이템 데이터 삭제를 위해서 분기하여 오브젝트를 삭제하는 함수
    /// </summary>
    /// <param name="itemName"></param>
    public void DeleteItemObject(string itemName)
    {
        Debug.Log("오브젝트 삭제 함수");
        switch (itemName)
        {
            case "Wood":
                Debug.Log("나무 아이템 제거");
                itemController.ResetWoodItem();
                break;
            case "Ore":
                Debug.Log("광석 아이템 제거");
                itemController.ResetOreItem();
                break;
            case "Fruit":
                Debug.Log("열매 아이템 제거");
                itemController.ResetFruitItem();
                break;
            default:
                break;
        }
    }
}
