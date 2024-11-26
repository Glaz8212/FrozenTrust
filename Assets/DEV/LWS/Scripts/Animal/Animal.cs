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

    [SerializeField] public float Damage;
    protected float damage
    {
        get { return Damage; }
        set { Damage = value; }
    }

    // ���� ����
    protected AnimalState curState;

    protected virtual void Start()
    {
        curHp = maxHp;
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

    public void TakeDamage(float damage)
    {
        // ������ Ŭ���̾�Ʈ�� ü�� ����

        if (!PhotonNetwork.IsMasterClient)
            return;

        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // TODO: ��� ���
        PhotonNetwork.Destroy(gameObject);
    }

    public abstract void OnIdleUpdate(IdleState state);
    public abstract GameObject DetectPlayer();
}
