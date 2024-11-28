using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Mission�̳� ItemBox �ݶ��̴��� �浹 �Ͽ����� 
    // e ��ư�� Ŭ���ϸ� �� �ݶ��̴��� �ִ� ������Ʈ�� �Լ��� ������ ��ȣ�ۿ�
    // ���� �ݶ��̴��� ��ĥ�� �ϳ��� �ݶ��̴����� ��ȣ�ۿ�
    // false���� �;����� �ٸ� ������Ʈ�� ��ȣ�ۿ�

    // ��ȣ�ۿ� �� �� �ִ� ������Ʈ�� �� ��ȣ�ۿ� ��ũ��Ʈ 
    // private ��ũ��Ʈ�� �Լ���;

    public enum Type { Idle, Misson, ItemBox, Item }
    public Type type = Type.Idle;
    // ��ȣ�ۿ� ���� ����
    public bool isInteracting = false;
    // �ݶ��̴� �浹 ����
    private bool isCollider = false;

    public MissionBox missionController;
    public BoxController boxController;
    public Item item;
    public PlayerInventory playerInventory;
    private PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        playerInventory = GetComponent<PlayerInventory>();
    }
    private void Update()
    {
        // E Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && isCollider && status.playerDie == false)
        {
            isInteracting = true; // ��ȣ�ۿ� �� ���·� ����

            // ��ȣ�ۿ� �Լ� ȣ��
            switch (type)
            {
                case Type.Idle:
                    Debug.Log("��ȣ�ۿ� ������ ���°� �ƴմϴ�.");
                    break;
                case Type.Misson:
                    if (missionController != null)
                    {
                        missionController.MissionBoxOpen();
                    }
                    else
                    {
                        Debug.LogWarning("MissionBoxController �������� �ʾҽ��ϴ�.");
                    }
                    break;
                case Type.ItemBox:
                    if (boxController != null)
                    {
                        boxController.BoxOpen();
                    }
                    else
                    {
                        Debug.LogWarning("BoxController�� �������� �ʾҽ��ϴ�.");
                    }
                    break;
                case Type.Item:
                    if (item != null)
                    {
                        // ������ �׽����� interaction�� playerInventory�� �־� ����
                        item.interaction(playerInventory);
                    }
                    else
                    {
                        Debug.LogWarning("item�� �������� �ʾҽ��ϴ�.");
                    }
                    break;
            }
        }
        // ��ȣ�ۿ��� ������Ʈ���� ��ȣ�ۿ��� �������� false���� ���� �Ѱ� �����;ߵ�
        // �� ���� false����� else if ���ǿ� �־���ߵ� 
        else if (!isCollider || (missionController != null && !missionController.IsUIOpen) || (boxController != null && !boxController.IsUIOpen))
        {              
            isInteracting = false;
        }
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            missionController = other.GetComponent<MissionBox>();
            if (missionController != null)
            {
                type = Type.Misson;
            }
        }
        else if (other.CompareTag("ItemBox"))
        {
            boxController = other.GetComponent<BoxController>();
            if (boxController != null)
            {
                type = Type.ItemBox;
            }
        }
        else if (other.CompareTag("Item"))
        {
            item = other.GetComponent<Item>();
            if (item != null)
            {
                type = Type.Item;
            }
        }
        else
        {
            Debug.Log("��ȣ�ۿ� �Ҽ��ִ� ������Ʈ�� �ƴմϴ�");
            type = Type.Idle;
        }

        isCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MissionBox>() == missionController)
        {
            missionController = null;
        }
        else if (other.GetComponent<BoxController>() == boxController)
        {
            boxController.BoxClose();
            boxController = null;
        }
        else if (other.GetComponent<ItemController>() == item)
        {
            item = null;
        }

        type = Type.Idle;
        isCollider = false; // �浹 ���� ����
        isInteracting = false; // ��ȣ�ۿ� ���� �ʱ�ȭ
    }
}
