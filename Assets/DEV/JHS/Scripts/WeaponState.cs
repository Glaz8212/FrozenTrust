using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    // ������
    [SerializeField] float weaponDamage;

    private PlayerAttacker playerAttacker;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (playerAttacker.attackTerm == true)
        {
            // Ȱ��ȭ�� ���� ������ ���� ������ �������� ����
            if (other.CompareTag("Player"))
            {
                //�浹�� �÷��̾��� ��ũ��Ʈ�� �ִ� ���� �Լ� ��������

            }
            else if (other.CompareTag("Resource"))
            {
                // ������Ʈ ���� ����

            }
        }
    }*/
}
