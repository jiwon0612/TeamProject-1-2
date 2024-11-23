using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    
    private float _min;
    private float _sic;
    private float _mSic;
    

    private void Awake()
    {
        timerText = transform.Find("TimerInfo").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _mSic += Time.deltaTime;
        if (_mSic > 1f)
        {
            _mSic = 0;
            _sic += 1f;
        }
        if (_sic > 60f)
        {
            _mSic = 0;
            _min += 1f;
        }
        
        timerText.text = $"{_min}:{_sic}:{Mathf.Round(_mSic * 100f)}";
    }

    public void ResetTimer()
    {
        _min = 0;
        _sic = 0;
        _mSic = 0;
    }
}
