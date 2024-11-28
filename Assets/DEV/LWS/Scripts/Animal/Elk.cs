using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Elk : Animal
{
    private Vector3[] patrolPoints = { new Vector3(0, 0, 0), new Vector3(5, 0, 5), new Vector3(10, 0, 0) };
    private int currentPointIndex = 0;

    public override void OnIdleUpdate(IdleState state)
    {
        Vector3 target = patrolPoints[currentPointIndex];
        Vector3 dir = target - transform.position;

        RotateTowardsDirection(dir);
        
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);



        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }

        PlayMoveAnimation();
    }

    public override GameObject DetectPlayer()
    {
        return null;
    }

    [PunRPC]
    public override void SyncState(string state)
    {
        base.SyncState(state);
    }
}
