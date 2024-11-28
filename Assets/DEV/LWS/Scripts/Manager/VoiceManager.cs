using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Recorder recorder;
    [SerializeField] private PhotonVoiceView voiceView;

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("∑Î¿Ã æ∆¥‘");
            return;
        }

        if (recorder == null)
        {
            recorder = GetComponent<Recorder>();
        }

        if (voiceView == null)
        {
            voiceView = GetComponent<PhotonVoiceView>();
        }
    }
}
