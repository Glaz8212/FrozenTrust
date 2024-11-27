using UnityEngine;

/// <summary>
/// TODO: MissionBox���� UI���� �ʿ�
/// </summary>
public class MissionController : MonoBehaviour
{
    public bool Is1Clear = false;
    public bool Is2Clear = false;
    public bool IsEndingClear = false;

    public bool IsUIOpen;

    public void MissionBoxOpen()
    {
        Debug.Log("�̼ǻ��� ����");
        IsUIOpen = true;
        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = ture; - return��
    }
    public void MissionBoxClose()
    {
        Debug.Log("�̼ǻ��� �ݱ�");
        IsUIOpen = false;
        // TODO : UI ��ȣ�ۿ� â �����ִ��� Ȯ���ϴ� bool���� = false; - return��
    }


    // TODO: UI���� ��ư���� �����ϰ� �� ����
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
            // TODO : �̼� �������� ���� ������ Ȯ���Ͽ�
            //        �� ��� => �̼� Ŭ����
            //        ������ ��� => ����
            //        �κ��丮 ��� �߰� �� ���� �ʿ�
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
        // �̼�2 Ŭ��� �Ϸ�� ��� �Լ� ����
        if (Is2Clear)
        {
            Debug.Log("�̹� 2 Ŭ����");
            return;
        }
        // �̼�1 Ŭ��� �Ȱ��
        else if (Is1Clear)
        {
            // TODO : �̼� �������� ���� ������ Ȯ���Ͽ�
            //        �� ��� => �̼� Ŭ����
            //        ������ ��� => ����
            //        �κ��丮 ��� �߰� �� ���� �ʿ�
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
