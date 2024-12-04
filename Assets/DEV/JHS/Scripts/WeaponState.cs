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
   

    // ��Ȱ��ȭ �� �ݶ��̴��� rigid
    [SerializeField] Collider collider1;
    [SerializeField] Collider collider2;
    [SerializeField] Rigidbody rigidbody1;
    [SerializeField] Collider weaponCollider;

    [SerializeField] public WeaponType weaponType;

    
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
}
