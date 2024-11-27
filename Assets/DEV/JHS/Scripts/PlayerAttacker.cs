using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAttacker : MonoBehaviourPun
{
    // �Ǽ�, �ٰŸ�, ���Ÿ�
    public enum Type { Non, CloserWeapon, RangedWeapon }
    public Type type = Type.Non;
    public bool attackTerm = false; // ����
    private bool attackHand = false; // �¿� ��ġ ���� ����
    private PlayerStatus status;
    [SerializeField] BoxCollider leftAttackArea; // �Ǽ� ���� ���� // ���� ��Ŵ� ���� ������Ʈ���ٰ� �߰�. �ֵθ��� ��Ǹ� ����
    [SerializeField] BoxCollider rightAttackArea;

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
        if (!photonView.IsMine || attackTerm)
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
                        // �������� ���� ���� �ʿ�
                        break;
                    case Type.RangedWeapon:
                        // ���Ÿ� �������� ����
                        break;
                }
            }
        }       
    }

    public void Non()
    {
        attackHand = !attackHand;

        // ���� �ִϸ��̼� �� �ݶ��̴� Ȱ��ȭ�� RPC�� ����ȭ
        photonView.RPC("ExecuteAttack", RpcTarget.All, attackHand);

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
    private void ExecuteAttack(bool isLeftHand)
    {
        if (isLeftHand)
        {
            // ���� ��ġ �ִϸ��̼� ����
            animator.Play("Punch_LeftHand");
            leftAttackArea.enabled = true;
        }
        else
        {
            // ���� ��ġ �ִϸ��̼� ����
            animator.Play("Punch_RightHand");
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
