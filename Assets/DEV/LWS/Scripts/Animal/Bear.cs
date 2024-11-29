using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Animal, IPunObservable
{
    [SerializeField] private float sensingRange;

    protected override void UpdateBehaviour()
    {
        GameObject detectedPlayer = DetectPlayer();
        if (detectedPlayer != null)
        {
            RotateTowards(detectedPlayer.transform.position);
            photonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All);

            AttackPlayer(detectedPlayer);
        }
        else
        {
            photonView.RPC(nameof(PlayIdleAnimation), RpcTarget.All);
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

    private void AttackPlayer(GameObject player)
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 2f)
        {
            player.GetComponent<PlayerStatus>()?.TakeHP(Damage);
        }
    }
}
