using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    [Header("UISetting")] 
    [SerializeField] private float corretionValue;
    [SerializeField] private float maxVisibleValue;
    
    private Image _speedImageDelta;
    private TextMeshProUGUI _speedText;

    private void Awake()
    {
        _speedImageDelta = transform.Find("SpeedCurrentIcon").GetComponent<Image>();
        _speedText = transform.Find("SpeedText").GetComponent<TextMeshProUGUI>();
    }

    public void SetSpeedUI(float speed)
    {
        float value = Mathf.Round(speed * corretionValue * 100) / 100;
        
        _speedImageDelta.fillAmount = value / maxVisibleValue;
        
        _speedText.text = $"{value}";
    }
}
