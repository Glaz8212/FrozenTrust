using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // static���� �����ͺ��̽� ����...?
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }

    // ������ ����Ʈ ����
    public List<Item> itemDB = new List<Item>();
}
