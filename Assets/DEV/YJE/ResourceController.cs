using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int maxHp;
    public int curHp;

    public Vector3 startPos; // ó�� ���� ��ġ�� ���
    [SerializeField] int second; // ������ �ð�


    private void Start()
    {
        curHp = maxHp;
        startPos = transform.position;
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
        yield return new WaitForSeconds(second);
        curHp = maxHp;
        gameObject.transform.position = startPos;
    }
}
