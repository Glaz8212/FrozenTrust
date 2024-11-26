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

    public void PlayAnimation(string name)
    {
        animator.Play(name);
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

    protected virtual void Die()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PlayAnimation("Die");

        // TODO: ��� ���
        
        
        PhotonNetwork.Destroy(gameObject);
    }

    public abstract void OnIdleUpdate(IdleState state);
    public abstract GameObject DetectPlayer();
}
