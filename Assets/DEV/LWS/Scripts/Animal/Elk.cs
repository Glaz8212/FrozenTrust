using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elk : MonoBehaviourPun, IPunObservable
{
    // �ִϸ�����
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathAnimation;

    // ȸ�� �ӵ�
    [SerializeField] public float rotationSpeed;

    // ��ȸ �̵��� ��Ʈ�� ����Ʈ
    [SerializeField] Transform[] patrolPoints;
    private int currentPatrolIndex;

    // ����ȭ�� �����ǰ� �����̼�
    private Vector3 netPosition;
    private Quaternion netRotation;

    // ü�� (����ȭ ���ʿ�)
    [SerializeField] float maxHp;
    [SerializeField] float curHp;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        animator = GetComponent<Animator>();
        netPosition = transform.position;
        netRotation = transform.rotation;
        curHp = maxHp;
    }


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (curHp > 0)
                Idle();
            else if (!animator.GetBool("isDead"))
                Die();
        }
        else
        {
            SmoothSync();
        }
    }

    private void Idle()
    {
        Vector3 direction = Vector3.zero;

        // ��ȸ ������ �������� Ÿ������ ����
        Vector3 target = patrolPoints[currentPatrolIndex].position;

        // ��ǥ ������ �������� ���� ��쿡�� �̵� �� ȸ�� ó��
        if (Vector3.Distance(transform.position, target) >= 0.5f)
        {
            direction = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime);
            RotateToDirection(direction);

            // �ִϸ��̼��� isRunning���� ����
            // animator.SetBool("isRunning", true);
        }
        else
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    // Ÿ�ٹ������� ȸ��
    private void RotateToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SmoothSync()
    {
        // ����ȭ Ÿ�̸Ӹ� ������� ��ġ �� ȸ�� ����
        float syncRate = Time.deltaTime * 10;
        transform.position = Vector3.Lerp(transform.position, netPosition, syncRate);
        transform.rotation = Quaternion.Lerp(transform.rotation, netRotation, syncRate);

        // �ִϸ��̼� ����ȭ
        if (animator != null && curHp > 0)
        {
            bool isRunning = Vector3.Distance(transform.position, netPosition) > 0.1f; // �̵� �Ǵ�
            animator.SetBool("isRunning", isRunning);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        maxHp -= damage;

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathAnimation.length); // Die �ִϸ��̼� ��� �ð�

        DropMeat(); // ��� ���
        PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ �󿡼� ����
    }

    private void DropMeat()
    {

        PhotonNetwork.Instantiate("LWS/Meat", transform.position, Quaternion.identity);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �� ������ ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            // �ִϸ��̼� ����ȭ
            stream.SendNext(animator.GetBool("isDead"));
            stream.SendNext(animator.GetBool("isRunning"));
        }
        else
        {
            // �ٸ� �÷��̾� ������ ����
            netPosition = (Vector3)stream.ReceiveNext();
            netRotation = (Quaternion)stream.ReceiveNext();

            // �ִϸ��̼� ���� ����
            animator.SetBool("isDead", (bool)stream.ReceiveNext());
            animator.SetBool("isRunning", (bool)stream.ReceiveNext());
        }
    }
}