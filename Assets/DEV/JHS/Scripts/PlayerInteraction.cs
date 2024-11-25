using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Mission이나 ItemBox 콜라이더와 충돌 하였을때 
    // e 버튼을 클릭하면 그 콜라이더가 있는 오브젝트의 함수를 가져와 상호작용
    // 여러 콜라이더가 겹칠때 하나의 콜라이더랑만 상호작용
    // false값이 와야지만 다른 오브젝트와 상호작용

    // 상호작용 할 수 있는 오브젝트에 들어간 상호작용 스크립트 
    // private 스크립트명 함수명;

    public enum Type { Idle, Misson, ItemBox, Item }
    public Type type = Type.Idle;
    // 상호작용 상태 판정
    public bool isInteracting = false;
    // 콜라이더 충돌 판정
    private bool isCollider = false;

    public MissionController missionController;
    public BoxController BoxController;
    public ItemController itemController;

    private void Update()
    {
        // E 키 입력 처리
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && isCollider == true) //&& 불러온 상호작용 함수 != null )
        {
            isInteracting = true; // 상호작용 중 상태로 변경

            // 상호작용 함수 호출
            switch (type)
            {
                case Type.Idle:
                    break;
                case Type.Misson:
                    Misson();
                    break;
                case Type.ItemBox:
                    ItemBox();
                    break;
                case Type.Item:
                    Item();
                    break;
            }
        }
        // 상호작용한 오브젝트에서 상호작용이 끝났을때 false값을 설정 한걸 가져와야됨
        // 그 값이 false라면을 else if 조건에 넣어줘야됨 
        else if (isCollider == false || missionController.IsUIOpen == false || BoxController.IsUIOpen == false) // || 또는 상호작용 창을 닫았을때 
        {
            isInteracting = false;
        }
    }

    private void Misson()
    {
        // missionController.MissionBoxOpen();
    }

    private void ItemBox()
    {
        BoxController.BoxOpen();
    }

    private void Item()
    {
        itemController.SaveItem();
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Misson1") || other.CompareTag("Misson2") || other.CompareTag("Misson1"))
        {
            missionController = other.GetComponent<MissionController>();
            type = Type.Misson;
        }
        else if (other.CompareTag("ItemBox"))
        {
            BoxController = other.GetComponent<BoxController>();
            
            type = Type.ItemBox;
        }
        else if (other.CompareTag("Item"))
        {
            itemController = other.GetComponent<ItemController>();
            type = Type.Item;
        }
        else
        {
            Debug.Log("상호작용 할수있는 오브젝트가 아닙니다");
        }

        isCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // 충돌이 종료되면 상호작용 상태 초기화 해줌
        isCollider = false;
    }
}
