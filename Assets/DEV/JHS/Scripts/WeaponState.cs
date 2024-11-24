using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    // ������
    [SerializeField] float weaponDamage;

    private float Damageturn;
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
            // �θ�� �ִ� �ݶ��̴��� �����ؾߵ� �߰��ٶ�@@
            isHit = true;
        }
        else if (other.CompareTag("Resource"))
        {
            // ������Ʈ ���� ����
            isHit = true;
            Debug.Log("�ڿ� �浹 ó��");
        }
    }
}
