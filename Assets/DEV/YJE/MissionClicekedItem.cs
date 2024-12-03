using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionClicekedItem : MonoBehaviour
{
    [Header("���� ������ ��ư ����")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("������ ��ũ��Ʈ")]
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
        // ��Ʈ��ũ �غ� �ʿ��� �ð� ����
        yield return new WaitForSeconds(3f);
        SetPlayerInventory();
    }
    /// <summary>
    /// ������ �÷��̾��κ��丮�ϰ� ��ȣ�ۿ��ؾ��ϹǷ� PlayerInventroy.cs ����
    /// </summary>
    private void SetPlayerInventory()
    {

        // �ڽ� ��Ʈ�ѷ��� �ҷ����� ���� ������ �÷��̾��� PlayerInteration.cs ����
        playerInteraction = GameSceneManager.Instance.nowPlayer.GetComponentInParent<PlayerInteraction>();
        // playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();
        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();

        // ������ �θ� ������Ʈ�� �ִ� MissionBoxInventoryList.cs����
        missionInventoryList = gameObject.transform.GetComponentInParent<MissionInventoryList>();
        // boxInventoryList = gameObject.transform.GetComponentInParent<BoxInventoryList>();

    }
    /// <summary>
    /// MissionBox�� �ִ� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - ������� ��쿡�� ��µǾ����
    /// </summary>
    public void MissionBoxSteal()
    {
        if (GameManager.Instance.playerRole == 1)
        {
            Debug.Log("����� �ν�");
            // ���� ��ư�� Ŭ���� ���� ������Ʈ
            nowClicked = EventSystem.current.currentSelectedGameObject.gameObject;
            // ������Ʈ�� ItemPrefab.cs ����
            nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();
            missionBoxInventory = missionInventoryList.missionInventoryList[transform.GetSiblingIndex()];

            PhotonView photonView = missionBoxInventory.GetComponent<PhotonView>();

            GameObject curObject = missionBoxInventory.MakeItemObject(nowItemPrefab.itemNameText.text);
            Item curItem = curObject.GetComponent<Item>();
            ItemData curItemData = new ItemData(curItem);


            Debug.Log("���� Inventory�� �߰�");
            playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1);

            // ������ �÷��̾�� �����Ͽ� ������ �ڽ��� �����ִ� �����̹Ƿ�
            Debug.Log("Mission �ڽ����� ������ �Լ��� ����");
            photonView.RPC("MissionSteal", RpcTarget.All, curItemData.itemData.itemName);
            missionBoxInventory.DeleteItemObject(nowItemPrefab.itemNameText.text);

        }
        else
        {
            Debug.Log("����ڰ� �ƴմϴ�.");
            return;
        }
        
    }
}
