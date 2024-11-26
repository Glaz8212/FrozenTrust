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

    [SerializeField] public float Damage;
    protected float damage
    {
        get { return Damage; }
        set { Damage = value; }
    }

    // 현재 상태
    protected AnimalState curState;

    protected virtual void Start()
    {
        curHp = maxHp;
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

    public void TakeDamage(float damage)
    {
        // 마스터 클라이언트가 체력 관리

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
        // TODO: 고기 드랍
        PhotonNetwork.Destroy(gameObject);
    }

    public abstract void OnIdleUpdate(IdleState state);
    public abstract GameObject DetectPlayer();
}
