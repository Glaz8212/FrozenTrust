using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Animal : MonoBehaviourPun
{
    // ���� ���� �ʵ�
    [SerializeField] protected float speed;
    [SerializeField] protected float maxHp;
    protected float curHp;

    [SerializeField] protected float damage;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    protected Animator animator;

    // ���� ����
    protected AnimalState curState;

    protected virtual void Start()
    {
        curHp = maxHp;
        animator = GetComponent<Animator>();
        SetState(new IdleState(this));
    }

    protected virtual void Update()
    {
        // ������ Ŭ���̾�Ʈ�� ���°���
        if (PhotonNetwork.IsMasterClient)
        {
            curState?.Update();
        }
    }

    public void SetState(AnimalState state)
    {
        // ���� ���� ����
        curState?.Exit();

        // �� ����
        curState = state;
        curState?.Enter();

        // ���� ����ȭ
        photonView.RPC("SyncState", RpcTarget.Others, state.GetType().Name);
    }

    [PunRPC]
    private void SyncState(string state)
    {
        // ����ȭ�� ���� ����
        if (state == nameof(IdleState))
            curState = new IdleState(this);
        else if (state == nameof(AttackState))
            curState = new AttackState(this);
    }

    [PunRPC]
    private void SyncHealth(float updatedHp)
    {
        curHp = updatedHp;
    }

    public void PlayIdleAnimation()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
    }

    public void PlayMoveAnimation()
    {
        animator.SetBool("isMoving", true);
    }

    public void PlayAttackAnimation()
    {
        animator.SetBool("isAttacking", true);
    }

    public void PlayDieAnimation()
    {
        animator.SetBool("isDead", true);
    }

    public void TakeDamage(float damage)
    {
        // ������ Ŭ���̾�Ʈ�� ü�� ����
        if (!PhotonNetwork.IsMasterClient)
            return;

        curHp -= damage;
        photonView.RPC("SyncHealth", RpcTarget.Others, curHp);

        if (curHp <= 0)
        {
            Die();
        }
    }
    public void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public void RotateTowardsDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f) // ���� ���Ͱ� 0�� �ƴϸ�
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 5f�� ȸ�� �ӵ�
        }
    }

    protected virtual void Die()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PlayDieAnimation();

        // TODO: ��� ���
        
        
        PhotonNetwork.Destroy(gameObject);
    }

    public abstract void OnIdleUpdate(IdleState state);
    public abstract GameObject DetectPlayer();
}
