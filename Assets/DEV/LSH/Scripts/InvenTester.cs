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
        if (Input.GetKeyDown(KeyCode.A)) // A Ű�� ������ �߰�
        {
            playerInventory.AddItem("Wood", sprite, 1);
        }
    }
}