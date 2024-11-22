using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TestPlayer : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // �����۰� Ʈ���� �߻� ��
        if(other.gameObject.tag == "Item")
        {
            Debug.Log("������ ����");
        }
        // ������ �ڽ��� Ʈ���� �߻� ��
        if (other.gameObject.tag == "ItemBox")
        {
            Debug.Log("�����۹ڽ� ����");
        }
        // ����� Ʈ���� �߻� ��
        if (other.gameObject.tag == "Weapon")
        {
            Debug.Log("���� ����");
        }
        // �̼� �ڽ��� Ʈ���� �߻� ��
        if(other.gameObject.tag == "Mission1")
        {
            // TODO : �׽�Ʈ�� ���� ���� �Լ�
            Debug.Log("�̼�1 �ڽ� ����");
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.transform.GetComponentInParent<MissionController>().Mission1ClearChecked();
            }
        }
        // �̼� �ڽ��� Ʈ���� �߻� ��
        if (other.gameObject.tag == "Mission2")
        {
            // TODO : �׽�Ʈ�� ���� ���� �Լ�
            Debug.Log("�̼�2 �ڽ� ����");
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.transform.GetComponentInParent<MissionController>().Mission2ClearChecked();
            }
        }
        // �̼� �ڽ��� Ʈ���� �߻� ��
        if (other.gameObject.tag == "Ending")
        {
            // TODO : �׽�Ʈ�� ���� ���� �Լ�
            Debug.Log("���� Ż�� ��Ʈ ����");
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.transform.GetComponentInParent<MissionController>().EndingClearChecked();
            }
        }
    }
}
