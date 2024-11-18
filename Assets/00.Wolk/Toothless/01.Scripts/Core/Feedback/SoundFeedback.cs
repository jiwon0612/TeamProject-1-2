using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundFeedback : Feedback
{
    [SerializeField] private SoundDataSO soundData;
    public override void PlayFeedback()
    {
        SoundPlayer soundPlayer = PoolManager.Instance.Pop("SoundPlayer") as SoundPlayer;
        soundPlayer.transform.position = transform.position;    
        soundPlayer.PlaySound(soundData);
    }

    public override void StopFeedback()
    {
        
    }
}
