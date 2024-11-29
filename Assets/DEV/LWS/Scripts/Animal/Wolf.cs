using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal, IPunObservable
{
    [SerializeField] private float sensingRange;
    private Vector3 target;

    protected override void UpdateBehaviour()
    {
        GameObject detectedPlayer = DetectPlayer();
        if (detectedPlayer != null)
        {
            RotateTowards(detectedPlayer.transform.position);
            PlayAttackAnimation();
            AttackPlayer(detectedPlayer);
        }
        else
        {
            MoveRandomly();
        }
    }
    private GameObject DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sensingRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                return hit.gameObject;
        }
        return null;
    }

    private void MoveRandomly()
    {
        if (target == Vector3.zero || Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = transform.position + new Vector3(
                Random.Range(-5f, 5f),
                0,
                Random.Range(-5f, 5f)
            );
        }

        RotateTowards(target);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        PlayMoveAnimation();
    }

    private void AttackPlayer(GameObject player)
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 2f)
        {
            player.GetComponent<PlayerStatus>()?.TakeHP(Damage);
        }
    }
}
