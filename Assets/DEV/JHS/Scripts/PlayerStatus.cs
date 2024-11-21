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
    // 일반 상태, 허기 부족, 온기 부족, 온기 없음, 허기온기 부족, 허기 부족 온기 없음, 사망
    enum SurroundingEnvironment
    {
        Warm, Cold, VeryCold
    }
    [SerializeField] public float moveSpeed; // 달리기 속도
    [SerializeField] public float playerMaxHP; // 최대체력
    public float playerReducedHP; //감소된 최대 체력
    public float playerHP; //현재 체력
    [SerializeField] public float hungerMax; // 최대 허기
    public float hunger; // 현재 허기
    [SerializeField] public float warmthMax; // 최대 온기
    public float warmth; // 현재 온기
    // 캐릭터 상태
    PlayerState state = PlayerState.Idle;
    // 주변 환경
    SurroundingEnvironment environment = SurroundingEnvironment.Warm;
    // 허기가 부족할 경우 : 20퍼 이하로 내려갔을 경우 => 최대체력 65퍼 감소, 데미지 감소
    // 음식을 먹어서 회복 가능
    // 온기가 부족할 경우 : 20퍼 이하로 내려갔을 경우 => 이동속도 감소 절반으로 
    // 다떨어졌을 경우 => 지속적으로 플레이어 체력에 데미지를 준다
    // 환경 상태가 Warm일 경우 점차 회복

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


    // 체력 저하
    public void TakeHP(float damage)
    {
        playerHP -= damage;
        Debug.Log($"현재 체력 : {playerHP}");
        // 보스의 체력이 0 이하가 되면 상태를 Die로 변경
        if (playerHP <= 0)
        {
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
        Debug.Log($"현재 체력 : {playerHP}");
    }
    // 허기 저하
    public void TakeHunger(float damage)
    {
        hunger -= damage;
        Debug.Log($"현재 허기 : {hunger}");
        // 허기가 최대허기의 20퍼 이하로 내려갈 경우
        if (hunger <= hungerMax/5)
        {
            // 만약 NonWarmth상태 였다면 LackVeryBad
            if (state == PlayerState.NonWarmth)
                state = PlayerState.LackVeryBad;
            else if (state == PlayerState.LackWarmth)
                state = PlayerState.LackEverything;
            else if (state == PlayerState.Idle)
                state = PlayerState.LackHunger;
        }
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
        Debug.Log($"현재 허기 : {hunger}");
        // 허기가 최대허기의 20퍼 초과일 경우
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
    // 온기 저하
    public void TakeWarmth(float damage)
    {
        warmth -= damage;
        Debug.Log($"현재 온기 : {warmth}");
        // 온기 20퍼 이하면 
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
        }
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
        Debug.Log($"현재 온기 : {warmth}");
        // 온기 20퍼 이하면 온기 부족 온기가 20 퍼 이상일 경우 부족 해결

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
        }
    }
}
