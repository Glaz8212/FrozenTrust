using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventoryList : MonoBehaviour
{
    BoxInventory boxInventory;
    [SerializeField] public BoxInventory[] boxInventorylist;

    private void Start()
    {
        // 자식오브젝트의 갯수만큼 배열 크기 설정
        boxInventorylist = new BoxInventory[gameObject.transform.childCount];
        for (int i = 0; i < boxInventorylist.Length; i++)
        {
            // 자식오브젝트에서 BoxInventory 컴포넌트를 배열로 생성
            boxInventory = gameObject.transform.GetChild(i).GetComponent<BoxInventory>();
            boxInventorylist[i] = boxInventory;
        }
    }
}
