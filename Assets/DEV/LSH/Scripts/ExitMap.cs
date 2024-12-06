using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus gameObject = other.GetComponent<PlayerStatus>();
        gameObject.TakeHP(1000);
    }
}
