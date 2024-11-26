using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    private Image _dashCoolTimeIcon;

    private void Awake()
    {
        _dashCoolTimeIcon = transform.Find("DashIconCollTime").GetComponent<Image>();
    }

    public void SetCoolTimeIcon(float coolTime)
    {
        _dashCoolTimeIcon.fillAmount = coolTime;
    }
}
