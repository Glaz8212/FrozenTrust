using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerStatus;

public class PlayerClickedItem : MonoBehaviour
{
    [Header("현재 선택한 버튼 정보")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("참조할 스크립트")]
    [SerializeField] PlayerInteraction playerInteraction;
    private PlayerStatus playerStatus;

    // ItmeBoxList 오브젝트에 있는 BoxInventroyList.cs
    [SerializeField] BoxController boxController;
    [SerializeField] BoxInventoryList boxInventoryList;
    [SerializeField] BoxInventory boxInventory;

    [SerializeField] MissionBox missionBoxController;
    [SerializeField] MissionInventoryList missionInventoryList;
    [SerializeField] MissionBoxInventory missionBoxInventory;

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
        // 박스 컨트롤러를 불러오기 위한 생성된 플레이어의 PlayerInteration.cs 참조
        playerInteraction = GameSceneManager.Instance.nowPlayer.GetComponent<PlayerInteraction>();

        playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();
        // playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();

        // 현재의 부모 오브젝트에 있는 BoxInventoryList.cs참조
        boxInventoryList = GameObject.Find("ItemBoxList").GetComponent<BoxInventoryList>();//*******************
        //boxInventoryList = GameObject.FindGameObjectWithTag("ItemBoxList").GetComponent<BoxInventoryList>();//*******************
        missionInventoryList = GameObject.Find("MissionController").GetComponent<MissionInventoryList>();//*******************
        //missionInventoryList = GameObject.FindGameObjectWithTag("MissionController").GetComponent<MissionInventoryList>();//*******************
        
        playerStatus = GameSceneManager.Instance.nowPlayer.GetComponent<PlayerStatus>();
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
        boxController = playerInteraction.boxController;
        missionBoxController = playerInteraction.missionController;
        // boxController 참조가 된 경우
        if (boxController != null) 
        {
            Debug.Log(boxController.gameObject.transform.GetSiblingIndex());
            boxInventory = boxInventoryList.boxInventorylist[boxController.gameObject.transform.GetSiblingIndex()];
            PhotonView photonView = boxInventory.GetComponent<PhotonView>();

            // Box의 인벤토리 UI가 닫혀있는 경우 - 소모 아이템 사용
            if (boxController.IsUIOpen == false)
            {
                if (nowItemPrefab.itemNameText.text == "Fruit" || nowItemPrefab.itemNameText.text == "Meat" )
                {
                    playerStatus.HealHunger(200f);
                    Debug.Log("플레이어의 허기를 증가");
                    playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                }

                Debug.Log("직접 사용할 수 없습니다.");
                return;
            }
            // Box의 인벤토리 UI가 열려있는 경우 박스에 아이템 추가
            else if (boxController.IsUIOpen == true)
            {
                // BoxInventory.cs의 RPC함수로 AddBox실행
                photonView.RPC("AddBox", RpcTarget.All, nowItemPrefab.itemNameText.text);
                playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                return;
            }
        }

        // 미션박스가 있는 경우
        else if(missionBoxController != null)
        {
            Debug.Log(missionBoxController.gameObject.transform.GetSiblingIndex());
            missionBoxInventory = missionInventoryList.missionInventoryList[missionBoxController.gameObject.transform.GetSiblingIndex()];
            PhotonView photonView = missionBoxInventory.GetComponent<PhotonView>();

            // Box의 인벤토리 UI가 닫혀있는 경우 - 소모 아이템 사용
            if (missionBoxController.IsUIOpen == false)
            {
                if (nowItemPrefab.itemNameText.text == "Fruit" || nowItemPrefab.itemNameText.text == "Meat")
                {
                    playerStatus.HealHunger(200f);
                    Debug.Log("플레이어의 허기를 증가");
                    playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                }

                Debug.Log("직접 사용할 수 없습니다.");
                return;
            }
            // Box의 인벤토리 UI가 열려있는 경우 박스에 아이템 추가
            else if (missionBoxController.IsUIOpen == true)
            {
                // MissionBoxInventory.cs의 RPC함수로 AddMission실행
                photonView.RPC("AddMission", RpcTarget.All, nowItemPrefab.itemNameText.text);
                playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
                return;
            }
        }
        // BoxController.cs가 없는 경우
        else
        {
            if (nowItemPrefab.itemNameText.text == "Fruit" || nowItemPrefab.itemNameText.text == "Meat")
            {
                playerStatus.HealHunger(200f);
                Debug.LogError("플레이어의 허기를 증가");
                playerInventory.RemoveItem(nowItemPrefab.itemNameText.text, 1);
            }
            Debug.Log("직접 사용할 수 없습니다.");
            return;
        }
    }


}
