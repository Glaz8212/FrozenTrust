using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedItem : MonoBehaviourPun
{
    [Header("현재 선택한 버튼 정보")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("참조할 스크립트")]
    [SerializeField] GameSceneManager gameSceneManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] BoxController boxController;
    [SerializeField] BoxInventory boxInventory;
    [SerializeField] PhotonView photonView;
    [SerializeField] PlayerInventory playerInventory;

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
        /*
        // player와 상호작용한 BoxConroller.cs 참조
        boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        photonView = boxInventory.GetComponent<PhotonView>();*/
    }

    /// <summary>
    /// Player Inventory의 Item Prefab에서 Button을 클릭했을 때 Onclick 이벤트로 발생
    /// - PlayerInventory에서 Item Box로 아이템 추가하는 함수
    /// </summary>
    public void PlayerAddBox()
    {
        // 현재 버튼을 클릭한 게임 오브젝트
        nowClicked = EventSystem.current.currentSelectedGameObject.gameObject;
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
                if (nowItemPrefab.itemNameText.text == "Fruit")
                {
                    // TODO: 플레이어의 허기를 올려준다
                    Debug.LogError("플레이어의 허기를 증가");
                    playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);

                }
                Debug.Log("직접 사용할 수 없습니다.");
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
        // 현재 버튼을 클릭한 게임 오브젝트
        nowClicked = EventSystem.current.currentSelectedGameObject.gameObject; ////
        // 오브젝트의 ItemPrefab.cs 참조
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();

        // 상호작용한 버튼의 부모가 가진 BoxInventroy.cs 참조
        boxInventory = nowClicked.GetComponentInParent<BoxInventory>(); ////
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        GameObject curObject = boxInventory.MakeItemObject(nowItemPrefab.itemNameText.text);
        Item curItem = curObject.GetComponent<Item>();
        ItemData curItemData = new ItemData(curItem);

        /*
                // TODO : ItemBox에 있는 boxInventory를 참조할것
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("마스터클라이언트 한사람만");
                    Debug.Log("선택된 아이템으로 ItemData 생성");
                    curObject = FindItem(nowItemPrefab.itemNameText.text);
                }
        */

        Debug.Log("개인 Inventory에 추가");
        playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, curItemData.itemData.itemCount);

        // 무조건 플레이어와 접촉하여 아이템 박스가 열려있는 상태이므로
        Debug.Log("ItemBox에 공공의 함수로 제거");
        photonView.RPC("SubBox", RpcTarget.All, curItemData.itemData.itemName);
        boxInventory.DeleteItemObject(nowItemPrefab.itemNameText.text);
        /*
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("마스터클라이언트 한사람만");
                    Debug.Log("아이템삭제");
                    PhotonNetwork.Destroy(curObject);
                }
        */
    }
    /*
    private GameObject FindItem(string itemName)
    {
        GameObject curObject = null;
        // 관련 아이템을 생성 -> 삽입 -> 삭제
        switch (itemName)
        {
            // boxItem 프리팹에 PhotonView 추가 후 실행
            case "Wood":
                Debug.Log("목재를 만들기");
                curObject = PhotonNetwork.InstantiateRoomObject("YJE/Wood", new Vector3(0, 0, 0), Quaternion.identity);
                //woodItemData;
                return curObject;
            case "Ore":
                Debug.Log("광석을 만들기");
                curObject = PhotonNetwork.InstantiateRoomObject("YJE/Ore", new Vector3(0, 0, 0), Quaternion.identity);
                //oreItemData;
                return curObject;
            case "Fruit":
                Debug.Log("열매를 만들기");
                curObject = PhotonNetwork.InstantiateRoomObject("YJE/Fruit", new Vector3(0, 0, 0), Quaternion.identity);
                //fruitItemData;
                return curObject;
            default:
                break;
        }
        return null;
    }
    */
}
