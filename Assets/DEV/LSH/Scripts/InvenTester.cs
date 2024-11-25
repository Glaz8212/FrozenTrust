using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTester : MonoBehaviour
{
    public PlayerInventory playerInventory; // PlayerInventory 스크립트 참조
    public Sprite sprite; // 테스트용 스프라이트

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // A 키로 아이템 추가
        {
            playerInventory.AddItem("Wood", sprite, 1);
        }
    }
}