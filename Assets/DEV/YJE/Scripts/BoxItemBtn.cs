using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾� �κ��丮 ������ ���� ��
/// �������� �θ������Ʈ�� ClickedItem.cs�� �ִ� BoxAddPlayer()�Լ��� Onclicked �� ����
/// </summary>
public class BoxItemBtn : MonoBehaviour
{
    public Button btn;
    private BoxClickedItem boxclickedItem;

    private void Start()
    {
        boxclickedItem = gameObject.GetComponentInParent<BoxClickedItem>(); // �θ������Ʈ�� ���� ClickedItem.cs
        btn = gameObject.GetComponent<Button>(); // ���� ������Ʈ�� ��ư
        Debug.Log("�����۹ڽ� ������ ������ ��ư ����");
        btn.onClick.AddListener(boxclickedItem.BoxAddPlayer); // �Լ��� ����
        Debug.LogError("��ư ���� �Ϸ�");

    }
}
