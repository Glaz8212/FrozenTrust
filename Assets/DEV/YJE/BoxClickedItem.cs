using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxClickedItem : MonoBehaviour
{
    [Header("���� ������ ��ư ����")]
    public GameObject nowClicked;
    public ItemPrefab nowItemPrefab;

    [Header("������ ��ũ��Ʈ")]
    [SerializeField] GameSceneManager gameSceneManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] BoxController boxController;
    [SerializeField] BoxInventoryList boxInventoryList;
    [SerializeField] BoxInventory boxInventory;
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
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        // �ڽ� ��Ʈ�ѷ��� �ҷ����� ���� ������ �÷��̾��� PlayerInteration.cs ����
        playerInteraction = gameSceneManager.nowPlayer.GetComponent<PlayerInteraction>();
        playerInventory = GameObject.Find("Inventory").GetComponent<PlayerInventory>();

        // ������ �θ� ������Ʈ�� �ִ� BoxInventoryList.cs����
        boxInventoryList = gameObject.transform.GetComponentInParent<BoxInventoryList>();
        // ������ BoxInventory = ������ BoxInventoryLsit �迭�� ���� ������Ʈ�� ���° �ڽĿ�����Ʈ�� ������ ����
        // GetSibligIndex() =  ���� ������Ʈ�� ���°���� int������ ���
        /* Debug.Log(gameObject.name);
         Debug.LogError(transform.GetSiblingIndex());
         boxInventory = boxInventoryList.boxInventorylist[transform.GetSiblingIndex()];*/
    }
    /// <summary>
    /// Box�� �ִ� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - Item Box���� PlayerInventory�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void BoxAddPlayer()
    {
        Debug.Log("��ư Ŭ��");
        // ���� ��ư�� Ŭ���� ���� ������Ʈ
        nowClicked = EventSystem.current.currentSelectedGameObject.gameObject;
        // ������Ʈ�� ItemPrefab.cs ����
        nowItemPrefab = nowClicked.GetComponent<ItemPrefab>();
        Debug.Log(gameObject.name);
        Debug.LogError(transform.GetSiblingIndex());
        boxInventory = boxInventoryList.boxInventorylist[transform.GetSiblingIndex()];
        // ��ȣ�ۿ��� ��ư�� �θ� ���� BoxInventroy.cs ����
        // boxInventory = nowClicked.GetComponentInParent<BoxInventory>(); ////

        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        GameObject curObject = boxInventory.MakeItemObject(nowItemPrefab.itemNameText.text);
        Item curItem = curObject.GetComponent<Item>();
        ItemData curItemData = new ItemData(curItem);


        Debug.Log("���� Inventory�� �߰�");
        playerInventory.AddItem(curItemData.itemData.itemName, curItemData.itemData.itemSprite, 1);//curItemData.itemData.itemCount);

        // ������ �÷��̾�� �����Ͽ� ������ �ڽ��� �����ִ� �����̹Ƿ�
        Debug.Log("ItemBox�� ������ �Լ��� ����");
        photonView.RPC("SubBox", RpcTarget.All, curItemData.itemData.itemName);
        boxInventory.DeleteItemObject(nowItemPrefab.itemNameText.text);

    }
}
