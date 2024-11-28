using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{ 
    public enum Type { Idle, Misson, ItemBox, Item }
    public Type type = Type.Idle;
    // 상호작용 상태 판정
    public bool isInteracting = false;
    // 콜라이더 충돌 판정
    private bool isCollider = false;

    public MissionBox missionController;
    public BoxController boxController;
    public Item item;
    public PlayerInventory playerInventory;
    private PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        // Inventory 오브젝트를 찾아 PlayerInventory 할당
        if (playerInventory == null)
        {
            GameObject inventoryObject = GameObject.Find("Inventory");
            if (inventoryObject != null)
            {
                playerInventory = inventoryObject.GetComponent<PlayerInventory>();
            }

            // Inventory 오브젝트를 찾을 수 없는 경우 경고 출력
            if (playerInventory == null)
            {
                Debug.LogError("Inventory 오브젝트에서 PlayerInventory를 찾을 수 없습니다!");
            }
        }
    }
    private void Update()
    {
        // E 키 입력 처리
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && isCollider && status.playerDie == false)
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
                        Debug.LogWarning("MissionBoxController 설정되지 않았습니다.");
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
                    if (item != null)
                    {
                        // 아이템 테스터의 interaction에 playerInventory를 넣어 실행
                        item.interaction(playerInventory);
                        type = Type.Idle;
                        isCollider = false;
                        isInteracting = false;
                    }
                    else
                    {
                        Debug.LogWarning("item이 설정되지 않았습니다.");
                    }
                    break;
            }
        }
        // 상호작용한 오브젝트에서 상호작용이 끝났을때 false값을 설정 한걸 가져와야됨
        else if (!isCollider || (missionController != null && !missionController.IsUIOpen) || (boxController != null && !boxController.IsUIOpen))
        {              
            isInteracting = false;
        }
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            missionController = other.GetComponent<MissionBox>();
            if (missionController != null)
            {
                type = Type.Misson;
            }
        }
        else if (other.CompareTag("ItemBox"))
        {
            boxController = other.GetComponent<BoxController>();
            if (boxController != null)
            {
                type = Type.ItemBox;
            }
        }
        else if (other.CompareTag("Item"))
        {
            item = other.GetComponent<Item>();
            if (item != null)
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
        if (other.GetComponent<MissionBox>() == missionController)
        {
            missionController.MissionBoxClose();
            missionController = null;
        }
        else if (other.GetComponent<BoxController>() == boxController)
        {
            boxController.BoxClose();
            boxController = null;
        }
        else if (other.GetComponent<ItemController>() == item)
        {
            item = null;
        }

        type = Type.Idle;
        isCollider = false; // 충돌 상태 해제
        isInteracting = false; // 상호작용 상태 초기화
    }
}
