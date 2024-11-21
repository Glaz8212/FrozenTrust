using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    
    private PlayerStatus status;
    public float rotationSpeed = 720;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        status = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        MovePosition();
    }

    private void MovePosition()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");

        if (direction == Vector3.zero) return;

        // �����¿� Ű�� ���� �Է��ϸ� �밢���� ������⿡ ������ ����ȭ��Ű�� Normalize()���
        direction.Normalize();

        transform.Translate(status.moveSpeed * Time.deltaTime * direction, Space.World);

        RotateToDirection(direction);
    }

    private void RotateToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
