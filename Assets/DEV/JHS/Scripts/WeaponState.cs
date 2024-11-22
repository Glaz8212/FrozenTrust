using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    // 데미지
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
            // 활성화된 공격 판정에 적이 들어오면 데미지를 적용
            if (other.CompareTag("Player"))
            {
                //충돌한 플레이어의 스크립트에 있는 공격 함수 가져오기

            }
            else if (other.CompareTag("Resource"))
            {
                // 오브젝트 공격 판정

            }
        }
    }*/
}
