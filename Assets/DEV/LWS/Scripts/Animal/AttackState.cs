using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AnimalState
{
    private GameObject targetPlayer;
    private bool isAttacking = false;
    public AttackState(Animal animal) : base(animal) { }

    public override void Enter()
    {
        targetPlayer = animal.DetectPlayer();
        Debug.Log($"{animal.name} : Attack ป๓ลย");
        if (targetPlayer != null)
        {
            animal.PlayAttackAnimation();
            animal.StartCoroutine(AttackRoutine());
        }
    }

    public override void Update()
    {
        if (targetPlayer == null)
            return;
        
        animal.RotateTowardsTarget(targetPlayer.transform.position);

        float distance = Vector3.Distance(animal.transform.position, targetPlayer.transform.position);

        if (distance > 5f)
        {
            animal.SetState(new IdleState(animal));
            return;
        }
    }

    public override void Exit()
    {
        animal.PlayIdleAnimation();
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        while (isAttacking)
        {
            if (targetPlayer != null)
            {
                targetPlayer.GetComponent<PlayerStatus>()?.TakeHP(animal.Damage);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
