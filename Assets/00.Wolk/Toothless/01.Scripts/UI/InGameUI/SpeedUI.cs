using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    private Image _speedIcon;
    private Image _speedImageDelta;
    private TextMeshProUGUI _speedText;

    private void Awake()
    {
        _speedIcon = transform.Find("SpeedIcon").GetComponent<Image>();
        _speedImageDelta = transform.Find("SpeedCurrentIcon").GetComponent<Image>();
        _speedText = transform.Find("SpeedText").GetComponent<TextMeshProUGUI>();
    }

    public void SetSpeedUI(float speed)
    {
        
    }
}
