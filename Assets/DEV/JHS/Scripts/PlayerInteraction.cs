using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static WeaponState;

public class PlayerInteraction : MonoBehaviourPun
{ 
    public enum Type { Idle, Mission, ItemBox, Item, Weapon }
    public Type type = Type.Idle;
    // ��ȣ�ۿ� ���� ����
    public bool isInteracting = false;
    // �ݶ��̴� �浹 ����
    public Collider currentCollider;

    // ��ũ��Ʈ �� �ҷ����� ���� ������
    public MissionBox missionController;
    public BoxController boxController;
    public Item item;
    public WeaponState weapon;
    public GameObject weaponGameObject;
    public PlayerInventory playerInventory;
    private PlayerStatus status;
    private PlayerAttacker attacker;

    // �÷��̾��� ���Ⱑ ��ȯ�Ǵ� ��ġ
    [SerializeField] Transform playerHandTransform;
    
    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        attacker = GetComponent<PlayerAttacker>();
        // Inventory ������Ʈ�� ã�� PlayerInventory �Ҵ�
        if (playerInventory == null)
        {
            GameObject inventoryObject = GameObject.FindWithTag("PlayerInventory");
            if (inventoryObject != null)
            {
                playerInventory = inventoryObject.GetComponent<PlayerInventory>();
            }

            // Inventory ������Ʈ�� ã�� �� ���� ��� ��� ���
            if (playerInventory == null)
            {
                Debug.LogError("Inventory ������Ʈ���� PlayerInventory�� ã�� �� �����ϴ�!");
            }
        }
    }
    private void Update()
    {
        // E Ű �Է� ó��
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.E) && !isInteracting && currentCollider != null && status.playerDie == false)
        {
            isInteracting = true; // ��ȣ�ۿ� �� ���·� ����

            // ��ȣ�ۿ� �Լ� ȣ��
            switch (type)
            {
                case Type.Idle:
                    Debug.Log("��ȣ�ۿ� ������ ���°� �ƴմϴ�.");
                    break;
                case Type.Mission:
                    // null���� ������ ?. missionController�� ���� null�� �ƴҰ�쿡�� MissionBoxOpen�� ����. nulld�̸� MissionBoxOpen�� �������� ����
                    missionController?.MissionBoxOpen();
                    break;
                case Type.ItemBox:
                    //boxController���� BoxOpen����
                    boxController?.BoxOpen();
                    break;
                case Type.Item:
                    if(playerInventory.inventory.Count < 4)
                    {
                        item?.interaction(playerInventory);                        
                        ResetInteraction();
                    }                    
                    // �������� ���� 3����� 
                    // item�� interaction�� �κ��丮�� playerInventory���� �־� ����                    
                    break;
                case Type.Weapon:
                    if(currentCollider == null)
                    {
                        ResetInteraction();
                    }
                    else
                    {

                        if (weapon.weaponType == WeaponType.OneHanded)
                        {
                            MoverWeapon();

                            attacker?.InstallationWeapon(PlayerAttacker.Type.CloserWeapon);
                        }
                        else if (weapon.weaponType == WeaponType.TwoHanded)
                        {
                            MoverWeapon();

                            attacker?.InstallationWeapon(PlayerAttacker.Type.TwoHandWeapon);
                        }
                        ResetInteraction();
                    }
                    break;
            }
        }
        // �÷��̾� �����̰� ���� �ʾҰ� ���� ������Ʈ�� �����Ǿ� ������ Q�� �����ٸ�
        else if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q) && status.playerDie == false && weaponGameObject != null)
        {
            // �ڽĿ� �� ���� ����
            WeaponState weaponState = weaponGameObject.GetComponentInChildren<WeaponState>();
            if (weaponState != null)
            {
                attacker.ReleaseWeapon();
                Debug.Log("���� �ٽ� Ȱ��ȭ");
                // ���� Ȱ��ȭ �� �浹�� Ȱ��ȭ
                weaponState.photonView.RPC("Active", RpcTarget.All);
            }
            PhotonView weaponPhotonView = weaponGameObject.GetComponent<PhotonView>();
            if (weaponPhotonView != null && weaponPhotonView.IsMine)
            {
                weaponPhotonView.TransferOwnership(0); // ������ ��ȯ
            }
            weaponGameObject.transform.SetParent(null);
            weaponGameObject = null;          
        }
    }

    private void MoverWeapon()
    {
        PhotonView weaponPhotonView = weaponGameObject.GetComponent<PhotonView>();
        if (weaponPhotonView != null && !weaponPhotonView.IsMine)
        {
            weaponPhotonView.TransferOwnership(photonView.Owner);
        }
        weaponGameObject.transform.SetParent(playerHandTransform);

        weaponGameObject.transform.localPosition = Vector3.zero;
        weaponGameObject.transform.localRotation = Quaternion.Euler(0, -90, 0); // Y�� 90�� ȸ��

        WeaponState weaponState = weapon.GetComponentInChildren<WeaponState>();
        if (weaponState != null)
        {
            attacker?.SetWeaponState(weaponState);
            weaponState.photonView.RPC("Deactivate", RpcTarget.All);
        }
        attacker?.InstallationWeapon(PlayerAttacker.Type.TwoHandWeapon);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        // �ݶ��̴��� ���� �� ������ ����
        if (currentCollider != null) return;

        Debug.Log("����");
        if (other.CompareTag("Item"))
        {
            // currentCollider�� �浹�� other�� ����
            currentCollider = other;
            // item�� other�� Item �ҷ�����
            item = other.GetComponent<Item>();
            // Ÿ���� item�� ���� ������ Item ������ Idle
            type = item != null ? Type.Item : Type.Idle;
        }
        else if (other.CompareTag("Weapon") && weaponGameObject == null)
        {
            Debug.Log("����");
            // currentCollider�� �浹�� other�� ����
            currentCollider = other;
            // WeaponState�� �ƴ�, �浹�� ������Ʈ ��ü�� weapon���� ����
            weaponGameObject = other.gameObject;
            // �浹�� ������Ʈ�� �ڽĿ��� ��ġ��  WeaponState�� ����
            weapon = other.GetComponentInChildren<WeaponState>();
            Debug.Log($"{weaponGameObject}");
            type = weaponGameObject != null ? Type.Weapon : Type.Idle;
        }
        else if (other.CompareTag("Mission1") || other.CompareTag("Mission2") || other.CompareTag("Ending"))
        {
            // currentCollider�� �浹�� other�� ����
            currentCollider = other;
            // missionController�� other���� MissionBox �ҷ�����
            missionController = other.GetComponent<MissionBox>();
            // ���� ������ ��� : ���� = ���ǹ� ? ���ǹ��� ���� �� �� : ���ǹ��� ������ �� ��
            type = missionController != null ? Type.Mission : Type.Idle;
        }
        else if (other.CompareTag("ItemBox"))
        {
            // currentCollider�� �浹�� other�� ����
            currentCollider = other;
            // boxController�� other�� BoxController �ҷ�����
            boxController = other.GetComponent<BoxController>();
            // Ÿ���� boxController�� ���� �ִٸ� ItemBox �ƴϸ� Idle
            type = boxController != null ? Type.ItemBox : Type.Idle;
        }       
        else
        {
            Debug.Log("��ȣ�ۿ� �Ҽ��ִ� ������Ʈ�� �ƴմϴ�");
            type = Type.Idle;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� �÷��̾� ������ ���� ó��
        if (!photonView.IsMine) return;
        // �ݶ��̴����� ���� ���� currentCollider ���� ���ٸ� ���� #�� �ݶ��̴��� �� ���� �� ��찡 �ֳ�?
        if (other == currentCollider)
        {
            // �̼� �ݶ��̴��� ���� �ְ� �� ���� other�� MissionBox�� ���ٸ� �̼� ui�ݱ� ����
            if (missionController != null && missionController == other.GetComponent<MissionBox>())
            {
                missionController.MissionBoxClose();
            }
            // �ڽ� �ݶ��̴��� ���� �ְ� �� ���� other�� BoxController�� ���ٸ� �ڽ� ui�ݱ� ����
            else if (boxController != null && boxController == other.GetComponent<BoxController>())
            {
                boxController.BoxClose();
            }
            // �� ����
            weaponGameObject = null;
            ResetInteraction();
        }
    }

    // ��� �� ����
    public void ResetInteraction()
    {
        type = Type.Idle;
        currentCollider = null;
        missionController = null;
        boxController = null;
        weapon = null;
        item = null;
        isInteracting = false;
    }
}
