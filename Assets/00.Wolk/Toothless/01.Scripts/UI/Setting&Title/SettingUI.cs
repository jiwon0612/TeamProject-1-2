using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private InputReader playerInput;
    [SerializeField] private bool isTitle;

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
        DataManager.Instance.OnComplete += SetSliderValue;
    }

    private void Start()
    {
        CloseSettingUI(false);
    }

    // private void OnApplicationQuit()
    // {
    //     SaveVolumeValue();
    // }
    
    
    private void OnDestroy()
    {
        playerInput.OnSettingEvent -= HandheldSettingUI;
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
            look.IsCantLook = true;
        }
        if(!isTitle)
            GameManager.Instance.SetCurser(true);

        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _isActive = true;

        OnActiveChanged?.Invoke(_isActive);
    }

    public void CloseSettingUI(bool isSave = true)
    {
        if (GameManager.Instance.player != null)
        {
            var look = GameManager.Instance.player.GetComp<PlayerLook>();
            look.IsCantLook = false;
        }
        if (!isTitle)
            GameManager.Instance.SetCurser(false);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _isActive = false;
        if (isSave)
            SaveVolumeValue();

        OnActiveChanged?.Invoke(_isActive);
    }

    public void SetSliderValue(StageData data)
    {
        Debug.Log(data.ToString());
        
        _masterSlider.value = data.MasterVolume;
        _sfxSlider.value = data.SFXVolume;
        _bgmSlider.value = data.BGMVolume;
    }

    public void ClickReturnBtn(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void ClickExitQuitBtn()
    {
        Debug.Log("게임나가기");
        Application.Quit();
    }

    public void SaveVolumeValue()
    {
        DataManager.Instance.StageData.MasterVolume = _masterSlider.value;
        DataManager.Instance.StageData.SFXVolume = _sfxSlider.value;
        DataManager.Instance.StageData.BGMVolume = _bgmSlider.value;
        
        DataManager.Instance.SaveData();
    }
}