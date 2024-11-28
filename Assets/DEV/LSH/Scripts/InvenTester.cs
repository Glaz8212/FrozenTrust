using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTester : MonoBehaviour
{
    public PlayerInventory playerInventory; // PlayerInventory ��ũ��Ʈ ����
    public Sprite sprite; // �׽�Ʈ�� ��������Ʈ

    private void Update()
    {
        // AŰ�� ������ �߰�
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerInventory.AddItem("Wood", sprite, 1);
        }

        // AŰ�� ������ �߰�
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerInventory.AddItem("ore", sprite, 1);
        }

        // RŰ�� ������ ����
        if (Input.GetKeyDown(KeyCode.R) && playerInventory.inventory.Count > 0)
        {
            playerInventory.RemoveItem("Wood", 1);
        }
    }
}