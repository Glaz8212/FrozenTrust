using UnityEngine;

/// <summary>
/// TODO: ItemBox���� UI���� �ʿ�
/// </summary>
public class BoxController : MonoBehaviour
{
    public bool IsUIOpen;

    // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� - public
    public void BoxOpen()
    {
        Debug.Log("�����ۻ��� ����");
        IsUIOpen = true;
        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = ture; - return��
    }

    public void BoxClose()
    {
        Debug.Log("�����ۻ��� �ݱ�");
        IsUIOpen = false;
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
