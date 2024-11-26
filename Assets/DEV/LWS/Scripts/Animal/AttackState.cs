using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AnimalState
{
    private GameObject targetPlayer;
    public AttackState(Animal animal) : base(animal) { }

    public override void Enter()
    {
        targetPlayer = animal.DetectPlayer();
        Debug.Log($"{animal.name} : Attack 상태");
        if (targetPlayer != null)
        {
            animal.PlayAnimation("Attack");
        }
    }

    public override void Update()
    {
        if (targetPlayer == null)
            return;

        float distance = Vector3.Distance(animal.transform.position, targetPlayer.transform.position);

        if (distance < 2f)
        {
            targetPlayer.GetComponent<PlayerStatus>()?.TakeHP(animal.Damage);
        }
    }

    public override void Exit()
    {
        Debug.Log($"{animal.name} : Attack 상태 종료");
    }
}
