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

    private Vector3 startPos; // ó�� ���� ��ġ�� ���
    public Transform startItem;
    [SerializeField] int second; // ������ �ð�

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
        // �� �ڿ��� ������ ���� �ٸ� item ����
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
        Debug.Log("�ڿ� ��Ȱ��ȭ");
        gameObject.transform.position = new Vector3(startPos.x, startPos.y - 10, startPos.z);
        Debug.Log("�ڿ� ������");
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
        }*/
}
