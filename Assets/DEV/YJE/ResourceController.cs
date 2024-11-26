using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int maxHp;
    public int curHp;

    public Vector3 startPos; // 처음 스폰 위치를 기억
    [SerializeField] int second; // 리스폰 시간


    private void Start()
    {
        curHp = maxHp;
        startPos = transform.position;
    }

    /// <summary>
    /// Player와 Resource의 상호작용 함수 - 삭제 시 주의 요망
    /// Resource의 사망상태를 처리
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (curHp > 0)
        {
            Debug.Log($"체력감소 {curHp}");
            curHp -= damage;
        }
        if (curHp <= 0)
        {
            Debug.Log("죽음");
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
