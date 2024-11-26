using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Animal
{
    public float sensingRange;
    public override void OnIdleUpdate(IdleState state)
    {

    }

    public override GameObject DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sensingRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                return hit.gameObject;
        }
        return null;
    }
}
