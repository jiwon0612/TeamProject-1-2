using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    private Image _hpBar;

    private void Awake()
    {
        _hpBar = transform.Find("CurrentHPIcon").GetComponent<Image>();
    }

    public void SetHP(float hp)
    {
        _hpBar.fillAmount = hp;
    }
}
