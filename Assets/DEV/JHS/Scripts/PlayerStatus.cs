using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    enum PlayerState
    {
        Idle, LackHunger, LackWarmth, NonWarmth, LackEverything, LackVeryBad, Die
    }
    // �Ϲ� ����, ��� ����, �±� ����, �±� ����, ���±� ����, ��� ���� �±� ����, ���
    enum SurroundingEnvironment
    {
        Warm, Cold, VeryCold
    }
    [SerializeField] public float moveSpeed; // �޸��� �ӵ�
    [SerializeField] public float playerMaxHP; // �ִ�ü��
    public float playerReducedHP; //���ҵ� �ִ� ü��
    public float playerHP; //���� ü��
    [SerializeField] public float hungerMax; // �ִ� ���
    public float hunger; // ���� ���
    [SerializeField] public float warmthMax; // �ִ� �±�
    public float warmth; // ���� �±�
    // ĳ���� ����
    PlayerState state = PlayerState.Idle;
    // �ֺ� ȯ��
    SurroundingEnvironment environment = SurroundingEnvironment.Warm;
    // ��Ⱑ ������ ��� : 20�� ���Ϸ� �������� ��� => �ִ�ü�� 65�� ����, ������ ����
    // ������ �Ծ ȸ�� ����
    // �±Ⱑ ������ ��� : 20�� ���Ϸ� �������� ��� => �̵��ӵ� ���� �������� 
    // �ٶ������� ��� => ���������� �÷��̾� ü�¿� �������� �ش�
    // ȯ�� ���°� Warm�� ��� ���� ȸ��

    private void Start()
    {
        playerHP = playerMaxHP;
        hunger = hungerMax;
        warmth = warmthMax;
    }

    private void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.LackHunger:
                break;
            case PlayerState.LackWarmth:
                break;
            case PlayerState.NonWarmth:
                break;
            case PlayerState.LackEverything:
                break;
            case PlayerState.LackVeryBad:
                break;
            case PlayerState.Die:
                break;
        }
    }


    // ü�� ����
    public void TakeHP(float damage)
    {
        playerHP -= damage;
        Debug.Log($"���� ü�� : {playerHP}");
        // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (playerHP <= 0)
        {
            state = PlayerState.Die;
        }
    }
    // ü�� ȸ��
    public void HealHP(float heal)
    {
        playerHP += heal;
        // ü���� ���ҵ� �ִ�ü�º��� Ŀ���� �ʴ´�.
        if (playerHP > playerReducedHP)
        {
            playerHP = playerReducedHP;
        }
        Debug.Log($"���� ü�� : {playerHP}");
    }
    // ��� ����
    public void TakeHunger(float damage)
    {
        hunger -= damage;
        Debug.Log($"���� ��� : {hunger}");
        // ��Ⱑ �ִ������ 20�� ���Ϸ� ������ ���
        if (hunger <= hungerMax/5)
        {
            // ���� NonWarmth���� ���ٸ� LackVeryBad
            if (state == PlayerState.NonWarmth)
                state = PlayerState.LackVeryBad;
            else if (state == PlayerState.LackWarmth)
                state = PlayerState.LackEverything;
            else if (state == PlayerState.Idle)
                state = PlayerState.LackHunger;
        }
    }
    // ��� ����
    public void HealHunger(float heal)
    {
        hunger += heal;
        // �ִ� ��⸦ ���� �ʰ� ȸ��
        if (hunger > hungerMax)
        {
            hunger = hungerMax;
        }
        Debug.Log($"���� ��� : {hunger}");
        // ��Ⱑ �ִ������ 20�� �ʰ��� ���
        if (hunger > hungerMax / 5)
        {
            if (state == PlayerState.LackVeryBad)
                state = PlayerState.NonWarmth;
            else if (state == PlayerState.LackEverything)
                state = PlayerState.LackWarmth;
            else if (state == PlayerState.LackHunger)
                state = PlayerState.Idle;
        }
    }
    // �±� ����
    public void TakeWarmth(float damage)
    {
        warmth -= damage;
        Debug.Log($"���� �±� : {warmth}");
        // �±� 20�� ���ϸ� 
        if (warmth <= 0)
        {
            warmth = 0;
            // ��⵵ 20�� ��� 
            if (state == PlayerState.LackHunger || state == PlayerState.LackEverything)
            {
                state = PlayerState.LackVeryBad;
            }
            else
                state = PlayerState.NonWarmth;
        }
        else if (warmth <= warmthMax/5)
        {
            if (state == PlayerState.LackHunger)
            {
                state = PlayerState.LackEverything;
            }
            else
                state = PlayerState.LackWarmth;
        }
    }

    // �±� ����
    public void HealWarmth(float heal)
    {
        warmth += heal;
        // �ִ� �±⸦ ���� �ʰ� ȸ��
        if (warmth > warmthMax)
        {
            warmth = warmthMax;
        }
        Debug.Log($"���� �±� : {warmth}");
        // �±� 20�� ���ϸ� �±� ���� �±Ⱑ 20 �� �̻��� ��� ���� �ذ�

        // �±Ⱑ 0���� ũ�ų� ���� �ִ� �±��� 20�� ���� �۰ų� ������
        if (warmth > 0 && warmth <= warmthMax / 5)
        {
            // ��� ���� �±� ���� => ��� ���� �±� ��������
            if (state == PlayerState.LackVeryBad)
            {
                state = PlayerState.LackEverything;
            }
            // �±� ���� => �±� ���� ����
            else if ( state == PlayerState.NonWarmth)
            {
                state = PlayerState.LackWarmth;
            }
        }
        // �±��� 20�� ���� Ŭ�� 
        else if (warmth >= warmthMax / 5)
        {
            // ��� ����, �±� ���� �±� ���� �϶� ��� �������θ� 
            if (state == PlayerState.LackVeryBad || state == PlayerState.LackEverything)
            {
                state = PlayerState.LackHunger;
            }
            // �±� ���� �±���� �϶� �������� ����
            else if (state == PlayerState.NonWarmth || state == PlayerState.LackWarmth)
                state = PlayerState.Idle;
        }
    }
}
