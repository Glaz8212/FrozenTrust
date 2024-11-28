using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class voicetester : MonoBehaviour
{
    [SerializeField] Image speaker;

    private PhotonVoiceView voiceview;

    private void Update()
    {
        this.speaker.enabled = this.voiceview.IsSpeaking;
    }
}
