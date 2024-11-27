using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerInven : MonoBehaviour
{
    public PlayerInventory playerInventory;


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
        }
    }
}
