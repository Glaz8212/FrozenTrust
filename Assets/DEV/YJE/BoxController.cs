using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// TODO: ItemBox���� UI���� �ʿ�
/// </summary>
public class BoxController : MonoBehaviourPun
{
    public bool IsUIOpen;
    [SerializeField] GameObject UI_ItemBox;
    private GameSceneManager gameSceneManager;

    // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� - public
    public void BoxOpen()
    {
        Debug.Log("�����ۻ��� ����");
        IsUIOpen = true;
        UI_ItemBox.SetActive(true);
        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = ture; - return��
    }

    public void BoxClose()
    {
        Debug.Log("�����ۻ��� �ݱ�");
        IsUIOpen = false;
        UI_ItemBox.SetActive(false);

        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = false; - return��

    }




    /*
    [SerializeField] PlayerInteraction playerInteraction;

    public void Update()
    {
        if (playerInteraction.isInteracting)
        {
            BoxOpen();
        }
        else
        {
            BoxClose();
        }
    }

    public void BoxOpen()
    {
        Debug.Log("�����ۻ��� ����");
    }

    public void BoxClose()
    {
        Debug.Log("�����ۻ��� �ݱ�");

    }
    */
}
