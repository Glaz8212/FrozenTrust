using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TestPlayer : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // 아이템과 트리거 발생 시
        if(other.gameObject.tag == "Item")
        {
            Debug.Log("아이템 접촉");
        }
        // 아이템 박스와 트리거 발생 시
        // TODO : Finish 태그를 ItemBox로 변경할 것
        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("아이템박스 접촉");
        }
        // 무기와 트리거 발생 시
        if (other.gameObject.tag == "Weapon")
        {
            Debug.Log("무기 접촉");
        }
        // 미션 박스와 트리거 발생 시
        // TODO : Finish 태그를 MissionBox로 변경할 것
        if(other.gameObject.tag == "GameController")
        {
            Debug.Log("미션 박스 접촉");
        }
    }
}
