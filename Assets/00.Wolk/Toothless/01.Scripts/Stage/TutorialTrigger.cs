using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
    private LayerMask _whatIsTarget;
    private TextMeshProUGUI _text;
    private Action _action;

    public void Initialized(TextMeshProUGUI text, LayerMask target, Action callback)
    {
        _text = text;
        _whatIsTarget = target;
        _action = callback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _whatIsTarget) != 0)
        {
            _action?.Invoke();
        }
    }
}
