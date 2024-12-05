using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInventoryList : MonoBehaviour
{
    MissionBoxInventory missionBoxInventory;
    public MissionBoxInventory[] missionInventoryList;

    private void Start()
    {
        missionInventoryList = new MissionBoxInventory[gameObject.transform.childCount];

        for( int i = 0; i < missionInventoryList.Length; i++)
        {
            missionBoxInventory= gameObject.transform.GetChild(i).GetComponent<MissionBoxInventory>();
            missionInventoryList[i] = missionBoxInventory;
        }
    }
}
