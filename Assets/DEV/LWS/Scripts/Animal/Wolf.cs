using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    private Vector3 target;

    public float sensingRange;

    public override void OnIdleUpdate(IdleState state)
    {
        if (target == Vector3.zero || Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = transform.position + new Vector3(
                Random.Range(-5f, 5f),
                0,
                Random.Range(-5f, 5f)
            );
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
