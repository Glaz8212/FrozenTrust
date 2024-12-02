using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾� �κ��丮 ������ ���� ��
/// �������� �θ������Ʈ�� ClickedItem.cs�� �ִ� PlayerAddBox()�Լ��� Onclicked �� ����
/// </summary>
public class PlayerItemBtn : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] PlayerClickedItem playerClickedItem;

    private void OnEnable()
    {
        Debug.LogError("��ư�������");
        playerClickedItem = gameObject.GetComponentInParent<PlayerClickedItem>(); // �θ������Ʈ�� ���� ClickedItem.cs
        btn = gameObject.GetComponent<Button>(); // ���� ������Ʈ�� ��ư
        Debug.Log("�÷��̾� ������ ������ ��ư ����");
        btn.onClick.AddListener(playerClickedItem.PlayerAddBox); // �Լ��� ����
        Debug.LogError("��ư ���� �Ϸ�");
    }
}
