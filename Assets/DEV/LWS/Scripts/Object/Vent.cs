using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    // 벤트로 이동 후 나오는 위치
    [SerializeField] private Transform exitPoint;

    // 인스펙터에서 참조시킬 벤트 (도착할 벤트)
    [SerializeField] private Vent targetVent;

    private void OnTriggerStay(Collider other)
    {
        Debug.LogWarning("벤트 트리거 도착");
         if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning("벤트에서 E누름");
            PlayerMover player = other.GetComponent<PlayerMover>();

            if (player != null && IsTraitor(player))
            {
                Debug.LogWarning(player.photonView.Owner.ActorNumber);
                Debug.LogWarning("벤트 이동");
                UseVent(player);
            }
        }
            
    }

    private bool IsTraitor(PlayerMover player)
    {
        // 액트넘버가 1이면 (배신자면) true 반환
        return player.photonView.Owner.ActorNumber == 1;
    }

    private void UseVent(PlayerMover player)
    {
        if (targetVent != null)
        {
            Debug.LogWarning("타겟벤트로 이동");

            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // CharacterController 비활성화
                player.transform.position = targetVent.exitPoint.transform.position;
                controller.enabled = true;  // CharacterController 재활성화
            }
            else
            {
                player.transform.position = targetVent.exitPoint.transform.position;
            }
        }
    }
}
