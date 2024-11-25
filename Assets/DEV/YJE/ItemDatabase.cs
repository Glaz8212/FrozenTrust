using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // static으로 데이터베이스 선언...?
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }

    // 아이템 리스트 생성
    public List<Item> itemDB = new List<Item>();
}
