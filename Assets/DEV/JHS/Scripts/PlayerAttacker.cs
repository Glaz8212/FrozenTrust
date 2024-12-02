using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAttacker : MonoBehaviourPun
{
    // 맨손, 근거리, 원거리
    public enum Type { Non, CloserWeapon, RangedWeapon, TwoHandWeapon }
    public Type type = Type.Non;
    public bool attackTerm = false; // 공속
    private bool attackHand = false; // 좌우 펀치 공격 판정
    private PlayerStatus status;
    [SerializeField] BoxCollider leftAttackArea; // 맨손 공격 판정 // 무기 든거는 무기 오브젝트에다가 추가. 휘두르는 모션만 구현
    [SerializeField] BoxCollider rightAttackArea;
    public WeaponState weaponState;
    public Collider weaponCollider;

    [SerializeField] Animator animator;
        
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        status = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (attackTerm)
            return;
        if (status.playerDie == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackTerm = true;
                switch (type)
                {
                    case Type.Non:
                        Non();
                        break;
                    case Type.CloserWeapon:
                        CloserWeapon();
                        break;
                    case Type.TwoHandWeapon:
                        TwoHandWeapon();                    
                        break;
                    case Type.RangedWeapon:
                        // 원거리 넣을꺼면 여기
                        break;
                }
            }
        }
    }
    public void SetWeaponState(WeaponState state)
    {
        weaponState = state;

        if (weaponState != null)
        {
            weaponCollider = weaponState.GetComponent<Collider>();
            Debug.Log("WeaponState와 Collider가 설정되었습니다.");
        }
        else
        {
            Debug.LogError("WeaponState가 null입니다.");
        }
    }
    // 무기 장착
    public void InstallationWeapon(Type types)
    {
        type = types;

        if (weaponState == null)
        {
            Debug.LogError("WeaponState가 설정되지 않았습니다.");
            return;
        }

        // 이미 설정된 weaponState에서 Collider 참조
        weaponCollider = weaponState.GetComponent<Collider>();
        Debug.Log($"{type}으로 변경");
    }
    // 무기 해제
    public void ReleaseWeapon()
    {      
        type = Type.Non;
        weaponState = null;
        weaponCollider = null;
        Debug.Log($"{type}으로 변경");
    }
    public void Non()
    {
        attackHand = !attackHand;

        // 공격 애니메이션 및 콜라이더 활성화를 RPC로 동기화
        photonView.RPC("ExecuteAttack", RpcTarget.All, attackHand);

        StartCoroutine(EndAttack());
    }
    public void CloserWeapon()
    {
        // 근거리 애니메이션 실행
        photonView.RPC("CloserAttack", RpcTarget.All);
        StartCoroutine(EndAttack());
    }
    public void TwoHandWeapon()
    {
        // 근거리 두손 애니메이션 실행
        photonView.RPC("TwoHandedAttack", RpcTarget.All);
        StartCoroutine(EndAttack());
    }
    public void RangedWeapon()
    {
        // 원거리 애니메이션 실행
    }
    private IEnumerator EndAttack()
    {        
        yield return new WaitForSeconds(2f); // 공격 지속 시간
        //leftAttackArea.enabled = false;
        //rightAttackArea.enabled = false;
        photonView.RPC("DeactivateAttackArea", RpcTarget.All);
        attackTerm = false; // 공격 쿨타임 해제
    }  
    [PunRPC]
    private void ExecuteAttack(bool isLeftHand)
    {
        if (isLeftHand)
        {
            // 좌측 펀치 애니메이션 실행
            animator.Play("Punch_LeftHand");
            leftAttackArea.enabled = true;
        }
        else
        {
            // 우측 펀치 애니메이션 실행
            animator.Play("Punch_RightHand");
            rightAttackArea.enabled = true;
        }
    }
    [PunRPC]
    private void CloserAttack(bool isLeftHand)
    {
        animator.Play("Punch_LeftHand");
        weaponCollider.enabled = true;
    }
    [PunRPC]
    private void TwoHandedAttack(bool isLeftHand)
    {
        animator.Play("Punch_LeftHand");
        weaponCollider.enabled = true;
    }


    [PunRPC]
    private void DeactivateAttackArea()
    {
        leftAttackArea.enabled = false;
        rightAttackArea.enabled = false;
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

     
}
