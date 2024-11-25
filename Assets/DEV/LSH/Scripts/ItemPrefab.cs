using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefab : MonoBehaviour
{
    public Image itemImage; // 스프라이트를 표시하는 이미지
    public Text itemNameText; // 아이템 이름
    public Text itemQuantityText; // 아이템 갯수

    // UI를 업데이트
    public void SetItemUI(Sprite sprite, string name, int quantity)
    {
        itemImage.sprite = sprite;
        itemNameText.text = name;
        itemQuantityText.text = $"x{quantity}";
    }
}
