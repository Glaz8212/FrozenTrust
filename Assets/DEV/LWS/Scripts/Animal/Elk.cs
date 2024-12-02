using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem.XR;

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

    // ����� ��� ������ ������
    [SerializeField] GameObject meatPrefab;

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
        if (PhotonNetwork.IsMasterClient && curHp > 0)
        {
            Idle();
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
        direction = (target - transform.position).normalized;

        // ��ȸ ������ ���������� �̵�, ȸ��
        transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime);
        RotateToDirection(direction);

        // ���� �� ���� ��ȸ������ Ÿ������ ����
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        // �ִϸ��̼��� isRunning���� ����
        animator.SetBool("isRunning", true);
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

        curHp -= maxHp;

        if (curHp <= 0)
        {
            Die();
        }
    }

    // ��� �ִϸ��̼� ��� bool Ʈ��� ����
    [PunRPC]
    private void DieAnimation()
    {
        animator.SetBool("isDead", true);
    }

    private void Die()
    {
        photonView.RPC("DieAnimation", RpcTarget.All);
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
        if (meatPrefab != null)
        {
            PhotonNetwork.Instantiate(meatPrefab.name, transform.position, Quaternion.identity);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �� ������ ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // �ٸ� �÷��̾� ������ ����
            netPosition = (Vector3)stream.ReceiveNext();
            netRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
