using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviourPun
{
    // 무기 타입 확인
    public enum WeaponType
    {
        Non, OneHanded, TwoHanded
    }
   

    // 비활성화 할 콜라이더와 rigid
    [SerializeField] Collider collider1;
    [SerializeField] Collider collider2;
    [SerializeField] Rigidbody rigidbody1;
    [SerializeField] Collider weaponCollider;

    [SerializeField] public WeaponType weaponType;

    
    [PunRPC]
    public void Deactivate()
    {
        rigidbody1.isKinematic = true;// 물리 비활성화
        collider1.enabled = false;
        collider2.enabled = false; 
    }
    [PunRPC]
    public void Active()
    {
        rigidbody1.isKinematic = false; // 물리 활성화
        collider1.enabled = true;
        collider2.enabled = true; 
    }   
}
