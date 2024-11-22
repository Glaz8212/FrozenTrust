using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    // �Ǽ�, �ٰŸ�, ���Ÿ�
    public enum Type { Non, CloserWeapon, RangedWeapon }
    public Type type = Type.Non;
    public bool attackTerm = false; // ����
    private bool attackHand = false; // �¿� ��ġ ���� ����
    [SerializeField] BoxCollider leftAttackArea; // �Ǽ� ���� ���� // ���� ��Ŵ� ���� ������Ʈ���ٰ� �߰�. �ֵθ��� ��Ǹ� ����
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
            // �¼� ��ġ �ִϸ��̼� ����
            animator.SetBool("isPunching_Left", true);
            // ���� ��ġ �ݶ��̴� Ȱ��ȭ
            leftAttackArea.enabled = true;
        }
        else if (attackHand)
        {
            attackHand = false;
            // ��� ��ġ �ִϸ��̼� ����
            animator.SetBool("isPunching_Right", true);
            // ���� ��ġ �ݶ��̴� Ȱ��ȭ
            rightAttackArea.enabled = true;
        }

        StartCoroutine(EndAttack());
    }
    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(2f); // ���� ���� �ð�
        animator.SetBool("isPunching_Left", false);
        animator.SetBool("isPunching_Right", false);
        leftAttackArea.enabled = false;
        rightAttackArea.enabled = false;
        attackTerm = false; // ���� ��Ÿ�� ����
    }

    public void CloserWeapon()
    {
        // �ٰŸ� �ִϸ��̼� ����
    }
    public void RangedWeapon()
    {
        // ���Ÿ� �ִϸ��̼� ����
    }  
}
