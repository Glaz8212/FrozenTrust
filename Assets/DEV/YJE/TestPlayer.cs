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
        // TODO : Finish �±׸� ItemBox�� ������ ��
        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("�����۹ڽ� ����");
        }
        // ����� Ʈ���� �߻� ��
        if (other.gameObject.tag == "Weapon")
        {
            Debug.Log("���� ����");
        }
        // �̼� �ڽ��� Ʈ���� �߻� ��
        // TODO : Finish �±׸� MissionBox�� ������ ��
        if(other.gameObject.tag == "GameController")
        {
            Debug.Log("�̼� �ڽ� ����");
        }
    }
}
