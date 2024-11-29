using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedItem : MonoBehaviourPun
{
    [Header("현재 선택한 버튼 정보")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("참조할 스크립트")]
    public GameSceneManager gameSceneManager;
    public PlayerInteraction playerInteraction;
    public BoxInventory boxInventory;
    public PlayerInventory playerInventory;

    private void Start()
    {
        StartCoroutine(StartDelayRoutine());
    }
    IEnumerator StartDelayRoutine()
    {
        // 네트워크 준비에 필요한 시간 설정
        yield return new WaitForSeconds(3f);
        SetPlayerInventory();
    }
    /// <summary>
    /// 개인의 플레이어인벤토리하고만 상호작용해야하므로 PlayerInventroy.cs 참조
    /// </summary>
    private void SetPlayerInventory()
    {
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        // 박스 컨트롤러를 불러오기 위한 생성된 플레이어의 PlayerInteration.cs 참조
        playerInteraction = gameSceneManager.nowPlayer.GetComponent<PlayerInteraction>();
        playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();
    }

    /// <summary>
    /// Player Inventory의 Item Prefab에서 Button을 클릭했을 때 Onclick 이벤트로 발생
    /// - PlayerInventory에서 Item Box로 아이템 추가하는 함수
    /// </summary>
    public void PlayerAddBox()
    {
        // 현재 버튼을 클릭한 게임 오브젝트
        nowClicked = EventSystem.current.currentSelectedGameObject;
        // 오브젝트의 ItemPrefab.cs 참조
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();

        // player와 상호작용한 BoxConroller.cs 참조
        BoxController boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController 참조가 된 경우
        {
            // Box의 인벤토리 UI가 닫혀있는 경우 작동 x
            if (boxController.IsUIOpen == false)
            {
                return;
            }
            // Box의 인벤토리 UI가 열려있는 경우 추가
            else if (boxController.IsUIOpen == true)
            {
                // BoxInventory.cs의 RPC함수로 AddBox실행
                photonView.RPC("AddBox", RpcTarget.All, nowItemPrefab.itemNameText.text);
                playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                return;
            }
        }
        else // 참조한 BoxController.cs가 없는 경우
        {
            return;
        }
    }


    /*
    /// <summary>
    /// Player Inventory의 Item Prefab에서 Button을 클릭했을 때 Onclick 이벤트로 발생
    /// - PlayerInventory에서 Item Box로 아이템 추가하는 함수
    /// </summary>
    public void PlayerAddBox()
    {
        // 현재 버튼을 클릭한 게임 오브젝트
        nowClicked = EventSystem.current.currentSelectedGameObject;
        // 오브젝트의 ItemPrefab.cs 참조
        nowItemUI = nowClicked.GetComponent<ItemPrefab>();

        // player와 상호작용한 BoxConroller.cs 참조
        BoxController boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController 참조가 된 경우
        {
            // Box의 인벤토리 UI가 닫혀있는 경우 작동 x
            if (boxController.IsUIOpen == false)
            {
                return;
            }
            // Box의 인벤토리 UI가 열려있는 경우 추가
            else if (boxController.IsUIOpen == true)
            {
                // BoxInventory.cs의 RPC함수로 AddBox실행
                photonView.RPC("AddBox", RpcTarget.All, nowItemUI.itemNameText.text);
                playerInventory.RemoveItem(nowItemUI.itemNameText.text, 1);
                return;
            }
        }
        else // 참조한 BoxController.cs가 없는 경우
        {
            return;
        }
    }
   */
    /// <summary>
    /// Box에 있는 Item Prefab에서 Button을 클릭했을 때 Onclick 이벤트로 발생
    /// - Item Box에서 PlayerInventory로 아이템 추가하는 함수
    /// </summary>
    public void BoxAddPlayer()
    {
        Debug.Log("버튼 클릭");
        nowClicked = EventSystem.current.currentSelectedGameObject;
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();

        // TODO : ItemBox에 있는 boxInventory를 참조할것
        Debug.Log("선택된 아이템으로 ItemData 생성");
        ItemData curItem = FindItem(nowItemPrefab.itemNameText.text);
        Debug.Log("개인 Inventory에 추가");
        playerInventory.AddItem(curItem.itemData.itemName, curItem.itemData.itemSprite, curItem.itemData.itemCount);

        // 무조건 플레이어와 접촉하여 아이템 박스가 열려있는 상태이므로
        Debug.Log("ItemBox에 공공의 함수로 제거");
        photonView.RPC("SubBox", RpcTarget.All, nowItemPrefab.itemNameText.text);

        Debug.Log("아이템삭제");
        PhotonNetwork.Destroy(curItem.itemData.gameObject);
    }

    private ItemData FindItem(string itemName)
    {
        ItemData itemData = null;
        // 관련 아이템을 생성 -> 삽입 -> 삭제
        switch (itemName)
        {
            case "Wood":
                Item woodItem = PhotonNetwork.Instantiate("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                itemData = new ItemData(woodItem);
                return itemData;
            case "Ore":
                Debug.Log("광석을 만들기");
                Item oreItem = PhotonNetwork.Instantiate("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                itemData = new ItemData(oreItem);
                return itemData;
            case "Fruit":
                Debug.Log("열매를 만들기");
                Item fruitItem = PhotonNetwork.Instantiate("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Item>();
                itemData = new ItemData(fruitItem);
                return itemData;
            default:
                break;
        }
        return null;
    }
}
