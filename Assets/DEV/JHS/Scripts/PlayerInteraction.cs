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

    public MissionController missionController;
    public BoxController BoxController;
    public ItemController itemController;

    private void Update()
    {
        // E Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && isCollider == true) //&& �ҷ��� ��ȣ�ۿ� �Լ� != null )
        {
            isInteracting = true; // ��ȣ�ۿ� �� ���·� ����

            // ��ȣ�ۿ� �Լ� ȣ��
            switch (type)
            {
                case Type.Idle:
                    break;
                case Type.Misson:
                    Misson();
                    break;
                case Type.ItemBox:
                    ItemBox();
                    break;
                case Type.Item:
                    Item();
                    break;
            }
        }
        // ��ȣ�ۿ��� ������Ʈ���� ��ȣ�ۿ��� �������� false���� ���� �Ѱ� �����;ߵ�
        // �� ���� false����� else if ���ǿ� �־���ߵ� 
        else if (isCollider == false || missionController.IsUIOpen == false || BoxController.IsUIOpen == false) // || �Ǵ� ��ȣ�ۿ� â�� �ݾ����� 
        {
            isInteracting = false;
        }
    }

    private void Misson()
    {
        // missionController.MissionBoxOpen();
    }

    private void ItemBox()
    {
        BoxController.BoxOpen();
    }

    private void Item()
    {
        itemController.SaveItem();
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Misson1") || other.CompareTag("Misson2") || other.CompareTag("Misson1"))
        {
            missionController = other.GetComponent<MissionController>();
            type = Type.Misson;
        }
        else if (other.CompareTag("ItemBox"))
        {
            BoxController = other.GetComponent<BoxController>();
            
            type = Type.ItemBox;
        }
        else if (other.CompareTag("Item"))
        {
            itemController = other.GetComponent<ItemController>();
            type = Type.Item;
        }
        else
        {
            Debug.Log("��ȣ�ۿ� �Ҽ��ִ� ������Ʈ�� �ƴմϴ�");
        }

        isCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // �浹�� ����Ǹ� ��ȣ�ۿ� ���� �ʱ�ȭ ����
        isCollider = false;
    }
}
