using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAttacker : MonoBehaviourPun
{
    // �Ǽ�, �ٰŸ�, ���Ÿ�
    public enum Type { Non, CloserWeapon, RangedWeapon, TwoHandWeapon }
    public Type type = Type.Non;
    public bool attackTerm = false; // ����
    private bool attackHand = false; // �¿� ��ġ ���� ����
    private PlayerStatus status;
    [SerializeField] BoxCollider leftAttackArea; // �Ǽ� ���� ���� // ���� ��Ŵ� ���� ������Ʈ���ٰ� �߰�. �ֵθ��� ��Ǹ� ����
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
                        // ���Ÿ� �������� ����
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
            Debug.Log("WeaponState�� Collider�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("WeaponState�� null�Դϴ�.");
        }
    }
    // ���� ����
    public void InstallationWeapon(Type types)
    {
        type = types;

        if (weaponState == null)
        {
            Debug.LogError("WeaponState�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �̹� ������ weaponState���� Collider ����
        weaponCollider = weaponState.GetComponent<Collider>();
        Debug.Log($"{type}���� ����");
    }
    // ���� ����
    public void ReleaseWeapon()
    {      
        type = Type.Non;
        weaponState = null;
        weaponCollider = null;
        Debug.Log($"{type}���� ����");
    }
    public void Non()
    {
        attackHand = !attackHand;

        // ���� �ִϸ��̼� �� �ݶ��̴� Ȱ��ȭ�� RPC�� ����ȭ
        photonView.RPC("ExecuteAttack", RpcTarget.All, attackHand);

        StartCoroutine(EndAttack());
    }
    public void CloserWeapon()
    {
        // �ٰŸ� �ִϸ��̼� ����
        photonView.RPC("CloserAttack", RpcTarget.All);
        StartCoroutine(EndAttack());
    }
    public void TwoHandWeapon()
    {
        // �ٰŸ� �μ� �ִϸ��̼� ����
        photonView.RPC("TwoHandedAttack", RpcTarget.All);
        StartCoroutine(EndAttack());
    }
    public void RangedWeapon()
    {
        // ���Ÿ� �ִϸ��̼� ����
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
