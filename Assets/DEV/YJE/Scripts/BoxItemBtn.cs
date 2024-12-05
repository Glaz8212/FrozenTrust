using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 인벤토리 프리펩 실행 시
/// 프리팹의 부모오브젝트의 ClickedItem.cs에 있는 BoxAddPlayer()함수를 Onclicked 로 설정
/// </summary>
public class BoxItemBtn : MonoBehaviour
{
    public Button btn;
    private BoxClickedItem boxclickedItem;

    private void Start()
    {
        boxclickedItem = gameObject.GetComponentInParent<BoxClickedItem>(); // 부모오브젝트가 가진 ClickedItem.cs
        btn = gameObject.GetComponent<Button>(); // 현재 오브젝트의 버튼
        Debug.Log("아이템박스 아이템 프리팹 버튼 연결");
        btn.onClick.AddListener(boxclickedItem.BoxAddPlayer); // 함수를 연결
        Debug.LogError("버튼 연결 완료");

    }
}
