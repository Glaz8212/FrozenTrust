using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionClicekedItem : MonoBehaviour
{
    [Header("현재 선택한 버튼 정보")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("참조할 스크립트")]
    [SerializeField] PlayerInteraction playerInteraction;
    // [SerializeField] BoxController boxController;
    // [SerializeField] BoxInventoryList boxInventoryList;
    [SerializeField] MissionInventoryList missionInventoryList;
    [SerializeField] MissionBoxInventory missionBoxInventory;
    // [SerializeField] BoxInventory boxInventory;
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
        playerInteraction = GameSceneManager.Instance.nowPlayer.GetComponentInParent<PlayerInteraction>();
        // playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();
        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();

        // 현재의 부모 오브젝트에 있는 MissionBoxInventoryList.cs참조
        missionInventoryList = gameObject.transform.GetComponentInParent<MissionInventoryList>();
        // boxInventoryList = gameObject.transform.GetComponentInParent<BoxInventoryList>();

    }
    /// <summary>
    /// MissionBox에 있는 Item Prefab에서 Button을 클릭했을 때 Onclick 이벤트로 발생
    /// - 배신자의 경우에만 출력되어야함
    /// </summary>
    public void MissionBoxSteal()
    {
        if (GameManager.Instance.playerRole == 1)
        {
            Debug.Log("배신자 인식");
            // 현재 버튼을 클릭한 게임 오브젝트
            nowClicked = EventSystem.current.currentSelectedGameObject.gameObject;
            // 오브젝트의 ItemPrefab.cs 참조
            nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();
            missionBoxInventory = missionInventoryList.missionInventoryList[transform.GetSiblingIndex()];

            PhotonView photonView = missionBoxInventory.GetComponent<PhotonView>();

            GameObject curObject = missionBoxInventory.MakeItemObject(nowItemPrefab.itemNameText.text);
            Item curItem = curObject.GetComponent<Item>();
            ItemData curItemData = new ItemData(curItem);


            Debug.Log("개인 Inventory에 추가");
            playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1);

            // 무조건 플레이어와 접촉하여 아이템 박스가 열려있는 상태이므로
            Debug.Log("Mission 박스에서 공공의 함수로 제거");
            photonView.RPC("MissionSteal", RpcTarget.All, curItemData.itemData.itemName);
            missionBoxInventory.DeleteItemObject(nowItemPrefab.itemNameText.text);

        }
        else
        {
            Debug.Log("배신자가 아닙니다.");
            return;
        }
        
    }
}
