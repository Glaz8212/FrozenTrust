using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviourPun
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
        if (!photonView.IsMine || attackTerm)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            attackTerm = true;            
            switch (type)
            {
                case Type.Non:
                    Non();
                    break;
                case Type.CloserWeapon:
                    // �������� ���� ���� �ʿ�
                    break;
                case Type.RangedWeapon:
                    // ���Ÿ� �������� ����
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
            animator.Play("Punch_LeftHand");

            // ���� ��ġ �ݶ��̴� Ȱ��ȭ
            //leftAttackArea.enabled = true;
            photonView.RPC("ActivateAttackArea", RpcTarget.All, true);
        }
        else if (attackHand)
        {
            attackHand = false;
            // ��� ��ġ �ִϸ��̼� ����
            animator.Play("Punch_RightHand");
           
            // ���� ��ġ �ݶ��̴� Ȱ��ȭ
            //rightAttackArea.enabled = true;
            photonView.RPC("ActivateAttackArea", RpcTarget.All, false);
        }

        StartCoroutine(EndAttack());
    }
    private IEnumerator EndAttack()
    {        
        yield return new WaitForSeconds(2f); // ���� ���� �ð�
        //leftAttackArea.enabled = false;
        //rightAttackArea.enabled = false;
        photonView.RPC("DeactivateAttackArea", RpcTarget.All);
        attackTerm = false; // ���� ��Ÿ�� ����
    }

    [PunRPC]
    private void ActivateAttackArea(bool isLeft)
    {
        if (isLeft)
        {
            leftAttackArea.enabled = true;
        }
        else
        {
            rightAttackArea.enabled = true;
        }
    }

    [PunRPC]
    private void DeactivateAttackArea()
    {
        leftAttackArea.enabled = false;
        rightAttackArea.enabled = false;
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
