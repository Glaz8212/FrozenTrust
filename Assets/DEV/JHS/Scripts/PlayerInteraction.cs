using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{ 
    public enum Type { Idle, Mission, ItemBox, Item }
    public Type type = Type.Idle;
    // 상호작용 상태 판정
    public bool isInteracting = false;
    // 콜라이더 충돌 판정
    private Collider currentCollider;

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
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting && currentCollider != null && status.playerDie == false)
        {
            isInteracting = true; // 상호작용 중 상태로 변경

            // 상호작용 함수 호출
            switch (type)
            {
                case Type.Idle:
                    Debug.Log("상호작용 가능한 상태가 아닙니다.");
                    break;
                case Type.Mission:
                    // null조건 연산자 ?. missionController의 값이 null이 아닐경우에만 MissionBoxOpen를 실행. nulld이면 MissionBoxOpen를 실행하지 않음
                    missionController?.MissionBoxOpen();
                    break;
                case Type.ItemBox:
                    boxController?.BoxOpen();
                    break;
                case Type.Item:
                    item?.interaction(playerInventory);
                    ResetInteraction();
                    break;
            }
        }
        // 상호작용한 오브젝트에서 상호작용이 끝났을때 false값을 설정 한걸 가져와야됨
        if (missionController != null && !missionController.IsUIOpen || boxController != null && !boxController.IsUIOpen)
        {
            ResetInteraction();
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        // 콜라이더에 값이 들어가 있으면 리턴
        if (currentCollider != null) return;

        currentCollider = other;

        if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            missionController = other.GetComponent<MissionBox>();
            // 삼항 연산자 사용 : 변수 = 조건문 ? 조건문이 참일 때 값 : 조건문이 거짓일 때 값
            type = missionController != null ? Type.Mission : Type.Idle;
        }
        else if (other.CompareTag("ItemBox"))
        {
            boxController = other.GetComponent<BoxController>();
            // 타입이 boxController에 값이 있다면 ItemBox 아니면 Idle
            type = boxController != null ? Type.ItemBox : Type.Idle;
        }
        else if (other.CompareTag("Item"))
        {
            item = other.GetComponent<Item>();
            // 타입이 item에 값이 있으면 Item 없으면 Idle
            type = item != null ? Type.Item : Type.Idle;
        }
        else
        {
            Debug.Log("상호작용 할수있는 오브젝트가 아닙니다");
            type = Type.Idle;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 콜라이더에서 나온 값이 currentCollider 값과 같다면 실행 #땅 콜라이더가 이 값에 들어갈 경우가 있나?
        if (other == currentCollider)
        {
            // 미션 콜라이더에 값이 있고 그 값이 other의 MissionBox와 같다면 미션 ui닫기 실행
            if (missionController != null && missionController == other.GetComponent<MissionBox>())
            {
                missionController.MissionBoxClose();
            }
            // 박스 콜라이더에 값이 있고 그 값이 other의 BoxController와 같다면 박스 ui닫기 실행
            else if (boxController != null && boxController == other.GetComponent<BoxController>())
            {
                boxController.BoxClose();
            }

            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        type = Type.Idle;
        currentCollider = null;
        missionController = null;
        boxController = null;
        item = null;
        isInteracting = false;
    }
}
