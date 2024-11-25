using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType 
{
    Wood, Ore, Fruit, Meat, Weapon
}

// [System.Serializable]
public class Item : MonoBehaviour
{
    public ItemType itemType;
    private ItemObj itemObj;
    [SerializeField] ItemObj returnObj;
    [SerializeField] List<ItemObj> returnPoolList;
    // public string itemName;
    // public Sprite itemImg;

    private void Start()
    {
        itemObj = gameObject.GetComponent<ItemObj>();
        returnObj= itemObj.returnObj;
        returnPoolList = itemObj.returnPoolList;
    }

    /// <summary>
    /// 선택한 아이템 사용 성공여부를 반환
    /// </summary>
    /// <returns></returns>
    public bool UseItme()
    {
        return false;
    }

    public void Update()
    {
        // 임의 테스트
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveItem();
        }
    }
    /// <summary>
    /// 아이템을 인벤토리에 넣는 함수
    /// </summary>
    public void SaveItem()
    {
        // TODO: 추후 삭제하는 것이 아닌 아이템 인벤토리에 들어가는 것으로 수정이 필요
        //       현재는 Test용으로 간편하게 작성
        Debug.Log("아이템을 습득했습니다.");
        itemObj.ReturnItem(returnObj, returnPoolList); 
        // 첫번째로 오브젝트 풀에서 생성하는 오브젝트만 returnObj, returnPoolList가 제대로 설정되지 않음.
        // Destroy(gameObject);
    }
}
