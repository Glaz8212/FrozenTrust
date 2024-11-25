using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    // ������
    [SerializeField] int weaponDamage;

    private PlayerAttacker playerAttacker;
    [SerializeField] BoxCollider weaponCollider;

    private bool isHit = false;

    private void Update()
    {
        if (weaponCollider.enabled == false)
            isHit = false ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;

        // Ȱ��ȭ�� ���� ������ ���� ������ �������� ���� // �θ� �÷��̾� ����
        if (other.CompareTag("Player") && other.gameObject != transform.root.gameObject)
        {
            //�浹�� �÷��̾��� ��ũ��Ʈ�� �ִ� ���� �Լ� ��������
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                // TakeHP �Լ� ȣ��� ������ ����
                playerStatus.TakeHP(weaponDamage);
                Debug.Log($"������ {weaponDamage}��ŭ ����");
            }
            else
            {
                Debug.LogWarning("�浹�� ��ü�� PlayerStatus ��ũ��Ʈ�� �����ϴ�.");
            }
            isHit = true;
        }
        else if (other.CompareTag("Resource"))
        {
            ResourceController resourceController = other.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                // TakeHP �Լ� ȣ��� ������ ����
                resourceController.TakeDamage(weaponDamage);
                Debug.Log($"������ {weaponDamage}��ŭ ����");
            }
            else
            {
                Debug.LogWarning("�浹�� ��ü�� ResourceController ��ũ��Ʈ�� �����ϴ�.");
            }
            // ������Ʈ ���� ����
            isHit = true;
        }
    }
}
