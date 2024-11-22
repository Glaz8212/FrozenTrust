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
        if (other.gameObject.tag == "ItemBox")
        {
            Debug.Log("아이템박스 접촉");
        }
        // 무기와 트리거 발생 시
        if (other.gameObject.tag == "Weapon")
        {
            Debug.Log("무기 접촉");
        }
        // 미션 박스와 트리거 발생 시
        if(other.gameObject.tag == "Mission1")
        {
            // TODO : 테스트를 위한 임의 함수
            Debug.Log("미션1 박스 접촉");
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.transform.GetComponentInParent<MissionController>().Mission1ClearChecked();
            }
        }
        // 미션 박스와 트리거 발생 시
        if (other.gameObject.tag == "Mission2")
        {
            // TODO : 테스트를 위한 임의 함수
            Debug.Log("미션2 박스 접촉");
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.transform.GetComponentInParent<MissionController>().Mission2ClearChecked();
            }
        }
        // 미션 박스와 트리거 발생 시
        if (other.gameObject.tag == "Ending")
        {
            // TODO : 테스트를 위한 임의 함수
            Debug.Log("엔딩 탈출 포트 접촉");
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.transform.GetComponentInParent<MissionController>().EndingClearChecked();
            }
        }
    }
}
