using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    WoodSpawner woodSpawner;
    OreSpawner oreSpawner;
    FruitSpawner fruitSpawner;

    public int maxHp;
    public int curHp;

    private Vector3 startPos; // 처음 스폰 위치를 기억
    public Transform startItem;
    [SerializeField] int second; // 리스폰 시간

    private void OnEnable()
    {
        StopCoroutine(DieRoutine());
    }

    private void Start()
    {
        curHp = maxHp;
        startPos = transform.position;
        startItem.position = new Vector3(startPos.x, startPos.y, startPos.z);
    }

    public void TakeDamage(int damage)
    {
        if (curHp <= 0)
        {
            Debug.Log("죽음");
            Die();
        }
        else
        {
            Debug.Log($"체력감소 {curHp}");
            curHp -= damage;
        }
    }

    private void Die()
    {
        // 각 자원의 종류에 따른 다른 item 생성
        if (gameObject.tag == "Wood")
        {
            woodSpawner = GetComponent<WoodSpawner>();
            woodSpawner.WoodSpawn(); 
            StartCoroutine(DieRoutine());
        }
        else if (gameObject.tag == "Ore")
        {
            oreSpawner = GetComponent<OreSpawner>();
            oreSpawner.OreSpawn();
            StartCoroutine(DieRoutine());
        }
        else if(gameObject.tag == "Fruit")
        {
            fruitSpawner = GetComponent<FruitSpawner>();
            fruitSpawner.FruitSpawn();
            StartCoroutine(DieRoutine());
        }
        else if(gameObject.tag == "Meat")
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        Debug.Log("자원 비활성화");
        gameObject.transform.position = new Vector3(startPos.x, startPos.y - 10, startPos.z);
        Debug.Log("자원 대기상태");
        yield return new WaitForSeconds(second);
        curHp = maxHp;
        gameObject.transform.position = startPos;
    }

    /*
        private void OnEnable()
        {
            StopCoroutine(DieRoutine());
        }

        private void Start()
        {
            curHp = maxHp;
            startPos = transform.position;
            startItem.position = new Vector3(startPos.x, startPos.y, startPos.z);
        }

        public void TakeDamage(int damage)
        {
            if (curHp <= 0)
            {
                Debug.Log("죽음");
                Die();
            }
            else
            {
                Debug.Log($"체력감소 {curHp}");
                curHp -= damage;
            }
        }

        private void Die()
        {
            StartCoroutine(DieRoutine());
        }

        IEnumerator DieRoutine()
        {
            // TODO: 아이템의 오브젝트 풀 기능으로 수정 필요
            Debug.Log("알맞은 아이템 생성");
            Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
            Debug.Log("자원 비활성화");
            gameObject.transform.position = new Vector3(startPos.x, startPos.y - 10, startPos.z);
            Debug.Log("자원 대기상태");
            yield return new WaitForSeconds(second);
            curHp = maxHp;
            gameObject.transform.position = startPos;
        }*/
}
