using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static WeaponState;

public class PlayerInteraction : MonoBehaviourPun
{ 
    public enum Type { Idle, Mission, ItemBox, Item, Weapon }
    public Type type = Type.Idle;
    // 상호작용 상태 판정
    public bool isInteracting = false;
    // 콜라이더 충돌 판정
    public Collider currentCollider;

    // 스크립트 값 불러오기 위한 변수들
    public MissionBox missionController;
    public BoxController boxController;
    public Item item;
    public WeaponState weapon;
    public GameObject weaponGameObject;
    public PlayerInventory playerInventory;
    private PlayerStatus status;
    private PlayerAttacker attacker;

    // 플레이어의 무기가 소환되는 위치
    [SerializeField] Transform playerHandTransform;
    
    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        attacker = GetComponent<PlayerAttacker>();
        // Inventory 오브젝트를 찾아 PlayerInventory 할당
        if (playerInventory == null)
        {
            GameObject inventoryObject = GameObject.FindWithTag("PlayerInventory");
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
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.E) && !isInteracting && currentCollider != null && status.playerDie == false)
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
                    //boxController값의 BoxOpen실행
                    boxController?.BoxOpen();
                    break;
                case Type.Item:
                    if(playerInventory.inventory.Count < 4)
                    {
                        item?.interaction(playerInventory);                        
                        ResetInteraction();
                    }                    
                    // 아이템의 값이 3개라면 
                    // item의 interaction에 인벤토리의 playerInventory값을 넣어 실행                    
                    break;
                case Type.Weapon:
                    if(currentCollider == null)
                    {
                        ResetInteraction();
                    }
                    else
                    {

                        if (weapon.weaponType == WeaponType.OneHanded)
                        {
                            MoverWeapon();

                            attacker?.InstallationWeapon(PlayerAttacker.Type.CloserWeapon);
                        }
                        else if (weapon.weaponType == WeaponType.TwoHanded)
                        {
                            MoverWeapon();

                            attacker?.InstallationWeapon(PlayerAttacker.Type.TwoHandWeapon);
                        }
                        ResetInteraction();
                    }
                    break;
            }
        }
        // 플레이어 본인이고 죽지 않았고 무기 오브젝트가 장착되어 있을때 Q를 누른다면
        else if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q) && status.playerDie == false && weaponGameObject != null)
        {
            // 자식에 들어간 무기 제거
            WeaponState weaponState = weaponGameObject.GetComponentInChildren<WeaponState>();
            if (weaponState != null)
            {
                attacker.ReleaseWeapon();
                Debug.Log("무기 다시 활성화");
                // 물리 활성화 및 충돌기 활성화
                weaponState.photonView.RPC("Active", RpcTarget.All);
            }
            PhotonView weaponPhotonView = weaponGameObject.GetComponent<PhotonView>();
            if (weaponPhotonView != null && weaponPhotonView.IsMine)
            {
                weaponPhotonView.TransferOwnership(0); // 소유권 반환
            }
            weaponGameObject.transform.SetParent(null);
            weaponGameObject = null;          
        }
    }

    private void MoverWeapon()
    {
        PhotonView weaponPhotonView = weaponGameObject.GetComponent<PhotonView>();
        if (weaponPhotonView != null && !weaponPhotonView.IsMine)
        {
            weaponPhotonView.TransferOwnership(photonView.Owner);
        }
        weaponGameObject.transform.SetParent(playerHandTransform);

        weaponGameObject.transform.localPosition = Vector3.zero;
        weaponGameObject.transform.localRotation = Quaternion.Euler(0, -90, 0); // Y값 90도 회전

        WeaponState weaponState = weapon.GetComponentInChildren<WeaponState>();
        if (weaponState != null)
        {
            attacker?.SetWeaponState(weaponState);
            weaponState.photonView.RPC("Deactivate", RpcTarget.All);
        }
        attacker?.InstallationWeapon(PlayerAttacker.Type.TwoHandWeapon);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        // 콜라이더에 값이 들어가 있으면 리턴
        if (currentCollider != null) return;

        Debug.Log("반응");
        if (other.CompareTag("Item"))
        {
            // currentCollider에 충돌한 other값 삽입
            currentCollider = other;
            // item에 other의 Item 불러오기
            item = other.GetComponent<Item>();
            // 타입이 item에 값이 있으면 Item 없으면 Idle
            type = item != null ? Type.Item : Type.Idle;
        }
        else if (other.CompareTag("Weapon") && weaponGameObject == null)
        {
            Debug.Log("무기");
            // currentCollider에 충돌한 other값 삽입
            currentCollider = other;
            // WeaponState가 아닌, 충돌한 오브젝트 자체를 weapon으로 설정
            weaponGameObject = other.gameObject;
            // 충돌한 오브젝트의 자식에서 위치한  WeaponState를 참조
            weapon = other.GetComponentInChildren<WeaponState>();
            Debug.Log($"{weaponGameObject}");
            type = weaponGameObject != null ? Type.Weapon : Type.Idle;
        }
        else if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            // currentCollider에 충돌한 other값 삽입
            currentCollider = other;
            // missionController에 other값의 MissionBox 불러오기
            missionController = other.GetComponent<MissionBox>();
            // 삼항 연산자 사용 : 변수 = 조건문 ? 조건문이 참일 때 값 : 조건문이 거짓일 때 값
            type = missionController != null ? Type.Mission : Type.Idle;
        }
        else if (other.CompareTag("ItemBox"))
        {
            // currentCollider에 충돌한 other값 삽입
            currentCollider = other;
            // boxController에 other의 BoxController 불러오기
            boxController = other.GetComponent<BoxController>();
            // 타입이 boxController에 값이 있다면 ItemBox 아니면 Idle
            type = boxController != null ? Type.ItemBox : Type.Idle;
        }       
        else
        {
            Debug.Log("상호작용 할수있는 오브젝트가 아닙니다");
            type = Type.Idle;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 로컬 플레이어 소유일 때만 처리
        if (!photonView.IsMine) return;
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
            // 값 리셋
            weaponGameObject = null;
            ResetInteraction();
        }
    }

    // 모든 값 리셋
    public void ResetInteraction()
    {
        type = Type.Idle;
        currentCollider = null;
        missionController = null;
        boxController = null;
        weapon = null;
        item = null;
        isInteracting = false;
    }
}
