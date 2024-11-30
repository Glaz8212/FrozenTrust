using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Animal : MonoBehaviourPun, IPunObservable
{
    [SerializeField] protected float speed;
    [SerializeField] protected float maxHp;
    protected float curHp;

    [SerializeField] protected float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    protected Animator animator;

    // HP 최대로 설정, animator 초기화
    protected virtual void Start()
    {
        curHp = maxHp;
        animator = GetComponent<Animator>();
    }

    // 마스터 클라이언트만 행동 업데이트
    protected virtual void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateBehaviour();
        }
    }

    // 각 동물마다 구현할 추상 메서드
    protected abstract void UpdateBehaviour();


    public void TakeDamage(float damage)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        curHp -= damage;

        if (curHp <= 0)
        {
            photonView.RPC(nameof(Die), RpcTarget.All);
        }
    }

    [PunRPC]
    protected virtual void Die()
    {
        PlayDieAnimation();
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void PlayIdleAnimation()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
    }

    [PunRPC]
    public void PlayMoveAnimation()
    {
        animator.SetBool("isMoving", true);
    }

    [PunRPC]
    public void PlayAttackAnimation()
    {
        animator.SetBool("isAttacking", true);
    }

    [PunRPC]
    public void PlayDieAnimation()
    {
        animator.SetBool("isDead", true);
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // 네트워크 데이터 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 데이터 전송
        {
            stream.SendNext(curHp);
        }
        else // 데이터 수신
        {
            curHp = (float)stream.ReceiveNext();
        }
    }
}
