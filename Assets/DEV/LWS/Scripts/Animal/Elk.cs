using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Elk : Animal, IPunObservable
{
    [SerializeField] private Vector3[] patrolPoints;
    private int currentPointIndex = 0;

    protected override void UpdateBehaviour()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 target = patrolPoints[currentPointIndex];
        RotateTowards(target);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        photonView.RPC(nameof(PlayMoveAnimation), RpcTarget.All);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}
