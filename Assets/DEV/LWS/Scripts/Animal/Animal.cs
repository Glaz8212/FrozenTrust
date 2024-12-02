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

    // HP �ִ�� ����, animator �ʱ�ȭ
    protected virtual void Start()
    {
        curHp = maxHp;
        animator = GetComponent<Animator>();
    }

    // ������ Ŭ���̾�Ʈ�� �ൿ ������Ʈ
    protected virtual void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateBehaviour();
        }
    }

    // �� �������� ������ �߻� �޼���
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

    // ��Ʈ��ũ ������ ����ȭ
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // ������ ����
        {
            stream.SendNext(curHp);
        }
        else // ������ ����
        {
            curHp = (float)stream.ReceiveNext();
        }
    }
}
