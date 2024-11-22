using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int maxHp;
    public int curHp;

    private Vector3 startPos; // ó�� ���� ��ġ�� ���
    [SerializeField] int second; // ������ �ð�
    [SerializeField] GameObject prefab; // ����� ������

    private void OnEnable()
    {
        StopCoroutine(DieRoutine());
    }

    private void Start()
    {
        curHp = maxHp;
        startPos = transform.position;
    }

    public void TakeDamage(int damage)
    {
        if (curHp <= 0)
        {
            Debug.Log("����");
            Die();
        }
        else
        {
            Debug.Log($"ü�°��� {curHp}");
            curHp -= damage;
        }
    }

    private void Die()
    {
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        // TODO: �������� ������Ʈ Ǯ ������� ���� �ʿ�
        Debug.Log("�˸��� ������ ����");
        Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        Debug.Log("�ڿ� ��Ȱ��ȭ");
        gameObject.transform.position = new Vector3(startPos.x, startPos.y - 10, startPos.z);
        Debug.Log("�ڿ� ������");
        yield return new WaitForSeconds(second);
        curHp = maxHp;
        gameObject.transform.position = startPos;
    }

}
