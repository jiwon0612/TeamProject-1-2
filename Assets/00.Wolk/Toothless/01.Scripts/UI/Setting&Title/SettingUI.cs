using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private InputReader playerInput;
    
    private CanvasGroup _canvasGroup;
    
    private Slider _masterSlider;
    private Slider _sfxSlider;
    private Slider _bgmSlider;

    private bool _isActive;
    
    public UnityEvent<bool> OnActiveChanged; 

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _masterSlider = transform.Find("MasterVolumeSlider").GetComponent<Slider>();
        _sfxSlider = transform.Find("EffectSoundSlider").GetComponent<Slider>();
        _bgmSlider = transform.Find("BgmSlider").GetComponent<Slider>();

        playerInput.OnSettingEvent += HandheldSettingUI;
        
        CloseSettingUI();
    }

    private void HandheldSettingUI()
    {
        if (_isActive)
        {
            CloseSettingUI();
        }
        else if (!_isActive)
        {
            OpenSettingUI();
        }
    }

    public void OpenSettingUI()
    {
        if (GameManager.Instance.player != null)
        {
            var look = GameManager.Instance.player.GetComp<PlayerLook>();
            look.IsCantLook = false;
        }
        
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _isActive = true;
        
        OnActiveChanged?.Invoke(_isActive);
    }

    public void CloseSettingUI()
    {
        if (GameManager.Instance.player != null)
        {
            var look = GameManager.Instance.player.GetComp<PlayerLook>();
            look.IsCantLook = true;
        }
        
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _isActive = false;
        
        OnActiveChanged?.Invoke(_isActive);
    }
}
