using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] PlayerInteraction playerInteraction;
    public bool IsUIOpen;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInteraction = other.GetComponent<PlayerInteraction>();
        }
    }

    public void Update()
    {
        if (!playerInteraction.isInteracting)
        {
            BoxClose();
        }
    }

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
