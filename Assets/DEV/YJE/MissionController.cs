using Photon.Pun;
using UnityEngine;

/// <summary>
/// TODO: MissionBox���� UI���� �ʿ�
/// </summary>
public class MissionController : MonoBehaviour, IPunObservable
{
    public bool Is1Clear = false;
    public bool Is2Clear = false;
    public bool IsEndingClear = false;

    [SerializeField] GameObject missionBox1;
    [SerializeField] GameObject missionBox2;
    [SerializeField] GameObject Ending;

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

    private void Update()
    {
        if (Is1Clear)
        {
            missionBox2.gameObject.SetActive(true);
            missionBox1.gameObject.SetActive(false);
            GameSceneManager.Instance.nowPlayer.GetComponent<PlayerInteraction>().ResetInteraction();
        }
        else if(Is2Clear)
        {
            missionBox2.gameObject.SetActive(false);
            Ending.gameObject.SetActive(true);
            GameSceneManager.Instance.nowPlayer.GetComponent<PlayerInteraction>().ResetInteraction();
        }
        else if (IsEndingClear)
        {
            GameManager.Instance.CheckWin(IsEndingClear);
        }
    }


    /// <summary>
    /// Mission1�� Ŭ���� ���θ� Ȯ���ϴ� �Լ�
    /// </summary>
    public void Mission1ClearChecked()
    {
        // �̼� Ŭ��� �Ϸ�� ��쿡�� �Լ� ����
        if (Is1Clear)
        {
            Debug.Log("�̹� 1 Ŭ����");
            return;
        }
        // �̼�1 Ŭ��� �ȵ� ���
        else
        {
            Debug.Log("1 Ŭ����");
            Is1Clear = true;
        }
    }

    /// <summary>
    /// Mission2�� Ŭ���� ���θ� Ȯ���ϴ� �Լ�
    /// </summary>
    public void Mission2ClearChecked()
    {
        // �̼�1 Ŭ��� �Ϸ���� ���� ��쿡�� �Լ� ����
        if (!Is1Clear)
        {
            Debug.Log("1 Ŭ���� �̿ϼ�");
            return;
        }
        // �̼�1 Ŭ��� �Ȱ��
        else if (Is1Clear)
        {
            Debug.Log("2 Ŭ����");
            Is2Clear = true;
        }
    }

    /// <summary>
    /// ������ Ŭ���� ���θ� Ȯ���ϴ� �Լ�
    /// </summary>
    public void EndingClearChecked()
    {
        // �̼�1 �Ǵ� �̼�2�� Ŭ������� ���� ��� �Լ� ����
        if (!Is1Clear || !Is2Clear)
        {
            Debug.Log("1�̳� 2 Ŭ���� �̿ϼ�");
            return;
        }
        // �̼�1�� 2�� ��� Ŭ���� �� ���
        else if (Is1Clear && Is2Clear)
        {
            Debug.Log("���� Ŭ����");
            // ����Ȯ��
            IsEndingClear = true;
        }
    }

}
