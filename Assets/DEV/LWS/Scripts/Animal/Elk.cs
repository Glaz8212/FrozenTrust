using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elk : MonoBehaviourPun, IPunObservable
{
    // 애니메이터
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathAnimation;

    // 회전 속도
    [SerializeField] public float rotationSpeed;

    // 순회 이동용 패트롤 포인트
    [SerializeField] Transform[] patrolPoints;
    private int currentPatrolIndex;

    // 동기화용 포지션과 로테이션
    private Vector3 netPosition;
    private Quaternion netRotation;

    // 체력 (동기화 불필요)
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

        // 순회 지점의 포지션을 타겟으로 설정
        Vector3 target = patrolPoints[currentPatrolIndex].position;

        // 목표 지점에 도달하지 않은 경우에만 이동 및 회전 처리
        if (Vector3.Distance(transform.position, target) >= 0.5f)
        {
            direction = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime);
            RotateToDirection(direction);

            // 애니메이션을 isRunning으로 설정
            // animator.SetBool("isRunning", true);
        }
        else
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    // 타겟방향으로 회전
    private void RotateToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SmoothSync()
    {
        // 동기화 타이머를 기반으로 위치 및 회전 보간
        float syncRate = Time.deltaTime * 10;
        transform.position = Vector3.Lerp(transform.position, netPosition, syncRate);
        transform.rotation = Quaternion.Lerp(transform.rotation, netRotation, syncRate);

        // 애니메이션 동기화
        if (animator != null && curHp > 0)
        {
            bool isRunning = Vector3.Distance(transform.position, netPosition) > 0.1f; // 이동 판단
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
        yield return new WaitForSeconds(deathAnimation.length); // Die 애니메이션 재생 시간

        DropMeat(); // 고기 드랍
        PhotonNetwork.Destroy(gameObject); // 네트워크 상에서 삭제
    }

    private void DropMeat()
    {

        PhotonNetwork.Instantiate("LWS/Meat", transform.position, Quaternion.identity);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내 데이터 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            // 애니메이션 동기화
            stream.SendNext(animator.GetBool("isDead"));
            stream.SendNext(animator.GetBool("isRunning"));
        }
        else
        {
            // 다른 플레이어 데이터 수신
            netPosition = (Vector3)stream.ReceiveNext();
            netRotation = (Quaternion)stream.ReceiveNext();

            // 애니메이션 상태 수신
            animator.SetBool("isDead", (bool)stream.ReceiveNext());
            animator.SetBool("isRunning", (bool)stream.ReceiveNext());
        }
    }
}