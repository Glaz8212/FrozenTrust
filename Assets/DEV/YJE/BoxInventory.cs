using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventory : MonoBehaviourPun//, IPunObservable
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// 아이템 박스 인벤토리
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
        if (inventory.Count != 0)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemData.itemName == itemName)
                {
                    if (inventory[i].itemData.itemCount < 4)
                    {
                        curItemData = inventory[i];
                    }
                }
            }
        }

        if (curItemData != null) // 현재 아이템이 이미 있으면
        {
            curItemData.itemData.itemCount += 1;
            Debug.Log($"인벤토리 아이템 갯수 : {curItemData.itemData.itemName} : {curItemData.itemData.itemCount} ");
            UpdateItem(curItemData);/////////
        }
        else if (inventory.Count <= size) // 아이템 박스 사이즈 보다 용량이 적은 경우
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            //curItemData.itemData.itemCount = 1; // 초기 값 지정
            Debug.LogError($"{curItemData.itemData.itemName} : {curItemData.itemData.itemCount}");
            Debug.Log("인벤토리 추가");
            inventory.Add(curItemData); // 아이템 추가
            Debug.Log("UI생성");
            CreateItemUI(curItemData);
            Debug.Log("생성한 게임 오브젝트 반납");
            DeleteItemObject(itemName);
            Debug.LogError("삭제 완료");
        }
        else // 인벤토리가 가득 찬 경우
        {
            return;
        }
    }

    /* 20241130 ver.
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
            curItem.itemData.itemCount += 1; /////////
            UpdateItem(curItem);

        }
        else if (inventory.Count <= size) // 아이템 박스 사이즈 보다 용량이 적은 경우
        {
            // 관련 아이템을 생성 -> 삽입 -> 삭제
            switch (itemName)
            {
                case "Wood":
                    curItem = woodItemData;
                    break;
                case "Ore":
                    Debug.Log("광석을 만들기");
                    curItem = oreItemData;
                    break;
                case "Fruit":
                    Debug.Log("열매를 만들기");
                    curItem = fruitItemData;
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
    */

    /*
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
    */

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
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemData.itemName == itemName)
            {
                if (inventory[i].itemData.itemCount <= 4)
                {
                    curItemData = inventory[i];
                    Debug.LogWarning($"{inventory[i].itemData.itemCount}");
                    Debug.LogWarning($"{curItemData.itemData.itemCount}");
                }
            }
        }

        if (curItemData != null) // 아이템이 있으면
        {
            Debug.Log("아이템 갯수 감소");
            curItemData.itemData.itemCount -= 1; // 한개 제거
            Debug.LogError($"인벤토리 아이템 갯수 : {curItemData.itemData.itemName} : {curItemData.itemData.itemCount} ");
            if (curItemData.itemData.itemCount <= 0) // 0 이하인 경우
            {
                Debug.Log(curItemData.itemData.gameObject.name);
                //UpdateItem(curItemData);///////////
                //curItemData.itemData.itemCount = 1; // 갯수 기본값으로 회복
                Destroy(curItemData.itemPrefab.gameObject);/////////
                Debug.Log("인벤토리에서 삭제");
                inventory.Remove(curItemData); // 리스트에서 아이템 제외
            }
            else
            {
                UpdateItem(curItemData);
                Debug.LogError($"인벤토리 아이템 갯수 : {curItemData.itemData.itemName} : {curItemData.itemData.itemCount} ");
            }
        }
    }

    /* 20241130 ver.
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
                Destroy(boxItem.itemPrefab.gameObject); // UI를 모두의 화면에서 제거/////////
                UpdateItem(boxItem);
            }
            else
            {
                UpdateItem(boxItem);
            }
        }
    }
    */
    /*
        [PunRPC]
        public void UpdateItem(string name, int itemCount)
        {
            Debug.Log("아이템 업데이트");
            int arrayNum = 0;
            switch (name)
            {
                case "Wood":
                    arrayNum = 0;
                    break;
                case "Ore":
                    arrayNum = 1;
                    break;
                case "Fruit":
                    arrayNum = 2;
                    break;
            }
            Debug.Log("아이템UI설정 실행 준비 - 각 개인별 전부 실행여부.....확인필요.....");
            nowUIChaged.GetComponent<ItemPrefab>().SetItemUI(arrayNum, itemCount);
        }

    */


    // 아이템 업데이트
    public void UpdateItem(ItemData item)
    {
        if (item.itemPrefab != null)////////
        {
            Debug.LogError("아이템 업데이트");

            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        }
    }

    public void CreateItemUI(ItemData item)
    {

        GameObject itemUI = Instantiate(itemPrefabObj, itemContent);
        ItemPrefab itemPrefab = itemUI.GetComponent<ItemPrefab>();
        Debug.Log("UI 자료 세팅");
        itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        item.itemPrefab = itemPrefab;
    }

    /* public void CreateItemUI(string itemName) // RPC로 기본 데이터형만 받아올 수 있음 공통실행함수
     {
         Debug.Log("이름에 맞는 아이템 데이터 생성");
         GameObject curObject = null;
         Item curItem = null;
         ItemData curItemData = null;

         GameObject newItemUI = null;
         ItemPrefab itemUI = null;
         // 관련 아이템을 생성 -> 삽입 -> 삭제
         switch (itemName)
         {
             case "Wood":
                 Debug.Log("목재를 만들기");
                 curObject = itemController.MakeWoodItem();
                 curItem = curObject.GetComponent<Item>();
                 curItemData = new ItemData(curItem);
                 newItemUI = Instantiate(itemPrefab, itemContent);
                 itemUI = newItemUI.GetComponent<ItemPrefab>();

                 Debug.Log("UI 자료 세팅");
                 //photonView.RPC("SetItemUIRPC", RpcTarget.All, 0, curItemData.itemData.itemCount);
                 itemUI.SetItemUI(curItemData.itemData.itemSprite, curItemData.itemData.itemName, curItemData.itemData.itemCount);
                 Debug.Log("나무 아이템 제거");
                 itemController.ResetWoodItem();
                 break;
             case "Ore":
                 Debug.Log("광석을 만들기");
                 curObject = itemController.MakeOreItem();
                 curItem = curObject.GetComponent<Item>();
                 curItemData = new ItemData(curItem);
                 newItemUI = Instantiate(itemPrefab, itemContent);
                 itemUI = newItemUI.GetComponent<ItemPrefab>();

                 Debug.Log("UI 자료 세팅");
                 // photonView.RPC("SetItemUIRPC", RpcTarget.All, 1, curItemData.itemData.itemCount);
                 itemUI.SetItemUI(curItemData.itemData.itemSprite, curItemData.itemData.itemName, curItemData.itemData.itemCount);
                 Debug.Log("광석 아이템 제거");
                 itemController.ResetOreItem();
                 break;
             case "Fruit":
                 Debug.Log("열매를 만들기");
                 curObject = itemController.MakeFruitItem();
                 curItem = curObject.GetComponent<Item>();
                 curItemData = new ItemData(curItem);
                 newItemUI = Instantiate(itemPrefab, itemContent);
                 itemUI = newItemUI.GetComponent<ItemPrefab>();

                 Debug.Log("UI 자료 세팅");
                 // photonView.RPC("SetItemUIRPC", RpcTarget.All, 3, curItemData.itemData.itemCount);
                 itemUI.SetItemUI(curItemData.itemData.itemSprite, curItemData.itemData.itemName, curItemData.itemData.itemCount);
                 Debug.Log("광석 아이템 제거");
                 itemController.ResetFruitItem();
                 break;
             default:
                 break;
         }
     }
    */
    /// <summary>
    /// 이름에 맞는 아이템 데이터 사용을 위해서 분기하여 오브젝트를 빌려오는 함수
    /// </summary>
    /// <param name="itemName"></param>
    public GameObject MakeItemObject(string itemName)
    {
        Debug.Log("이름에 맞는 아이템 데이터 생성");
        GameObject curObject = null;
        /*
        Item curItem = null;
        ItemData curItemData = null;

        GameObject newItemUI = null;
        ItemPrefab itemUI = null;
        */
        switch (itemName)
        {
            case "Wood":
                Debug.Log("목재를 만들기");
                curObject = itemController.MakeWoodItem();
                return curObject;

            //curItem = curObject.GetComponent<Item>();
            //curItemData = new ItemData(curItem);

            //newItemUI = Instantiate(itemPrefab, itemContent);
            //itemUI = newItemUI.GetComponent<ItemPrefab>();

            // Debug.Log("UI 자료 세팅");
            //itemUI.SetItemUI(curItemData.itemData.itemSprite, curItemData.itemData.itemName, curItemData.itemData.itemCount);
            //Debug.Log("나무 아이템 제거");
            //itemController.ResetWoodItem();
            case "Ore":
                Debug.Log("광석을 만들기");
                curObject = itemController.MakeOreItem();
                return curObject;

            //curItem = curObject.GetComponent<Item>();
            //curItemData = new ItemData(curItem);
            //newItemUI = Instantiate(itemPrefab, itemContent);
            //itemUI = newItemUI.GetComponent<ItemPrefab>();

            //Debug.Log("UI 자료 세팅");
            // itemUI.SetItemUI(curItemData.itemData.itemSprite, curItemData.itemData.itemName, curItemData.itemData.itemCount);
            //Debug.Log("광석 아이템 제거");
            //itemController.ResetOreItem();
            case "Fruit":
                Debug.Log("열매를 만들기");
                curObject = itemController.MakeFruitItem();
                return curObject;

            //curItem = curObject.GetComponent<Item>();
            //curItemData = new ItemData(curItem);


            //newItemUI = Instantiate(itemPrefab, itemContent);
            //itemUI = newItemUI.GetComponent<ItemPrefab>();

            //Debug.Log("UI 자료 세팅");
            //itemUI.SetItemUI(curItemData.itemData.itemSprite, curItemData.itemData.itemName, curItemData.itemData.itemCount);
            //Debug.Log("광석 아이템 제거");
            //itemController.ResetFruitItem();

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



    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(inventory);
        }
        else
        {
            this.inventory = (List<ItemData>)stream.ReceiveNext();
        }
    }
    */
}
