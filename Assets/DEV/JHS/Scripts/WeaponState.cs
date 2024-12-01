using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    // ���� Ÿ�� Ȯ��
    public enum WeaponType
    {
        Non, OneHanded, TwoHanded
    }
    // ������
    [SerializeField] int weaponDamage;

    private PlayerAttacker playerAttacker;
    [SerializeField] Collider weaponCollider;

    private bool isHit = false;

    [SerializeField] public WeaponType weaponType;

    private void Update()
    {
        if (weaponCollider.enabled == false)
            isHit = false ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;
        // ������Ʈ ���� ����
        isHit = true;
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
        }
    }
}
