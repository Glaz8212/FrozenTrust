using Photon.Pun;
using UnityEngine;

/// <summary>
/// TODO: MissionBox들의 UI관리 필요
/// </summary>
public class MissionController : MonoBehaviour, IPunObservable
{
    public bool Is1Clear = false;
    public bool Is2Clear = false;
    public bool IsEndingClear = false;

    // TODO : missionBox의 활성화 비활성화는 GameManager에서 Is1Clear와 Is2Clear를 받아서 체크 필요
    // [SerializeField] GameObject missionBox1;
    // [SerializeField] GameObject missionBox2;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Is1Clear);
            stream.SendNext(Is2Clear);
            stream.SendNext(IsEndingClear);
        }
        else if (stream.IsReading)
        {
            Is1Clear = (bool)stream.ReceiveNext();
            Is2Clear = (bool)stream.ReceiveNext();
            IsEndingClear = (bool)stream.ReceiveNext();
        }
    }


    /// <summary>
    /// Mission1의 클리어 여부를 확인하는 함수
    /// </summary>
    public void Mission1ClearChecked()
    {
        // 미션 클리어가 완료된 경우에는 함수 종료
        if (Is1Clear)
        {
            Debug.Log("이미 1 클리어");
            return;
        }
        // 미션1 클리어가 안된 경우
        else
        {
            Debug.Log("1 클리어");
            Is1Clear = true;
            // missionBox2.gameObject.SetActive(true);
            // missionBox1.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Mission2의 클리어 여부를 확인하는 함수
    /// </summary>
    public void Mission2ClearChecked()
    {
        // 미션1 클리어가 완료되지 않은 경우에는 함수 종료
        if (!Is1Clear)
        {
            Debug.Log("1 클리어 미완성");
            return;
        }
        // 미션1 클리어가 된경우
        else if (Is1Clear)
        {
            Debug.Log("2 클리어");
            Is2Clear = true;
            // missionBox2.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 게임의 클리어 여부를 확인하는 함수
    /// </summary>
    public void EndingClearChecked()
    {
        // 미션1 또는 미션2가 클리어되지 않은 경우 함수 종료
        if (!Is1Clear || !Is2Clear)
        {
            Debug.Log("1이나 2 클리어 미완성");
            return;
        }
        // 미션1과 2가 모두 클리어 된 경우
        else if (Is1Clear && Is2Clear)
        {
            Debug.Log("엔딩 클리어");
            // 엔딩확인
            IsEndingClear = true;
        }
    }

}
