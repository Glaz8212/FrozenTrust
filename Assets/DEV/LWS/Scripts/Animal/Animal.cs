using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Animal : MonoBehaviourPun
{
    // 동물 공통 필드
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

    // 현재 상태
    protected AnimalState curState;

    protected virtual void Start()
    {
        curHp = maxHp;
        animator = GetComponent<Animator>();
        SetState(new IdleState(this));
    }

    protected virtual void Update()
    {
        // 마스터 클라이언트가 상태관리
        if (PhotonNetwork.IsMasterClient)
        {
            curState?.Update();
        }
    }

    public void SetState(AnimalState state)
    {
        // 현재 상태 종료
        curState?.Exit();

        // 새 상태
        curState = state;
        curState?.Enter();

        // 상태 동기화
        photonView.RPC("SyncState", RpcTarget.Others, state.GetType().Name);
    }

    [PunRPC]
    private void SyncState(string state)
    {
        // 동기화된 상태 설정
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
        // 마스터 클라이언트가 체력 관리
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
        if (direction.sqrMagnitude > 0.01f) // 방향 벡터가 0이 아니면
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 5f는 회전 속도
        }
    }

    protected virtual void Die()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PlayDieAnimation();

        // TODO: 고기 드랍
        
        
        PhotonNetwork.Destroy(gameObject);
    }

    public abstract void OnIdleUpdate(IdleState state);
    public abstract GameObject DetectPlayer();
}
