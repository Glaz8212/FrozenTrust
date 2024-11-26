using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AnimalState
{
    public IdleState(Animal animal) : base(animal) { }

    public override void Enter()
    {
        Debug.Log($"{animal.name} : Idle 상태");
    }

    public override void Update()
    {
        animal.OnIdleUpdate(this);
        
        GameObject detectedPlayer = animal.DetectPlayer();
        if (detectedPlayer != null)
        {
            animal.SetState(new AttackState(animal));
        }
    }

    public override void Exit() 
    {
        Debug.Log($"{animal.name} : Idle 상태 종료");
    }
}
