using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public struct TimeStruct
{
    public TimeStruct(float min, float sec, float mis)
    {
        Minutes = min;
        Seconds = sec;
        Milliseconds = mis;
    }
    
    public float Minutes;
    public float Seconds;
    public float Milliseconds;
}

public class TimerUI : MonoBehaviour
{
    [Header("TimerSetting")] [SerializeField]
    private bool isTutorial; 
    
    private TextMeshProUGUI timerText;
    
    private float _min;
    private float _sic;
    private float _mSic;

    private void Awake()
    {
        timerText = transform.Find("TimerInfo").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (isTutorial)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTutorial) return;
        
        _mSic += Time.deltaTime;
        if (_mSic > 1f)
        {
            _mSic = 0;
            _sic += 1f;
        }
        if (_sic > 60f)
        {
            _sic = 0;
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
