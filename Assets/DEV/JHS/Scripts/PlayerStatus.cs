using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerStatus : MonoBehaviourPun
{
    public enum PlayerState
    {
        Idle, LackHunger, LackWarmth, NonWarmth, LackEverything, LackVeryBad, Die
    }
    // �Ϲ� ����, ��� ����, �±� ����, �±� ����, ���±� ����, ��� ���� �±� ����, ���
    public enum SurroundingEnvironment
    {
        Warm, Cold, VeryCold
    }
    // �ٲ�� ���� �Ǵ�
    private Coroutine environmentEffectCoroutine;

    [SerializeField] public float moveSpeed; // �޸��� �ӵ�
    [SerializeField] public float playerMaxHP; // �ִ�ü�� 1000
    public float playerReducedHP; //���ҵ� �ִ� ü��
    public float playerHP; //���� ü��
    [SerializeField] public float hungerMax; // �ִ� ��� 500
    public float hunger; // ���� ���
    [SerializeField] public float warmthMax; // �ִ� �±� 500
    public float warmth; // ���� �±�
    [SerializeField] Animator animator;
    // ĳ���� ����
    public PlayerState state = PlayerState.Idle;
    public bool playerDie = false;
    // �ֺ� ȯ��
    public SurroundingEnvironment environment = SurroundingEnvironment.Cold;
    // ��Ⱑ ������ ��� : 20�� ���Ϸ� �������� ��� => �ִ�ü�� 70�� ����
    // ������ �Ծ ȸ�� ����
    // �±Ⱑ ������ ��� : 20�� ���Ϸ� �������� ��� => �̵��ӵ� ���� �������� 
    // �ٶ������� ��� => ���������� �÷��̾� ü�¿� �������� �ش�
    // ȯ�� ���°� Warm�� ��� ���� ȸ��

    // ���� �����
    public bool ishungry = false;
    // ���� �����
    public bool iscold = false;

    private Coroutine healthTackCoroutine;
    private void Start()
    {
        environment = SurroundingEnvironment.Cold;
        playerHP = playerMaxHP;
        playerReducedHP = playerMaxHP;
        hunger = hungerMax;
        warmth = warmthMax;
        StartEnvironmentEffect();
    }

    private void Update()
    {
        if (!photonView.IsMine || playerDie) return;
        if (state == PlayerState.Die)
        {
            Die();
        }
        else
        {
            CheckState();
        }
        
        switch (state)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.LackHunger:
                LackHunger();
                break;
            case PlayerState.LackWarmth:
                LackWarmth();
                break;
            case PlayerState.NonWarmth:
                NonWarmth();
                break;
            case PlayerState.LackEverything:
                LackEverything();
                break;
            case PlayerState.LackVeryBad:
                LackVeryBad();
                break;         
        }

        // �÷��̾� ���� �Ǵ� �ʿ�
    }
    // �Ϲ� ����, ��� ����, �±� ����, �±� ����, ���±� ����, ��� ���� �±� ����, ���
    private void Idle()
    {
        // ����
        if (ishungry)
            HPBuff();
        if (iscold)
            SpeedBuff();
    }
    private void LackHunger()
    {
        // ��� ����
        if (!ishungry)
            HPDebuff(); // ��Ⱑ �����ϸ� �ִ� ü�� 70%�� ����
        if (iscold)
            SpeedBuff(); // �±�� ����ϹǷ� �̵��ӵ� ����
    }
    private void LackWarmth()
    {
        // �±� ����
        if (!iscold)
            SpeedDebuff(); // �±Ⱑ �����ϸ� �̵��ӵ� �������� ����      
        if (ishungry)
            HPBuff(); // ���� ����ϹǷ� �ִ� ü�� ����
    }
    private void NonWarmth()
    {
        // �±� ����
        if (!iscold)
            SpeedDebuff(); // �±Ⱑ ���� ������ �̵��ӵ� ����
        if (ishungry)
            HPBuff(); // ���� ����ϹǷ� �ִ� ü�� ����

        // ü�� ���� �ڵ� �߰� �ٶ�
        if (healthTackCoroutine == null)
        {
            healthTackCoroutine = StartCoroutine(HealthTackTime());
        }
    }
    private void LackEverything()
    {
        // ���±� ����
        if (!ishungry)
            HPDebuff(); // ��� �������� �ִ� ü�� ����
        if (!iscold)
            SpeedDebuff(); // �±� �������� �̵��ӵ� ����
    }
    private void LackVeryBad()
    {
        // ��� ���� �±� ����
        if (!ishungry)
            HPDebuff(); // ��� �������� �ִ� ü�� ����
        if (!iscold)
            SpeedDebuff(); // �±� �������� �̵��ӵ� ����
        // ü�� ���� �ڵ� �߰� �ٶ�
        if (healthTackCoroutine == null)
        {
            healthTackCoroutine = StartCoroutine(HealthTackTime());
        }
    }
    private void Die()
    {
        playerDie = true;

        // ��Ʈ��ũ RPC ȣ��
        photonView.RPC("PlayDeathAnimation", RpcTarget.All);

        Debug.Log("�÷��̾ ����߽��ϴ�.");
    }
    [PunRPC]
    private void PlayDeathAnimation()
    {
        // ��� Ŭ���̾�Ʈ���� �ִϸ��̼� ���
        animator.SetBool("isDead", true);
    }

    private IEnumerator HealthTackTime()
    {
        while (state == PlayerState.NonWarmth || state == PlayerState.LackVeryBad) // ���°� NonWarmth, LackVeryBad�� ���� ü���� ����
        {
            TakeHP(1); // ü���� 1�� ����
            yield return new WaitForSeconds(1f); // 1�� ���
        }

        // Coroutine ���� �� null�� �ʱ�ȭ
        healthTackCoroutine = null;
    }
    private void DamageStopState(PlayerState newState)
    {
        if ((newState != PlayerState.NonWarmth && newState != PlayerState.LackVeryBad) && healthTackCoroutine != null)
        {
            StopCoroutine(healthTackCoroutine);
            healthTackCoroutine = null;
        }
    }
    private void ChangeState(PlayerState newState)
    {
        DamageStopState(newState);
        if (state != newState)
        {
            Debug.Log($"���� ����: {state} -> {newState}");
            state = newState;
        }
    }
    // ����Ǵ� ���� Ȯ��
    private void CheckState()
    {
        if (playerDie)
        {
            ChangeState(PlayerState.Die);
            return;
        }

        // ���� �±⸦ �˻��Ͽ� ���� ����
        if (hunger <= hungerMax * 0.2f && warmth <= warmthMax * 0.2f)
        {
            if (warmth <= 0)
                ChangeState(PlayerState.LackVeryBad); // ��� ���� + �±� ����
            else
                ChangeState(PlayerState.LackEverything); // ��� ���� + �±� ����
        }
        else if (warmth <= warmthMax * 0.2f)
        {
            if (warmth <= 0)
                ChangeState(PlayerState.NonWarmth); // �±� ����
            else
                ChangeState(PlayerState.LackWarmth); // �±� ����
        }
        else if (hunger <= hungerMax * 0.2f)
        {
            ChangeState(PlayerState.LackHunger); // ��� ����
        }
        else
        {
            ChangeState(PlayerState.Idle); // ���� ����
        }
    }
    // ----------------ȯ�� ��� ------------------
    private void StartEnvironmentEffect()
    {
        if (environmentEffectCoroutine != null)
        {
            StopCoroutine(environmentEffectCoroutine);
        }

        switch (environment)
        {
            case SurroundingEnvironment.Warm:
                environmentEffectCoroutine = StartCoroutine(WarmEffect());
                break;
            case SurroundingEnvironment.Cold:
                environmentEffectCoroutine = StartCoroutine(ColdEffect());
                break;
            case SurroundingEnvironment.VeryCold:
                environmentEffectCoroutine = StartCoroutine(VeryColdEffect());
                break;
        }
    }
    // �ܺο��� ȯ�� ���� ����
    public void ChangeEnvironment(SurroundingEnvironment newEnvironment)
    {
        environment = newEnvironment;
        StartEnvironmentEffect();
    }

    private IEnumerator WarmEffect()
    {
        while (true)
        {
            TakeHunger(1f); // ��� 1 ����
            HealWarmth(4f); // �±� 4 ȸ��
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator ColdEffect()
    {
        while (true)
        {
            TakeHunger(2f); // ��� 2 ����
            TakeWarmth(2f); // �±� 2 ����
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator VeryColdEffect()
    {
        while (true)
        {
            TakeHunger(5f); // ��� 5 ����
            TakeWarmth(5f); // �±� 5 ����
            yield return new WaitForSeconds(1f);
        }
    }


    // ------------------����� �� ���� effect�ڵ�------------------------

    // ���ǵ� ����
    public void SpeedDebuff()
    {
        iscold = true;
        //�̵��ӵ� ���� ����
        moveSpeed = moveSpeed / 2;
        Debug.Log($"�̵��ӵ� {moveSpeed}�� ����");
    }
    // ���ǵ� ����
    public void SpeedBuff()
    {
        iscold = false;
        // �̵��ӵ� ����
        moveSpeed = moveSpeed * 2;
        Debug.Log($"�̵��ӵ� {moveSpeed}�� ����");
    }
    // �ִ� HP ����
    public void HPDebuff()
    {
        ishungry = true;
        float HP = playerMaxHP * 0.7f;
        playerReducedHP = HP;
        Debug.Log($"�ִ�ü�� {playerReducedHP}�� ����");

        if (playerHP > playerReducedHP)
        {
            playerHP = playerReducedHP;
        }
    }
    // HP ����
    public void HPBuff()
    {
        ishungry = false;
        float HP = playerMaxHP / 0.7f;
        playerReducedHP = HP;
        Debug.Log($"�ִ�ü�� {playerReducedHP} ����");

        if (playerHP > playerReducedHP)
        {
            playerHP = playerReducedHP;
        }
    }

    [PunRPC]
    // ü�� ����
    public void TakeHP(float damage)
    {
        playerHP -= damage;
        if (playerHP <= 0)
        {
            playerHP = 0;
            // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
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
    }
    // ��� ����
    public void TakeHunger(float damage)
    {
        hunger -= damage;
        if (hunger < 0)
            hunger = 0;
        // ��Ⱑ �ִ������ 20�� ���Ϸ� ������ ���
        // CheckState()�� Update���� �̹� �ϰ��ֱ⿡ �ּ�ó��
        /*
        if (hunger <= hungerMax/5)
        {
            // ���� NonWarmth���� ���ٸ� LackVeryBad
            if (state == PlayerState.NonWarmth)
                state = PlayerState.LackVeryBad;
            else if (state == PlayerState.LackWarmth)
                state = PlayerState.LackEverything;
            else if (state == PlayerState.Idle)
                state = PlayerState.LackHunger;
        }*/
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
        // ��Ⱑ �ִ������ 20�� �ʰ��� ���
        // CheckState()�� Update���� �̹� �ϰ��ֱ⿡ �ּ�ó��
        /*
        if (hunger > hungerMax / 5)
        {
            if (state == PlayerState.LackVeryBad)
                state = PlayerState.NonWarmth;
            else if (state == PlayerState.LackEverything)
                state = PlayerState.LackWarmth;
            else if (state == PlayerState.LackHunger)
                state = PlayerState.Idle;
        }*/
    }
    // �±� ����
    public void TakeWarmth(float damage)
    {
        warmth -= damage;
        if (warmth < 0)
            warmth = 0;
        // �±� 20�� ���ϸ� 
        // CheckState()�� Update���� �̹� �ϰ��ֱ⿡ �ּ�ó��
        /*
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
        }*/
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
        // �±� 20�� ���ϸ� �±� ���� �±Ⱑ 20 �� �̻��� ��� ���� �ذ�
        // CheckState()�� Update���� �̹� �ϰ��ֱ⿡ �ּ�ó��
        /*
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
        }*/
    }
}
