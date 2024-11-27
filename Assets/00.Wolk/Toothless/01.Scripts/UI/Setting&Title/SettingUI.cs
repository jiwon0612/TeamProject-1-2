using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    private TMP_InputField _dpiInput;

    private bool _isActive;
    private float _beforeDPI = 30;

    public UnityEvent<bool> OnActiveChanged;
    
    public UnityEvent OnDPIChanged;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _masterSlider = transform.Find("MasterVolumeSlider").GetComponent<Slider>();
        _sfxSlider = transform.Find("EffectSoundSlider").GetComponent<Slider>();
        _bgmSlider = transform.Find("BgmSlider").GetComponent<Slider>();
        _dpiInput = transform.Find("DPIInput").GetComponent<TMP_InputField>();

        playerInput.OnSettingEvent += HandheldSettingUI;
        DataManager.Instance.OnComplete += SetSliderValue;
        DataManager.Instance.OnComplete += LoadDPIValue;
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
        {
            SaveVolumeValue();
            SaveDPIValue();
        }

        OnActiveChanged?.Invoke(_isActive);
    }

    public void SetSliderValue(StageData data)
    {
        _masterSlider.value = data.MasterVolume;
        _sfxSlider.value = data.SFXVolume;
        _bgmSlider.value = data.BGMVolume;
        
        if (AudioManager.Instance == null) return;
        
        AudioManager.Instance.SetMasterVolume(data.MasterVolume);
        AudioManager.Instance.SetSFXVolume(data.SFXVolume);
        AudioManager.Instance.SetBGMVolume(data.BGMVolume);
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

    public void GetDPIValue(string dpi)
    {
        if (!string.IsNullOrEmpty(dpi))
        {
            _beforeDPI = float.Parse(dpi);
            SaveDPIValue();
            OnDPIChanged?.Invoke();
        }
        else
        {
            _dpiInput.text = _beforeDPI.ToString();
        }
        
    }

    public void SaveDPIValue()
    {
        DataManager.Instance.StageData.DPIValue = _beforeDPI;
        
        DataManager.Instance.SaveData();
    }

    private void LoadDPIValue(StageData data)
    {
        _beforeDPI = DataManager.Instance.LoadData().DPIValue;
        _dpiInput.text = _beforeDPI.ToString();
        OnDPIChanged?.Invoke();
    }
}