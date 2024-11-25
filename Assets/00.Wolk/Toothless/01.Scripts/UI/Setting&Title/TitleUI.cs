using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private SettingUI settingUI;
    
    private CanvasGroup _canvasGroup;

    public UnityEvent OnStartEvent;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetTitle(bool value)
    {
        _canvasGroup.interactable = !value;
        _canvasGroup.blocksRaycasts = !value;
    }

    public void ClickStartBtn()
    {
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, 1.5f)
            .OnComplete(() => OnStartEvent?.Invoke());
    }

    public void ClickLoadSceneBtn(int number)
    {
        Debug.Log(number);
        SceneManager.LoadScene(number);
    }

    public void ClickExitBtn()
    {
        DataManager.Instance.SaveData();
        Application.Quit();
    }
}
