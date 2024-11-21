using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    
    private PlayerStatus status;
    private CharacterController controller;
    public float gravity = -9.8f;
    public float rotationSpeed = 360;

    private Vector3 velocity;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        status = GetComponent<PlayerStatus>();
        controller = GetComponent<CharacterController>();
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
        else if (direction != Vector3.zero)
        {
            direction.Normalize();
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
}
