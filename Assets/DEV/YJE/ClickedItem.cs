using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedItem : MonoBehaviourPun
{
    [Header("���� ������ ��ư ����")]
    public GameObject nowClicked;
    public ItemPrefab nowItemUI;

    [Header("������ ��ũ��Ʈ")]
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
    }

    /// <summary>
    /// Player Inventory�� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - PlayerInventory���� Item Box�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void PlayerAddBox()
    {
        Debug.Log("��ư Ŭ��");
        nowClicked = EventSystem.current.currentSelectedGameObject;
        nowItemUI = nowClicked.GetComponent<ItemPrefab>();
        Debug.Log($"������ �̸� : {nowItemUI.itemNameText.text}");
        Debug.Log($"������ ���� : {nowItemUI.itemQuantityText.text}");

        BoxController boxController = playerInteraction.boxController;
        boxInventory = boxController.GetComponentInChildren<BoxInventory>();
        PhotonView photonView = boxInventory.GetComponent<PhotonView>();

        if (boxController != null) // boxController ������ �� ���
        {
            Debug.Log("BoxController�� ã�ҽ��ϴ�.");
            // Box�� �κ��丮 UI�� �����ִ� ��� �۵� x
            if (boxController.IsUIOpen == false)
            {
                Debug.Log("�ڽ��� ���µ��� �ʾҽ��ϴ�.");
                return;
            }
            // Box�� �κ��丮 UI�� �����ִ� ��� �߰�
            else if (boxController.IsUIOpen == true)
            {
                photonView.RPC("AddBox", RpcTarget.All, nowItemUI.itemNameText.text);
                Debug.Log("ItemBox�� ������ �Լ��� �߰�");
                playerInventory.RemoveItem(nowItemUI.itemNameText.text, 1);
                Debug.Log("���� Inventory���� ����");
                return;
            }

        }
        else
        {
            Debug.Log("BoxController�� �����ϴ�.");
            return;
        }
    }

    /// <summary>
    /// Box�� �ִ� Item Prefab���� Button�� Ŭ������ �� Onclick �̺�Ʈ�� �߻�
    /// - Item Box���� PlayerInventory�� ������ �߰��ϴ� �Լ�
    /// </summary>
    public void BoxAddPlayer()
    {
        Debug.Log("��ư Ŭ��");
        nowClicked = EventSystem.current.currentSelectedGameObject;
        nowItemUI = nowClicked.GetComponent<ItemPrefab>();

        // ������ �÷��̾�� �����Ͽ� ������ �ڽ��� �����ִ� �����̹Ƿ�
        Debug.Log("ItemBox�� ������ �Լ��� ����");
        Debug.Log("���� Inventory�� �߰�");

    }
}
