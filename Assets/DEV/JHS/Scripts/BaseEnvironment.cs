using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatus;

public class BaseEnvironment : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                if (playerStatus.environment == SurroundingEnvironment.Cold)
                {
                    playerStatus.ChangeEnvironment(SurroundingEnvironment.Warm);
                }
                else if (playerStatus.environment == SurroundingEnvironment.Warm)
                {
                    playerStatus.ChangeEnvironment(SurroundingEnvironment.Cold);
                }
            }
        }
    }
}
