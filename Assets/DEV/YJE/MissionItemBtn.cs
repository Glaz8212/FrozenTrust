using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemBtn : MonoBehaviour
{
    public Button btn;
    private MissionClicekedItem missionClicekedItem;

    private void Start()
    {
        missionClicekedItem = gameObject.GetComponentInParent<MissionClicekedItem>();
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(missionClicekedItem.MissionBoxSteal);
        Debug.Log("버튼 연결 완료");
    }
}
