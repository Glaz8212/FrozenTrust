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
    public BoxController boxController;
    public ItemController itemController;

    private void Update()
    {
        // E 키 입력 처리
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && isCollider)
        {
            isInteracting = true; // 상호작용 중 상태로 변경

            // 상호작용 함수 호출
            switch (type)
            {
                case Type.Idle:
                    Debug.Log("상호작용 가능한 상태가 아닙니다.");
                    break;
                case Type.Misson:
                    if (missionController != null)
                    {
                        missionController.MissionBoxOpen();
                    }
                    else
                    {
                        Debug.LogWarning("MissionController가 설정되지 않았습니다.");
                    }
                    break;
                case Type.ItemBox:
                    if (boxController != null)
                    {
                        boxController.BoxOpen();
                    }
                    else
                    {
                        Debug.LogWarning("BoxController가 설정되지 않았습니다.");
                    }
                    break;
                case Type.Item:
                    if (itemController != null)
                    {
                        itemController.SaveItem();
                    }
                    else
                    {
                        Debug.LogWarning("ItemController가 설정되지 않았습니다.");
                    }
                    break;
            }
        }
        // 상호작용한 오브젝트에서 상호작용이 끝났을때 false값을 설정 한걸 가져와야됨
        // 그 값이 false라면을 else if 조건에 넣어줘야됨 
        else if (!isCollider || (missionController != null && !missionController.IsUIOpen) || (boxController != null && !boxController.IsUIOpen))
        {
            isInteracting = false;
        }
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            missionController = other.GetComponent<MissionController>();
            if (missionController != null)
            {
                type = Type.Misson;
            }
        }
        else if (other.CompareTag("ItemBox"))
        {
            boxController = other.GetComponent<BoxController>();
            
            type = Type.ItemBox;
        }
        else if (other.CompareTag("Item"))
        {
            itemController = other.GetComponent<ItemController>();
            if (itemController != null)
            {
                type = Type.Item;
            }
        }
        else
        {
            Debug.Log("상호작용 할수있는 오브젝트가 아닙니다");
            type = Type.Idle;
        }

        isCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MissionController>() == missionController)
        {
            missionController = null;
        }
        else if (other.GetComponent<BoxController>() == boxController)
        {
            boxController = null;
        }
        else if (other.GetComponent<ItemController>() == itemController)
        {
            itemController = null;
        }

        type = Type.Idle;
        isCollider = false; // 충돌 상태 해제
        isInteracting = false; // 상호작용 상태 초기화
    }
}
