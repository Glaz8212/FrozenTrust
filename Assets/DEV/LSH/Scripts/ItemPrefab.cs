using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefab : MonoBehaviour
{
    public Image itemImage; // ��������Ʈ�� ǥ���ϴ� �̹���
    public Text itemNameText; // ������ �̸�
    public Text itemQuantityText; // ������ ����

    // UI�� ������Ʈ
    public void SetItemUI(Sprite sprite, string name, int quantity)
    {
        itemImage.sprite = sprite;
        itemNameText.text = name;
        itemQuantityText.text = $"x{quantity}";
    }
}
