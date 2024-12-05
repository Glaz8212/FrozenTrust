using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviourPun
{
    // 데미지
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
        Debug.Log("충돌01");
        if (!photonView.IsMine) return;
        Debug.Log("충돌02");
        if (isHit) return;
        Debug.Log("충돌03");
        // 오브젝트 공격 판정
        isHit = true;
        // 활성화된 공격 판정에 적이 들어오면 데미지를 적용 // 부모 플레이어 제외
        if (other.CompareTag("Player") && other.gameObject != gameObject.transform.root.gameObject)
        {
            //충돌한 플레이어의 스크립트에 있는 공격 함수 가져오기
            Debug.Log("충돌플레이어");
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                // TakeHP 함수 호출로 데미지 적용
                targetPhotonView.RPC("TakeHP", RpcTarget.All, weaponDamage);
                Debug.Log($"데미지 {weaponDamage}만큼 공격");
            }
            else
            {
                Debug.LogWarning("충돌한 객체에 PlayerStatus 스크립트가 없습니다.");
            }
        }
        else if (other.CompareTag("Tree") || other.CompareTag("Rock") || other.CompareTag("Grass"))
        {
            Debug.Log("충돌자원");
            ResourceController resourceController = other.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                // TakeHP 함수 호출로 데미지 적용
                resourceController.TakeDamage(weaponDamage);
                Debug.Log($"데미지 {weaponDamage}만큼 공격");
            }
            else
            {

                Debug.LogWarning("충돌한 객체에 ResourceController 스크립트가 없습니다.");
            }
        }
        else if (other.CompareTag("Animal"))
        {
            Debug.Log("충돌동물");
            PhotonView animals = other.GetComponent<PhotonView>();
            if (animals != null)
            {
                // TakeHP 함수 호출로 데미지 적용
                animals.RPC("TakeDamage", RpcTarget.All, weaponDamage);
                Debug.Log($"데미지 {weaponDamage}만큼 공격");
            }
            else
            {
                Debug.LogWarning("충돌한 객체에 Elk 스크립트가 없습니다.");
            }
        }
        StartCoroutine(ResetHitFlag());
    }
    private IEnumerator ResetHitFlag()
    {
        // 1.5초 후 충돌 플래그 초기화
        yield return new WaitForSeconds(1.5f);
        Debug.Log("공격 쿨타임");
        isHit = false;
    }
}
