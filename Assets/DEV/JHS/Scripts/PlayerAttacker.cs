using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    // 맨손, 근거리, 원거리
    public enum Type { Non, CloserWeapon, RangedWeapon }
    public Type type = Type.Non;
    public bool attackTerm = false; // 공속
    private bool attackHand = false; // 좌우 펀치 공격 판정
    [SerializeField] BoxCollider leftAttackArea; // 맨손 공격 판정 // 무기 든거는 무기 오브젝트에다가 추가. 휘두르는 모션만 구현
    [SerializeField] BoxCollider rightAttackArea;

    [SerializeField] Animator animator;

    private void Update()
    {
        if (attackTerm || !Input.GetMouseButtonDown(0))
            return;
        if (!attackTerm && Input.GetMouseButtonDown(0))
        {
            attackTerm = true;
            switch (type)
            {
                case Type.Non:
                    Non();
                    break;
                case Type.CloserWeapon:
                    break;
                case Type.RangedWeapon:
                    break;
            }
        }
    }

    public void Non()
    {
        if (!attackHand)
        {
            attackHand = true;
            // 좌수 펀치 애니메이션 실행
            animator.SetBool("isPunching_Left", true);
            // 좌측 펀치 콜라이더 활성화
            leftAttackArea.enabled = true;
        }
        else if (attackHand)
        {
            attackHand = false;
            // 우수 펀치 애니메이션 실행
            animator.SetBool("isPunching_Right", true);
            // 우측 펀치 콜라이더 활성화
            rightAttackArea.enabled = true;
        }

        StartCoroutine(EndAttack());
    }
    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(2f); // 공격 지속 시간
        animator.SetBool("isPunching_Left", false);
        animator.SetBool("isPunching_Right", false);
        leftAttackArea.enabled = false;
        rightAttackArea.enabled = false;
        attackTerm = false; // 공격 쿨타임 해제
    }

    public void CloserWeapon()
    {
        // 근거리 애니메이션 실행
    }
    public void RangedWeapon()
    {
        // 원거리 애니메이션 실행
    }  
}
