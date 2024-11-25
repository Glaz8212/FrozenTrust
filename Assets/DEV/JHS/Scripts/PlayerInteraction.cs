using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Mission이나 ItemBox 콜라이더와 충돌 하였을때 
    // e 버튼을 클릭하면 그 콜라이더가 있는 오브젝트의 함수를 가져와 상호작용
    // 여러 콜라이더가 겹칠때 하나의 콜라이더랑만 상호작용
    // false값이 와야지만 다른 오브젝트와 상호작용

    // 상호작용 할 수 있는 오브젝트에 들어간 상호작용 스크립트 
    // private 스크립트명 함수명;
    // 상호작용 상태 판정
    public bool isInteracting = false;
    // 콜라이더 충돌 판정
    private bool isCollider = false;
    private void Update()
    {
        // E 키 입력 처리
        if (Input.GetKeyDown(KeyCode.E)&& !isInteracting) //&& 불러온 상호작용 함수 != null )
        {
            isInteracting = true; // 상호작용 중 상태로 변경
            
            // 상호작용 함수 호출
        }
        // 상호작용한 오브젝트에서 상호작용이 끝났을때 false값을 설정 한걸 가져와야됨
        // 그 값이 false라면을 else if 조건에 넣어줘야됨 
        else if (isCollider == false) // || 또는 상호작용 창을 닫았을때 
        {
            isInteracting = false; 
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 충돌했을떄 상호작용 가능한 오브젝트인지 확인
        isCollider = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // 충돌이 종료되면 상호작용 상태 초기화 해줌
        isCollider = false;
    }
}
