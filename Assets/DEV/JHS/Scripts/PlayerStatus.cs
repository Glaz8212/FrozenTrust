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
    // 일반 상태, 허기 부족, 온기 부족, 온기 없음, 허기온기 부족, 허기 부족 온기 없음, 사망
    public enum SurroundingEnvironment
    {
        Warm, Cold, VeryCold
    }
    // 바뀌는 상태 판단
    private Coroutine environmentEffectCoroutine;

    [SerializeField] public float moveSpeed; // 달리기 속도
    [SerializeField] public float playerMaxHP; // 최대체력 1000
    public float playerReducedHP; //감소된 최대 체력
    public float playerHP; //현재 체력
    [SerializeField] public float hungerMax; // 최대 허기 500
    public float hunger; // 현재 허기
    [SerializeField] public float warmthMax; // 최대 온기 500
    public float warmth; // 현재 온기
    [SerializeField] Animator animator;
    // 캐릭터 상태
    public PlayerState state = PlayerState.Idle;
    public bool playerDie = false;
    // 주변 환경
    public SurroundingEnvironment environment = SurroundingEnvironment.Cold;
    // 허기가 부족할 경우 : 20퍼 이하로 내려갔을 경우 => 최대체력 70퍼 감소
    // 음식을 먹어서 회복 가능
    // 온기가 부족할 경우 : 20퍼 이하로 내려갔을 경우 => 이동속도 감소 절반으로 
    // 다떨어졌을 경우 => 지속적으로 플레이어 체력에 데미지를 준다
    // 환경 상태가 Warm일 경우 점차 회복

    // 더위 디버프
    public bool ishungry = false;
    // 추위 디버프
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

        // 플레이어 상태 판단 필요
    }
    // 일반 상태, 허기 부족, 온기 부족, 온기 없음, 허기온기 부족, 허기 부족 온기 없음, 사망
    private void Idle()
    {
        // 정상
        if (ishungry)
            HPBuff();
        if (iscold)
            SpeedBuff();
    }
    private void LackHunger()
    {
        // 허기 부족
        if (!ishungry)
            HPDebuff(); // 허기가 부족하면 최대 체력 70%로 감소
        if (iscold)
            SpeedBuff(); // 온기는 충분하므로 이동속도 복구
    }
    private void LackWarmth()
    {
        // 온기 부족
        if (!iscold)
            SpeedDebuff(); // 온기가 부족하면 이동속도 절반으로 감소      
        if (ishungry)
            HPBuff(); // 허기는 충분하므로 최대 체력 복구
    }
    private void NonWarmth()
    {
        // 온기 없음
        if (!iscold)
            SpeedDebuff(); // 온기가 전혀 없으면 이동속도 감소
        if (ishungry)
            HPBuff(); // 허기는 충분하므로 최대 체력 복구

        // 체력 감소 코드 추가 바람
        if (healthTackCoroutine == null)
        {
            healthTackCoroutine = StartCoroutine(HealthTackTime());
        }
    }
    private void LackEverything()
    {
        // 허기온기 부족
        if (!ishungry)
            HPDebuff(); // 허기 부족으로 최대 체력 감소
        if (!iscold)
            SpeedDebuff(); // 온기 부족으로 이동속도 감소
    }
    private void LackVeryBad()
    {
        // 허기 부족 온기 없음
        if (!ishungry)
            HPDebuff(); // 허기 부족으로 최대 체력 감소
        if (!iscold)
            SpeedDebuff(); // 온기 없음으로 이동속도 감소
        // 체력 감소 코드 추가 바람
        if (healthTackCoroutine == null)
        {
            healthTackCoroutine = StartCoroutine(HealthTackTime());
        }
    }
    private void Die()
    {
        playerDie = true;

        // 네트워크 RPC 호출
        photonView.RPC("PlayDeathAnimation", RpcTarget.All);

        Debug.Log("플레이어가 사망했습니다.");
    }
    [PunRPC]
    private void PlayDeathAnimation()
    {
        // 모든 클라이언트에서 애니메이션 재생
        animator.SetBool("isDead", true);
    }

    private IEnumerator HealthTackTime()
    {
        while (state == PlayerState.NonWarmth || state == PlayerState.LackVeryBad) // 상태가 NonWarmth, LackVeryBad일 때만 체력을 감소
        {
            TakeHP(1); // 체력을 1씩 감소
            yield return new WaitForSeconds(1f); // 1초 대기
        }

        // Coroutine 종료 시 null로 초기화
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
            Debug.Log($"상태 변경: {state} -> {newState}");
            state = newState;
        }
    }
    // 변경되는 상태 확인
    private void CheckState()
    {
        if (playerDie)
        {
            ChangeState(PlayerState.Die);
            return;
        }

        // 허기와 온기를 검사하여 상태 변경
        if (hunger <= hungerMax * 0.2f && warmth <= warmthMax * 0.2f)
        {
            if (warmth <= 0)
                ChangeState(PlayerState.LackVeryBad); // 허기 부족 + 온기 없음
            else
                ChangeState(PlayerState.LackEverything); // 허기 부족 + 온기 부족
        }
        else if (warmth <= warmthMax * 0.2f)
        {
            if (warmth <= 0)
                ChangeState(PlayerState.NonWarmth); // 온기 없음
            else
                ChangeState(PlayerState.LackWarmth); // 온기 부족
        }
        else if (hunger <= hungerMax * 0.2f)
        {
            ChangeState(PlayerState.LackHunger); // 허기 부족
        }
        else
        {
            ChangeState(PlayerState.Idle); // 정상 상태
        }
    }
    // ----------------환경 요소 ------------------
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
    // 외부에서 환경 변경 가능
    public void ChangeEnvironment(SurroundingEnvironment newEnvironment)
    {
        environment = newEnvironment;
        StartEnvironmentEffect();
    }

    private IEnumerator WarmEffect()
    {
        while (true)
        {
            TakeHunger(1f); // 허기 1 감소
            HealWarmth(4f); // 온기 4 회복
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator ColdEffect()
    {
        while (true)
        {
            TakeHunger(2f); // 허기 2 감소
            TakeWarmth(2f); // 온기 2 감소
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator VeryColdEffect()
    {
        while (true)
        {
            TakeHunger(5f); // 허기 5 감소
            TakeWarmth(5f); // 온기 5 감소
            yield return new WaitForSeconds(1f);
        }
    }


    // ------------------디버프 및 스텟 effect코드------------------------

    // 스피드 감소
    public void SpeedDebuff()
    {
        iscold = true;
        //이동속도 감소 절반
        moveSpeed = moveSpeed / 2;
        Debug.Log($"이동속도 {moveSpeed}로 감소");
    }
    // 스피드 복구
    public void SpeedBuff()
    {
        iscold = false;
        // 이동속도 복구
        moveSpeed = moveSpeed * 2;
        Debug.Log($"이동속도 {moveSpeed}로 증가");
    }
    // 최대 HP 감소
    public void HPDebuff()
    {
        ishungry = true;
        float HP = playerMaxHP * 0.7f;
        playerReducedHP = HP;
        Debug.Log($"최대체력 {playerReducedHP}로 감소");

        if (playerHP > playerReducedHP)
        {
            playerHP = playerReducedHP;
        }
    }
    // HP 복구
    public void HPBuff()
    {
        ishungry = false;
        float HP = playerMaxHP / 0.7f;
        playerReducedHP = HP;
        Debug.Log($"최대체력 {playerReducedHP} 복구");

        if (playerHP > playerReducedHP)
        {
            playerHP = playerReducedHP;
        }
    }

    [PunRPC]
    // 체력 저하
    public void TakeHP(float damage)
    {
        playerHP -= damage;
        if (playerHP <= 0)
        {
            playerHP = 0;
            // 보스의 체력이 0 이하가 되면 상태를 Die로 변경
            state = PlayerState.Die;
        }               
    }
    // 체력 회복
    public void HealHP(float heal)
    {
        playerHP += heal;
        // 체력은 감소된 최대체력보다 커지지 않는다.
        if (playerHP > playerReducedHP)
        {
            playerHP = playerReducedHP;
        }
    }
    // 허기 저하
    public void TakeHunger(float damage)
    {
        hunger -= damage;
        if (hunger < 0)
            hunger = 0;
        // 허기가 최대허기의 20퍼 이하로 내려갈 경우
        // CheckState()가 Update에서 이미 하고있기에 주석처리
        /*
        if (hunger <= hungerMax/5)
        {
            // 만약 NonWarmth상태 였다면 LackVeryBad
            if (state == PlayerState.NonWarmth)
                state = PlayerState.LackVeryBad;
            else if (state == PlayerState.LackWarmth)
                state = PlayerState.LackEverything;
            else if (state == PlayerState.Idle)
                state = PlayerState.LackHunger;
        }*/
    }
    // 허기 증가
    public void HealHunger(float heal)
    {
        hunger += heal;
        // 최대 허기를 넘지 않게 회복
        if (hunger > hungerMax)
        {
            hunger = hungerMax;
        }
        // 허기가 최대허기의 20퍼 초과일 경우
        // CheckState()가 Update에서 이미 하고있기에 주석처리
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
    // 온기 저하
    public void TakeWarmth(float damage)
    {
        warmth -= damage;
        if (warmth < 0)
            warmth = 0;
        // 온기 20퍼 이하면 
        // CheckState()가 Update에서 이미 하고있기에 주석처리
        /*
        if (warmth <= 0)
        {
            warmth = 0;
            // 허기도 20퍼 라면 
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

    // 온기 증가
    public void HealWarmth(float heal)
    {
        warmth += heal;
        // 최대 온기를 넘지 않게 회복
        if (warmth > warmthMax)
        {
            warmth = warmthMax;
        }
        // 온기 20퍼 이하면 온기 부족 온기가 20 퍼 이상일 경우 부족 해결
        // CheckState()가 Update에서 이미 하고있기에 주석처리
        /*
        // 온기가 0보다 크거나 같고 최대 온기의 20퍼 보다 작거나 같을때
        if (warmth > 0 && warmth <= warmthMax / 5)
        {
            // 허기 부족 온기 없음 => 허기 부족 온기 부족으로
            if (state == PlayerState.LackVeryBad)
            {
                state = PlayerState.LackEverything;
            }
            // 온기 없음 => 온기 부족 으로
            else if ( state == PlayerState.NonWarmth)
            {
                state = PlayerState.LackWarmth;
            }
        }
        // 온기의 20퍼 보다 클때 
        else if (warmth >= warmthMax / 5)
        {
            // 허기 부족, 온기 없음 온기 부족 일때 허기 부족으로만 
            if (state == PlayerState.LackVeryBad || state == PlayerState.LackEverything)
            {
                state = PlayerState.LackHunger;
            }
            // 온기 부족 온기없음 일때 정상으로 변경
            else if (state == PlayerState.NonWarmth || state == PlayerState.LackWarmth)
                state = PlayerState.Idle;
        }*/
    }
}
