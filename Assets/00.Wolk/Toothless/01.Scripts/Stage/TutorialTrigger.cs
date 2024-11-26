using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialTrigger : MonoBehaviour
{
    public bool IsComplete { get; set; }
    
    private TutorialUI _owner;
    private string _tutorialText;
    private float _duration;
    private LayerMask _whatIsTarget;

    private bool _isReady;

    public void Initialized(TutorialUI owner, string text, float duration, LayerMask layerMask)
    {
        _owner = owner;
        _tutorialText = text;
        _duration = duration;
        _whatIsTarget = layerMask;
        
        _isReady = true;
        IsComplete = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _whatIsTarget) != 0)
        {
            if (IsComplete) return;
            
            _owner.ShowTutorial(_tutorialText, _duration,this);
        }
    }
}
