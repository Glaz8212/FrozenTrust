using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ResourceController : MonoBehaviourPun
{
    public int maxHp;
    public int curHp;

    public Vector3 startPos; // ó�� ���� ��ġ�� ���
    public Vector3 itemSpawnPos; // ������ ���� ��ġ
    private float range;
    [SerializeField] int second; // ������ �ð�

    private string resourceName;

    private void Start()
    {
        curHp = maxHp;
        startPos = transform.position;
        resourceName = gameObject.name;
    }
  
    /// <summary>
    /// Player�� Resource�� ��ȣ�ۿ� �Լ� - ���� �� ���� ���
    /// Resource�� ������¸� ó��
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (curHp > 0)
        {
            Debug.Log($"ü�°��� {curHp}");
            curHp -= damage;
        }
        if (curHp <= 0)
        {
            Debug.Log("����");
            photonView.RPC("Die", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void Die()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            range = Random.Range(1.5f, 2f);
            itemSpawnPos = new Vector3(Random.onUnitSphere.x * range + startPos.x,
                               startPos.y + 1.5f,
                               Random.onUnitSphere.z * range + startPos.z);
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        gameObject.transform.position = new Vector3(startPos.x, startPos.y - 10, startPos.z);
        switch (resourceName)
        {
            case "R_Tree":
                PhotonNetwork.Instantiate("YJE/Wood", itemSpawnPos, Quaternion.identity);
                break;
            case "R_Rock":
                PhotonNetwork.Instantiate("YJE/Ore", itemSpawnPos, Quaternion.identity);
                break;
            case "R_Grass":
                PhotonNetwork.Instantiate("YJE/Fruit", itemSpawnPos, Quaternion.identity);
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(second);
        curHp = maxHp;
        gameObject.transform.position = startPos;
    }
}
