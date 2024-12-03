using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBox : MonoBehaviour
{
    public bool IsUIOpen;
    [SerializeField] GameObject UI_ItemBox;
    public void MissionBoxOpen()
    {
        Debug.Log("�̼ǻ��� ����");
        IsUIOpen = true;
        UI_ItemBox.SetActive(true);
    }
    public void MissionBoxClose()
    {
        Debug.Log("�̼ǻ��� �ݱ�");
        IsUIOpen = false;
        UI_ItemBox.SetActive(false);
    }

}
