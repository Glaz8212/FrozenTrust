using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBox : MonoBehaviour
{
    public bool IsUIOpen;
    public void MissionBoxOpen()
    {
        Debug.Log("�̼ǻ��� ����");
        IsUIOpen = true;
        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = ture; - return��
    }
    public void MissionBoxClose()
    {
        Debug.Log("�̼ǻ��� �ݱ�");
        IsUIOpen = false;
        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = false; - return��
    }

}
