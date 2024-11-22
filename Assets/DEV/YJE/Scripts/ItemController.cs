using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    /// <summary>
    /// 아이템을 인벤토리에 넣는 함수
    /// </summary>
    public void SaveItem()
    {
        // TODO: 추후 삭제하는 것이 아닌 아이템 인벤토리에 들어가는 것으로 수정이 필요
        //       현재는 Test용으로 간편하게 작성
        Debug.Log("아이템을 습득했습니다.");
        Destroy(gameObject);
    }
}
