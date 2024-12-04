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
        Debug.Log("�浹01");
        if (!photonView.IsMine) return;
        Debug.Log("�浹02");
        if (isHit) return;
        Debug.Log("�浹03");
        // ������Ʈ ���� ����
        isHit = true;
        // Ȱ��ȭ�� ���� ������ ���� ������ �������� ���� // �θ� �÷��̾� ����
        if (other.CompareTag("Player") && other.gameObject != gameObject.transform.root.gameObject)
        {
            //�浹�� �÷��̾��� ��ũ��Ʈ�� �ִ� ���� �Լ� ��������
            Debug.Log("�浹�÷��̾�");
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                // TakeHP �Լ� ȣ��� ������ ����
                targetPhotonView.RPC("TakeHP", RpcTarget.All, weaponDamage);
                Debug.Log($"������ {weaponDamage}��ŭ ����");
            }
            else
            {
                Debug.LogWarning("�浹�� ��ü�� PlayerStatus ��ũ��Ʈ�� �����ϴ�.");
            }            
        }
        else if (other.CompareTag("Resource"))
        {
            Debug.Log("�浹�ڿ�");
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
            Debug.Log("�浹����");
            PhotonView animals = other.GetComponent<PhotonView>();
            if (animals != null)
            { 
                // TakeHP �Լ� ȣ��� ������ ����
                animals.RPC("TakeDamage", RpcTarget.All, weaponDamage);
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
        Debug.Log("���� ��Ÿ��");
        isHit = false;
    }
}
