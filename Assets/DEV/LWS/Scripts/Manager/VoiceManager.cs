using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Recorder recorder;
    [SerializeField] PlayerStatus status;

    private void Update()
    {
        if( status.state == PlayerStatus.PlayerState.Die)
        {
             recorder.TransmitEnabled = false;
        }
    }
}
