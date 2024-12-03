using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class MissionBoxInventory : MonoBehaviour
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// 아이템 박스 인벤토리
    public List<int> inventoryCount = new List<int>(); // 아이템 개수를 저장하는 리스트
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    [SerializeField] GameObject itemPrefabObj; // 아이템 프리팹
    int size = 3; // 박스 사이즈

    [SerializeField] PlayerInventory playerInventory;

    public int missionWoodCount; // 최종 미션에 필요한 개수 - 아이템 종류별로 다르게 사용
    public int missionOreCount; // 최종 미션에 필요한 개수
    public int missionFruitCount; // 최종 미션에 필요한 개수

    public bool IsEnterChecked = false; 

    // 랜덤으로 미션 개수 설정
    private void Start()
    {
        int num = Random.Range(0, 5);
        missionWoodCount = num;
        num = Random.Range(0, 5);
        missionOreCount = num;
        num = Random.Range(0, 5);
        missionFruitCount = num;
        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();
    }

    /// <summary>
    /// 네트워크 동기함수로 AddBox() 생성
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddMission(string itemName)
    {
        Debug.Log("AddMission RPC함수 정상 실행");
        ItemData curItemData = null;
        int curItemCount = 0;
        int index = 0; // i의 값을 저장하는 index
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
                        break; // 동일한 아이템을 찾은 경우 for문 종료
                    }
                }
            }
        }

        if (curItemData != null) // 현재 아이템이 이미 있으면
        {
            // 각 아이템 별로 필요한 아이템 개수가 전부 찼는지 확인이 필요
            switch (curItemData.itemData.itemName)
            {
                case "Wood":
                    if (curItemCount == missionWoodCount)
                    {
                        Debug.Log("더 이상 넣을 수 없습니다.");
                        IsEnterChecked = true; // 가득 차 있는 경우
                        //playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1); //추가로 삭제되는 것을 방지
                        return;
                    }
                    else
                        break;
                case "Ore":
                    if (curItemCount == missionOreCount)
                    {
                        Debug.Log("더 이상 넣을 수 없습니다.");
                        IsEnterChecked = true; // 가득 차 있는 경우
                        //playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1); //추가로 삭제되는 것을 방지
                        return;
                    }
                    else
                        break;
                case "Fruit":
                    if (curItemCount == missionFruitCount)
                    {
                        Debug.Log("더 이상 넣을 수 없습니다.");
                        IsEnterChecked = true; // 가득 차 있는 경우
                        //playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1); //추가로 삭제되는 것을 방지
                        return;
                    }
                    else
                        break;
                default:
                    break;
            }
            // 더 넣을 수 있으면 추가
            curItemCount += 1;
            inventoryCount[index] = curItemCount; // 인벤토리에 카운트 저장
            Debug.LogWarning($"인벤토리 아이템 수정 후 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            UpdateItem(curItemData, curItemCount); // UI 업데이트
        }
        else if (inventory.Count <= size) // 아이템 박스 사이즈 보다 용량이 적은 경우
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            curItemCount = curItemData.itemData.itemCount; // 개수의 초기 값 지정
            Debug.Log($"{curItemData.itemData.itemName} : {curItemCount}");
            Debug.Log("인벤토리 추가");
            inventory.Add(curItemData); // 아이템 추가
            inventoryCount.Add(curItemCount); // 아이템의 개수 추가
            Debug.Log("UI생성");
            CreateItemUI(curItemData); // 한 개 짜리 생성
            Debug.Log("생성한 게임 오브젝트 반납");
            DeleteItemObject(itemName);
            Debug.Log("삭제 완료");
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
    public void MissionSteal(string itemName)
    {
        // box에 있는 아이템을 찾기
        Debug.LogError("MissionSteal RPC함수 정상 실행");
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
                    break;
                }
            }
        }

        if (curItemData != null) // 아이템이 있으면
        {
            Debug.Log("아이템 갯수 감소");
            curItemCount -= 1; // 한개 제거
            Debug.Log($"인벤토리 아이템 수정 후 갯수 : {curItemData.itemData.itemName} : {curItemCount} ");
            inventoryCount[index] = curItemCount; // 인벤토리에 카운트 저장
            Debug.Log($"{inventoryCount[index]}");
            if (inventoryCount[index] <= 0) // 0 이하인 경우
            {
                Debug.Log(curItemData.itemData.gameObject.name);
                Destroy(curItemData.itemPrefab.gameObject);
                Debug.Log("인벤토리에서 삭제");
                inventory.Remove(curItemData); // 리스트에서 아이템 제외
                inventoryCount.RemoveAt(index); // 리스트에서 인덱스 번호의 위치의 정보 제외
            }
            else
            {
                Debug.Log("UI 갱신");
                UpdateItem(curItemData, curItemCount);
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
            Debug.Log("아이템UI 업데이트");

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
