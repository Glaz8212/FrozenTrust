using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ResourceController : MonoBehaviourPun
{
    public int maxHp;
    public int curHp;

    public Vector3 startPos; // ó�� ���� ��ġ�� ���
    private Vector3 itemSpawnPos; // ������ ���� ��ġ
    private float range;
    [SerializeField] int second; // ������ �ð�

    private string resourceName;

    private void Start()
    {
        curHp = maxHp;
        startPos = transform.position;
        resourceName = gameObject.name;
        range = Random.Range(-2f, 2f);
        itemSpawnPos = new Vector3(startPos.x + range,
                                    startPos.y + 0.5f,
                                    startPos.z + range);
    }
    /* TODO : �������� ������ġ�� �ڿ� ������Ʈ�� ��ġ�� ��ġ�� �ʵ��� ������ �ʿ䰡 ����
     private void CheckRespawnPos()
    {
        while(itemSpawnPos == startPos)
        {
            range = Random.Range(-2f, 2f);
            itemSpawnPos = new Vector3(startPos.x + range,
                                        startPos.y + 0.5f,
                                        startPos.z + range);
        }

    }*/
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
            Die();
        }
    }

    private void Die()
    {
        StartCoroutine(DieRoutine());
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
                PhotonNetwork.Instantiate("YJE/Rock", itemSpawnPos, Quaternion.identity);
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
