using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{ 
    public enum Type { Idle, Mission, ItemBox, Item }
    public Type type = Type.Idle;
    // ��ȣ�ۿ� ���� ����
    public bool isInteracting = false;
    // �ݶ��̴� �浹 ����
    private Collider currentCollider;

    public MissionBox missionController;
    public BoxController boxController;
    public Item item;
    public PlayerInventory playerInventory;
    private PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        // Inventory ������Ʈ�� ã�� PlayerInventory �Ҵ�
        if (playerInventory == null)
        {
            GameObject inventoryObject = GameObject.Find("Inventory");
            if (inventoryObject != null)
            {
                playerInventory = inventoryObject.GetComponent<PlayerInventory>();
            }

            // Inventory ������Ʈ�� ã�� �� ���� ��� ��� ���
            if (playerInventory == null)
            {
                Debug.LogError("Inventory ������Ʈ���� PlayerInventory�� ã�� �� �����ϴ�!");
            }
        }
    }
    private void Update()
    {
        // E Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && currentCollider != null && status.playerDie == false)
        {
            isInteracting = true; // ��ȣ�ۿ� �� ���·� ����

            // ��ȣ�ۿ� �Լ� ȣ��
            switch (type)
            {
                case Type.Idle:
                    Debug.Log("��ȣ�ۿ� ������ ���°� �ƴմϴ�.");
                    break;
                case Type.Mission:
                    // null���� ������ ?. missionController�� ���� null�� �ƴҰ�쿡�� MissionBoxOpen�� ����. nulld�̸� MissionBoxOpen�� �������� ����
                    missionController?.MissionBoxOpen();
                    break;
                case Type.ItemBox:
                    boxController?.BoxOpen();
                    break;
                case Type.Item:
                    item?.interaction(playerInventory);
                    ResetInteraction();
                    break;
            }
        }
        // ��ȣ�ۿ��� ������Ʈ���� ��ȣ�ۿ��� �������� false���� ���� �Ѱ� �����;ߵ�
        if (missionController != null && !missionController.IsUIOpen || boxController != null && !boxController.IsUIOpen)
        {
            ResetInteraction();
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        // �ݶ��̴��� ���� �� ������ ����
        if (currentCollider != null) return;

        currentCollider = other;

        if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            missionController = other.GetComponent<MissionBox>();
            // ���� ������ ��� : ���� = ���ǹ� ? ���ǹ��� ���� �� �� : ���ǹ��� ������ �� ��
            type = missionController != null ? Type.Mission : Type.Idle;
        }
        else if (other.CompareTag("ItemBox"))
        {
            boxController = other.GetComponent<BoxController>();
            // Ÿ���� boxController�� ���� �ִٸ� ItemBox �ƴϸ� Idle
            type = boxController != null ? Type.ItemBox : Type.Idle;
        }
        else if (other.CompareTag("Item"))
        {
            item = other.GetComponent<Item>();
            // Ÿ���� item�� ���� ������ Item ������ Idle
            type = item != null ? Type.Item : Type.Idle;
        }
        else
        {
            Debug.Log("��ȣ�ۿ� �Ҽ��ִ� ������Ʈ�� �ƴմϴ�");
            type = Type.Idle;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �ݶ��̴����� ���� ���� currentCollider ���� ���ٸ� ���� #�� �ݶ��̴��� �� ���� �� ��찡 �ֳ�?
        if (other == currentCollider)
        {
            // �̼� �ݶ��̴��� ���� �ְ� �� ���� other�� MissionBox�� ���ٸ� �̼� ui�ݱ� ����
            if (missionController != null && missionController == other.GetComponent<MissionBox>())
            {
                missionController.MissionBoxClose();
            }
            // �ڽ� �ݶ��̴��� ���� �ְ� �� ���� other�� BoxController�� ���ٸ� �ڽ� ui�ݱ� ����
            else if (boxController != null && boxController == other.GetComponent<BoxController>())
            {
                boxController.BoxClose();
            }

            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        type = Type.Idle;
        currentCollider = null;
        missionController = null;
        boxController = null;
        item = null;
        isInteracting = false;
    }
}
