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

    // TODO : UI 상호작용 창 닫혀있는지 확인하는 bool변수 - public
    public void BoxOpen()
    {
        Debug.Log("아이템상자 열기");
        IsUIOpen = true;
        // TODO : UI 상호작용 창 닫혀있는지 확인하는 bool변수 = ture; - return값
    }

    public void BoxClose()
    {
        Debug.Log("아이템상자 닫기");
        IsUIOpen = false;
        // TODO : UI 상호작용 창 닫혀있는지 확인하는 bool변수 = false; - return값

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
        Debug.Log("아이템상자 열기");
    }

    public void BoxClose()
    {
        Debug.Log("아이템상자 닫기");

    }
    */
}
