using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    // 무기 타입 확인
    public enum WeaponType
    {
        Non, OneHanded, TwoHanded
    }
    // 데미지
    [SerializeField] int weaponDamage;

    private PlayerAttacker playerAttacker;
    [SerializeField] Collider weaponCollider;

    // 비활성화 할 콜라이더와 rigid
    [SerializeField] Collider collider1;
    [SerializeField] Collider collider2;
    [SerializeField] Rigidbody rigidbody1;
    private bool isHit = false;

    [SerializeField] public WeaponType weaponType;

    private void Update()
    {
        if (weaponCollider.enabled == false)
            isHit = false ;
    }

    public void Deactivate()
    {
        rigidbody1.isKinematic = true;// 물리 비활성화
        collider1.enabled = false;
        collider2.enabled = false; 
    }
    public void Active()
    {
        rigidbody1.isKinematic = false; // 물리 활성화
        collider1.enabled = true;
        collider2.enabled = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;
        // 오브젝트 공격 판정
        isHit = true;
        // 활성화된 공격 판정에 적이 들어오면 데미지를 적용 // 부모 플레이어 제외
        if (other.CompareTag("Player") && other.gameObject != transform.root.gameObject)
        {
            //충돌한 플레이어의 스크립트에 있는 공격 함수 가져오기
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                // TakeHP 함수 호출로 데미지 적용
                playerStatus.TakeHP(weaponDamage);
                Debug.Log($"데미지 {weaponDamage}만큼 공격");
            }
            else
            {
                Debug.LogWarning("충돌한 객체에 PlayerStatus 스크립트가 없습니다.");
            }            
        }
        else if (other.CompareTag("Resource"))
        {
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
            Elk animals = other.GetComponent<Elk>();
            if (animals != null)
            { 
                // TakeHP 함수 호출로 데미지 적용
                animals.TakeDamage(weaponDamage);
                Debug.Log($"데미지 {weaponDamage}만큼 공격");
            }
            else
            {
                Debug.LogWarning("충돌한 객체에 Elk 스크립트가 없습니다.");
            }
        }
    }
}
