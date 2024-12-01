using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    // ��Ʈ�� �̵� �� ������ ��ġ
    [SerializeField] private Transform exitPoint;

    // �ν����Ϳ��� ������ų ��Ʈ (������ ��Ʈ)
    [SerializeField] private Vent targetVent;

    private void OnTriggerStay(Collider other)
    {
        Debug.LogWarning("��Ʈ Ʈ���� ����");
         if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning("��Ʈ���� E����");
            PlayerMover player = other.GetComponent<PlayerMover>();

            if (player != null && IsTraitor(player))
            {
                Debug.LogWarning(player.photonView.Owner.ActorNumber);
                Debug.LogWarning("��Ʈ �̵�");
                UseVent(player);
            }
        }
            
    }

    private bool IsTraitor(PlayerMover player)
    {
        // ��Ʈ�ѹ��� 1�̸� (����ڸ�) true ��ȯ
        return player.photonView.Owner.ActorNumber == 1;
    }

    private void UseVent(PlayerMover player)
    {
        if (targetVent != null)
        {
            Debug.LogWarning("Ÿ�ٺ�Ʈ�� �̵�");

            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // CharacterController ��Ȱ��ȭ
                player.transform.position = targetVent.exitPoint.transform.position;
                controller.enabled = true;  // CharacterController ��Ȱ��ȭ
            }
            else
            {
                player.transform.position = targetVent.exitPoint.transform.position;
            }
        }
    }
}
