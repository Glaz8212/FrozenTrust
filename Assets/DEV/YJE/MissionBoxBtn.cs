using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBoxBtn : MonoBehaviour
{
    MissionBoxInventory missionBoxInventory;
    [SerializeField] GameObject applyBtn;

    private void Awake()
    {
        missionBoxInventory = gameObject.GetComponent<MissionBoxInventory>();
    }

    private void Update()
    {
        if(missionBoxInventory.IsWoodChecked && missionBoxInventory.IsOreChecked && missionBoxInventory.IsFruitChecked)
        {
            applyBtn.SetActive(true);
        }
        else 
        {
            applyBtn.SetActive(false);
        }
    }
}
