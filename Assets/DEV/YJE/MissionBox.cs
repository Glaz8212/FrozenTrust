using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBox : MonoBehaviour
{
    public bool IsUIOpen;
    [SerializeField] GameObject UI_ItemBox;
    public void MissionBoxOpen()
    {
        Debug.Log("미션상자 열기");
        IsUIOpen = true;
        UI_ItemBox.SetActive(true);
    }
    public void MissionBoxClose()
    {
        Debug.Log("미션상자 닫기");
        IsUIOpen = false;
        UI_ItemBox.SetActive(false);
    }

}
