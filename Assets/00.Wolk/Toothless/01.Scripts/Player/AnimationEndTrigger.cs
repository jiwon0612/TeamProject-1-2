using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationEndTrigger : MonoBehaviour, IPlayerComponent
{
    public NotifyValue<bool> OnAnimationPlaying;

    private Player _player;
    public void Initialize(Player player)
    {
        _player = player;
        
        OnAnimationPlaying = new NotifyValue<bool>();
    }

    public void StartAnimation()
    {
        OnAnimationPlaying.Value = true;
    }

    public void EndAnimation()
    {
        OnAnimationPlaying.Value = false;
    }
}
