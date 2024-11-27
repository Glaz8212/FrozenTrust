using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMover : MonoBehaviourPun, IPunObservable
{
    
    private PlayerStatus status;
    private CharacterController controller;
    [SerializeField] Animator animator;
    public float gravity = -9.8f;
    public float rotationSpeed = 360;

    private Vector3 velocity;
    private Vector3 netPosition;
    private Quaternion netRotation;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        status = GetComponent<PlayerStatus>();
        controller = GetComponent<CharacterController>();
        netPosition = transform.position;
        netRotation = transform.rotation;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (status.playerDie == false)
            {
                MovePosition();
            }
        }
        else
        {
            SmoothSync();
        }
    }
    // animation ���� isRunning isWalking isDead isKicking
    // isPunching_Left isPunching_Right
    private void MovePosition()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");

        if (direction == Vector3.zero)
        {
            animator.SetBool("isRunning", false);
            return;
        }
        else
        { 
            direction.Normalize();
            animator.SetBool("isRunning", true);
            RotateToDirection(direction);
        }

        Vector3 move = direction * status.moveSpeed;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);
        //transform.Translate(status.moveSpeed * Time.deltaTime * direction, Space.World);
    }

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
        if (animator != null)
        {
            bool isRunning = Vector3.Distance(transform.position, netPosition) > 0.1f; // �̵� �Ǵ�
            animator.SetBool("isRunning", isRunning);
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
