using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    /// <summary>
    /// �������� �κ��丮�� �ִ� �Լ�
    /// </summary>
    public void SaveItem()
    {
        // TODO: ���� �����ϴ� ���� �ƴ� ������ �κ��丮�� ���� ������ ������ �ʿ�
        //       ����� Test������ �����ϰ� �ۼ�
        Debug.Log("�������� �����߽��ϴ�.");
        Destroy(gameObject);
    }
}
