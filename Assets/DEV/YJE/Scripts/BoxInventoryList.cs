using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventoryList : MonoBehaviour
{
    BoxInventory boxInventory;
    [SerializeField] public BoxInventory[] boxInventorylist;

    private void Start()
    {
        // �ڽĿ�����Ʈ�� ������ŭ �迭 ũ�� ����
        boxInventorylist = new BoxInventory[gameObject.transform.childCount];
        for (int i = 0; i < boxInventorylist.Length; i++)
        {
            // �ڽĿ�����Ʈ���� BoxInventory ������Ʈ�� �迭�� ����
            boxInventory = gameObject.transform.GetChild(i).GetComponent<BoxInventory>();
            boxInventorylist[i] = boxInventory;
        }
    }
}
