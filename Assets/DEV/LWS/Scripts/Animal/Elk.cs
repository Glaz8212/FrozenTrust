using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem.XR;

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

    // 드랍될 고기 아이템 프리팹
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
        
        // 순회 지점의 포지션을 타겟으로 설정
        Vector3 target = patrolPoints[currentPatrolIndex].position;
        direction = (target - transform.position).normalized;

        // 순회 지점의 포지션으로 이동, 회전
        transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime);
        RotateToDirection(direction);

        // 도착 시 다음 순회지점을 타겟으로 설정
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        // 애니메이션을 isRunning으로 설정
        animator.SetBool("isRunning", true);
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

        curHp -= maxHp;

        if (curHp <= 0)
        {
            Die();
        }
    }

    // 사망 애니메이션 출력 bool 트루로 설정
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
        yield return new WaitForSeconds(deathAnimation.length); // Die 애니메이션 재생 시간

        DropMeat(); // 고기 드랍
        PhotonNetwork.Destroy(gameObject); // 네트워크 상에서 삭제
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
            // 내 데이터 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 다른 플레이어 데이터 수신
            netPosition = (Vector3)stream.ReceiveNext();
            netRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
