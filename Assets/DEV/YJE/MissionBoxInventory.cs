using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class MissionBoxInventory : MonoBehaviourPun, IPunObservable
{
    [SerializeField] ItemController itemController;

    public List<ItemData> inventory = new List<ItemData>();// ������ �ڽ� �κ��丮
    public List<int> inventoryCount = new List<int>(); // ������ ������ �����ϴ� ����Ʈ
    [SerializeField] RectTransform itemContent; // �������� ���� �� ��ġ
    [SerializeField] GameObject itemPrefabObj; // ������ ������
    int size = 3; // �ڽ� ������

    [SerializeField] PlayerInventory playerInventory;

    public int missionWoodCount; // ���� �̼ǿ� �ʿ��� ���� - ������ �������� �ٸ��� ���
    public int missionOreCount; // ���� �̼ǿ� �ʿ��� ����
    public int missionFruitCount; // ���� �̼ǿ� �ʿ��� ����

    public bool IsEnterChecked;

    public bool IsWoodChecked = false;
    public bool IsOreChecked = false;
    public bool IsFruitChecked = false;

    // �������� �̼� ���� ����
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            missionWoodCount = Random.Range(1, 5);
            missionOreCount = Random.Range(1, 5);
            missionFruitCount = Random.Range(1, 5);

            /* �ƹ��͵� ���� 0���� ��� Ȯ�� ���� �߻� - �������� ������ Ȯ��
             do
            {
                missionWoodCount = Random.Range(0, 5);
                missionOreCount = Random.Range(0, 5);
                missionFruitCount = Random.Range(0, 5);
            }
            while (missionWoodCount == 0 && missionOreCount == 0 && missionFruitCount == 0);*/
        }
        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            // �̼� ���� ���θ� �Ǵ��ϴ� switch ��
            switch (inventory[i].itemData.itemName)
            {
                case "Wood":
                    if (inventoryCount[i] == missionWoodCount)
                    {
                        IsWoodChecked = true;
                    }
                    else
                    {
                        Debug.LogWarning("������ �ٸ�");
                        IsWoodChecked = false;
                    }
                    break;

                case "Ore":
                    if (inventoryCount[i] == missionOreCount)
                    {
                        IsOreChecked = true;
                    }
                    else
                    {
                        IsWoodChecked = false;
                        Debug.LogWarning("������ �ٸ�");
                    } 
                    break;

                case "Fruit":
                    if (inventoryCount[i] == missionFruitCount)
                    {
                        IsFruitChecked = true;
                    }
                    else
                    {
                        IsWoodChecked = false;
                        Debug.LogWarning("������ �ٸ�");
                    } 
                    break;
                default:
                    break;
            }

        }
    }

    /// <summary>
    /// Mission�� Item ����ȭ
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(missionWoodCount);
            stream.SendNext(missionOreCount);
            stream.SendNext(missionFruitCount);

            stream.SendNext(IsWoodChecked);
            stream.SendNext(IsOreChecked);
            stream.SendNext(IsFruitChecked);
        }
        else if (stream.IsReading)
        {
            missionWoodCount = (int)stream.ReceiveNext();
            missionOreCount = (int)stream.ReceiveNext();
            missionFruitCount = (int)stream.ReceiveNext();

            IsWoodChecked = (bool)stream.ReceiveNext();
            IsOreChecked = (bool)stream.ReceiveNext();
            IsFruitChecked = (bool)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// ��Ʈ��ũ �����Լ��� AddMission() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void AddMission(string itemName)
    {
        IsEnterChecked = false;
        Debug.Log("AddMission RPC�Լ� ���� ����");
        ItemData curItemData = null;
        int curItemCount = 0;
        int index = 0; // i�� ���� �����ϴ� index
        if (inventory.Count != 0)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemData.itemName == itemName)
                {
                    if (inventoryCount[i] < 4) // inventoryCount ����Ʈ�� ���尳���� ���� ����
                    {
                        curItemData = inventory[i];
                        curItemCount = inventoryCount[i];
                        index = i;
                        break; // ������ �������� ã�� ��� for�� ����
                    }
                }
            }
        }

        if (curItemData != null) // ���� �������� �̹� ������
        {
            // �� ������ ���� �ʿ��� ������ ������ ���� á���� Ȯ���� �ʿ�
            switch (curItemData.itemData.itemName)
            {
                case "Wood":
                    if (curItemCount == missionWoodCount)
                    {
                        Debug.Log("���� �ϼ�");
                        IsEnterChecked = true; // ���� �� �ִ� ���
                        return;
                    }
                    break;
                case "Ore":
                    if (curItemCount == missionOreCount)
                    {
                        Debug.Log("�� �ϼ�");
                        IsEnterChecked = true; // ���� �� �ִ� ���
                        return;
                    }
                    break;
                case "Fruit":
                    if (curItemCount == missionFruitCount)
                    {
                        Debug.Log("���� �ϼ�");
                        IsEnterChecked = true; // ���� �� �ִ� ���
                        return;
                    }
                    break;
                default:
                    break;
            }
            // �� ���� �� ������ �߰�
            curItemCount += 1;
            inventoryCount[index] = curItemCount; // �κ��丮�� ī��Ʈ ����
            Debug.LogWarning($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            UpdateItem(curItemData, curItemCount); // UI ������Ʈ
        }
        else if (inventory.Count <= size) // ������ �ڽ� ������ ���� �뷮�� ���� ���
        {
            GameObject curObject = MakeItemObject(itemName);
            Item curItem = curObject.GetComponent<Item>();
            curItemData = new ItemData(curItem);
            curItemCount = curItemData.itemData.itemCount; // ������ �ʱ� �� ����
            Debug.Log($"{curItemData.itemData.itemName} : {curItemCount}");
            Debug.Log("�κ��丮 �߰�");
            inventory.Add(curItemData); // ������ �߰�
            inventoryCount.Add(curItemCount); // �������� ���� �߰�
            Debug.Log("UI����");
            CreateItemUI(curItemData); // �� �� ¥�� ����
            Debug.Log("������ ���� ������Ʈ �ݳ�");
            DeleteItemObject(itemName);
            Debug.Log("���� �Ϸ�");
        }
        else // �κ��丮�� ���� �� ���
        {
            return;
        }

    }
    /// <summary>
    /// ��Ʈ��ũ �����Լ��� MissionSteal() ����
    /// </summary>
    /// <param name="itemName"></param>
    [PunRPC]
    public void MissionSteal(string itemName)
    {
        // box�� �ִ� �������� ã��
        Debug.LogError("MissionSteal RPC�Լ� ���� ����");
        ItemData curItemData = null;
        int curItemCount = 0;
        int index = 0; // i�� ���� ����
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemData.itemName == itemName)
            {
                if (inventoryCount[i] <= 4) // �������� ���� ������ 4 ����
                {
                    curItemData = inventory[i];
                    curItemCount = inventoryCount[i];
                    index = i; // �κ��丮 ���� �� ���
                    break;
                }
            }
        }

        if (curItemData != null) // �������� ������
        {
            Debug.Log("������ ���� ����");
            curItemCount -= 1; // �Ѱ� ����
            Debug.Log($"�κ��丮 ������ ���� �� ���� : {curItemData.itemData.itemName} : {curItemCount} ");
            inventoryCount[index] = curItemCount; // �κ��丮�� ī��Ʈ ����
            Debug.Log($"{inventoryCount[index]}");

            if (inventoryCount[index] <= 0) // 0 ������ ���
            {
                Debug.Log(curItemData.itemData.gameObject.name);
                Destroy(curItemData.itemPrefab.gameObject);
                Debug.Log("�κ��丮���� ����");
                inventory.Remove(curItemData); // ����Ʈ���� ������ ����
                inventoryCount.RemoveAt(index); // ����Ʈ���� �ε��� ��ȣ�� ��ġ�� ���� ����
            }
            else
            {
                Debug.Log("UI ����");
                UpdateItem(curItemData, curItemCount);
            }
        }
    }

    /// <summary>
    /// ������UI�� ������Ʈ
    /// </summary>
    /// <param name="item"></param>
    /// <param name="curItemCount"></param>
    public void UpdateItem(ItemData item, int curItemCount)
    {
        if (item.itemPrefab != null)////////
        {
            Debug.Log("������UI ������Ʈ");

            item.itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, curItemCount);
        }
    }

    /// <summary>
    /// ó�� ������ �������� UI�� ����
    /// </summary>
    /// <param name="item"></param>
    public void CreateItemUI(ItemData item)
    {

        GameObject itemUI = Instantiate(itemPrefabObj, itemContent);
        ItemPrefab itemPrefab = itemUI.GetComponent<ItemPrefab>();
        Debug.Log("UI �ڷ� ����");
        itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, item.itemData.itemCount);
        item.itemPrefab = itemPrefab;
    }

    /// <summary>
    /// �̸��� �´� ������ ������ ����� ���ؼ� �б��Ͽ� ������Ʈ�� �������� �Լ�
    /// </summary>
    /// <param name="itemName"></param>
    public GameObject MakeItemObject(string itemName)
    {
        Debug.Log("�̸��� �´� ������ ������ ����");
        GameObject curObject = null;
        switch (itemName)
        {
            case "Wood":
                Debug.Log("���縦 �����");
                curObject = itemController.MakeWoodItem();
                return curObject;

            case "Ore":
                Debug.Log("������ �����");
                curObject = itemController.MakeOreItem();
                return curObject;

            case "Fruit":
                Debug.Log("���Ÿ� �����");
                curObject = itemController.MakeFruitItem();
                return curObject;

            default:
                return null;
        }
    }
    /// <summary>
    /// �̸��� �´� ������ ������ ������ ���ؼ� �б��Ͽ� ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="itemName"></param>
    public void DeleteItemObject(string itemName)
    {
        Debug.Log("������Ʈ ���� �Լ�");
        switch (itemName)
        {
            case "Wood":
                Debug.Log("���� ������ ����");
                itemController.ResetWoodItem();
                break;
            case "Ore":
                Debug.Log("���� ������ ����");
                itemController.ResetOreItem();
                break;
            case "Fruit":
                Debug.Log("���� ������ ����");
                itemController.ResetFruitItem();
                break;
            default:
                break;
        }
    }
}
