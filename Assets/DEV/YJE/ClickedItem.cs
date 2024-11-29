using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedItem : MonoBehaviourPun
{
    [Header("현재 선택한 버튼 정보")]
    public GameObject nowClicked;
    public ItemPrefab nowItemUI;

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
        Debug.Log("버튼 클릭");
        nowClicked = EventSystem.current.currentSelectedGameObject;
        nowItemUI = nowClicked.GetComponent<ItemPrefab>();
        Debug.Log($"아이템 이름 : {nowItemUI.itemNameText.text}");
        Debug.Log($"아이템 갯수 : {nowItemUI.itemQuantityText.text}");

        BoxController boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController 참조가 된 경우
        {
            Debug.Log("BoxController를 찾았습니다.");
            // Box의 인벤토리 UI가 닫혀있는 경우 작동 x
            if (boxController.IsUIOpen == false)
            {
                Debug.Log("박스가 오픈되지 않았습니다.");
                return;
            }
            // Box의 인벤토리 UI가 열려있는 경우 추가
            else if (boxController.IsUIOpen == true)
            {
                photonView.RPC("AddBox", RpcTarget.All, nowItemUI.itemNameText.text);
                Debug.Log("ItemBox에 공공의 함수로 추가");
                playerInventory.RemoveItem(nowItemUI.itemNameText.text, 1);
                Debug.Log("개인 Inventory에서 제거");
                return;
            }

        }
        else
        {
            Debug.Log("BoxController가 없습니다.");
            return;
        }
    }

    /// <summary>
    /// Box에 있는 Item Prefab에서 Button을 클릭했을 때 Onclick 이벤트로 발생
    /// - Item Box에서 PlayerInventory로 아이템 추가하는 함수
    /// </summary>
    public void BoxAddPlayer()
    {
        Debug.Log("버튼 클릭");
        nowClicked = EventSystem.current.currentSelectedGameObject;
        nowItemUI = nowClicked.GetComponent<ItemPrefab>();

        // 무조건 플레이어와 접촉하여 아이템 박스가 열려있는 상태이므로
        Debug.Log("ItemBox에 공공의 함수로 제거");
        Debug.Log("개인 Inventory에 추가");

    }
}
