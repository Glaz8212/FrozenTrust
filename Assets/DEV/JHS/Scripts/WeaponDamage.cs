using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviourPun
{
    // ������
    [SerializeField] float weaponDamage;
    [SerializeField] Collider weaponCollider;

    private bool isHit = false;
    private void Update()
    {
        if (!weaponCollider.enabled)
        {
            isHit = false;
        }
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
        else if (other.CompareTag("Tree") || other.CompareTag("Rock") || other.CompareTag("Grass"))
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
