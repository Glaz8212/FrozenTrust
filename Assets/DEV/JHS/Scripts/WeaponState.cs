using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviourPun
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

    // ��Ȱ��ȭ �� �ݶ��̴��� rigid
    [SerializeField] Collider collider1;
    [SerializeField] Collider collider2;
    [SerializeField] Rigidbody rigidbody1;
    private bool isHit = false;

    [SerializeField] public WeaponType weaponType;

    private void Update()
    {
        if (!weaponCollider.enabled)
        {
            isHit = false;
        }
    }
    [PunRPC]
    public void Deactivate()
    {
        rigidbody1.isKinematic = true;// ���� ��Ȱ��ȭ
        collider1.enabled = false;
        collider2.enabled = false; 
    }
    [PunRPC]
    public void Active()
    {
        rigidbody1.isKinematic = false; // ���� Ȱ��ȭ
        collider1.enabled = true;
        collider2.enabled = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
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
        else if (other.CompareTag("Animal"))
        {
            Elk animals = other.GetComponent<Elk>();
            if (animals != null)
            { 
                // TakeHP �Լ� ȣ��� ������ ����
                animals.TakeDamage(weaponDamage);
                Debug.Log($"������ {weaponDamage}��ŭ ����");
            }
            else
            {
                Debug.LogWarning("�浹�� ��ü�� Elk ��ũ��Ʈ�� �����ϴ�.");
            }
        }
        StartCoroutine(ResetHitFlag());
    }
    private IEnumerator ResetHitFlag()
    {
        // 1.5�� �� �浹 �÷��� �ʱ�ȭ
        yield return new WaitForSeconds(1.5f);
        isHit = false;
    }
}
