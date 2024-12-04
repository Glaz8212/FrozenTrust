using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMissionUI : MonoBehaviour
{
    MissionBoxInventory missionBoxInventory;
    [SerializeField] RectTransform itemContent; // 프리팹이 생성 될 위치
    [SerializeField] GameObject questPrefab; // 생성될 UI Prefab
    
    private void Awake()
    {
        // MissionBoxInventory.cs 참조
        missionBoxInventory = gameObject.transform.GetComponentInParent<MissionBoxInventory>();
    }

    private void Start()
    {
        Debug.LogWarning("UI세팅을 시작.");

        Debug.Log(missionBoxInventory.missionWoodCount);
        Debug.Log(missionBoxInventory.missionOreCount);
        Debug.Log(missionBoxInventory.missionFruitCount);

        if(missionBoxInventory.missionWoodCount > 0)
        {
            GameObject woodObj = missionBoxInventory.MakeItemObject("Wood");
            Item woodItem = woodObj.GetComponent<Item>();
            ItemData woodData = new ItemData(woodItem);
            SetUI(woodData, missionBoxInventory.missionWoodCount);
            missionBoxInventory.DeleteItemObject("Wood");
        }
        if(missionBoxInventory.missionOreCount > 0)
        {
            GameObject oreObj = missionBoxInventory.MakeItemObject("Ore");
            Item oreItem = oreObj.GetComponent<Item>();
            ItemData oreData = new ItemData(oreItem);
            SetUI(oreData, missionBoxInventory.missionWoodCount);
            missionBoxInventory.DeleteItemObject("Ore");
        }
        if(missionBoxInventory.missionFruitCount >0)
        {
            GameObject fruitObj = missionBoxInventory.MakeItemObject("Fruit");
            Item fruitItem = fruitObj.GetComponent<Item>();
            ItemData fruitData = new ItemData(fruitItem);
            SetUI(fruitData, missionBoxInventory.missionWoodCount);
            missionBoxInventory.DeleteItemObject("Fruit");
        }
    }

    private void SetUI(ItemData item, int count)
    {
        Debug.Log("SetUI함수 실행");
        GameObject itemUI = Instantiate(questPrefab, itemContent);
        ItemPrefab itemPrefab = itemUI.GetComponent<ItemPrefab>();
        itemPrefab.SetItemUI(item.itemData.itemSprite, item.itemData.itemName, count);
        item.itemPrefab = itemPrefab;
    }
    
}
