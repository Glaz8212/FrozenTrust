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
        // A키로 아이템 추가
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerInventory.AddItem("Wood", sprite, 1);
        }

        // A키로 아이템 추가
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerInventory.AddItem("ore", sprite, 1);
        }

        // R키로 아이템 삭제
        if (Input.GetKeyDown(KeyCode.R) && playerInventory.inventory.Count > 0)
        {
            playerInventory.RemoveItem("Wood", 1);
        }
    }
}