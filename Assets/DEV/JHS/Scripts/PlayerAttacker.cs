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
    public WeaponDamage damageCollider;
    public Collider weaponCollider;
    public PlayerInteraction interaction;

    [SerializeField] Animator animator;
        
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        status = GetComponent<PlayerStatus>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (!photonView.IsMine || attackTerm)
            return;
        if (interaction.missionBox.IsUIOpen == true || interaction.boxController.IsUIOpen == true)
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
    public void SetWeaponState(WeaponState state, WeaponDamage weaponDamage)
    {
        weaponState = state;
        damageCollider = weaponDamage;

        if (weaponState != null)
        {
            weaponCollider = damageCollider.GetComponent<Collider>();
        }
        else
        {
            Debug.LogError("weaponCollider null�Դϴ�.");
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

        DeactivateAttackArea();
        Debug.Log($"{type}���� ����");
    }
    // ���� ����
    public void ReleaseWeapon()
    {      
        type = Type.Non;
        weaponState = null;
        damageCollider = null;
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
    private void CloserAttack()
    {
        animator.Play("SlashOneHand");
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }
    [PunRPC]
    private void TwoHandedAttack()
    {
        animator.Play("SlashTwoHand");
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }


    [PunRPC]
    private void DeactivateAttackArea()
    {
        if (weaponCollider ==  null)
        {
            leftAttackArea.enabled = false;
            rightAttackArea.enabled = false;
        }      
        else if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }     
}
