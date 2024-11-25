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
    // ��ȣ�ۿ� ���� ����
    public bool isInteracting = false;
    // �ݶ��̴� �浹 ����
    private bool isCollider = false;
    private void Update()
    {
        // E Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.E)&& !isInteracting) //&& �ҷ��� ��ȣ�ۿ� �Լ� != null )
        {
            isInteracting = true; // ��ȣ�ۿ� �� ���·� ����
            
            // ��ȣ�ۿ� �Լ� ȣ��
        }
        // ��ȣ�ۿ��� ������Ʈ���� ��ȣ�ۿ��� �������� false���� ���� �Ѱ� �����;ߵ�
        // �� ���� false����� else if ���ǿ� �־���ߵ� 
        else if (isCollider == false) // || �Ǵ� ��ȣ�ۿ� â�� �ݾ����� 
        {
            isInteracting = false; 
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // �浹������ ��ȣ�ۿ� ������ ������Ʈ���� Ȯ��
        isCollider = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // �浹�� ����Ǹ� ��ȣ�ۿ� ���� �ʱ�ȭ ����
        isCollider = false;
    }
}
