using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [Header("TilteSetting")] [SerializeField]
    private SettingUI settingUI;
    [SerializeField] private Transform door;
 
    public UnityEvent OnStartEvent;

    private CanvasGroup _canvasGroup;

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
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, 2.5f)
            .OnComplete(() => OnStartEvent?.Invoke());
        door.DOLocalMoveY(10, 2);
    }

    public void ClickLoadSceneBtn(int number)
    {
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, 2.5f)
            .OnComplete(() => SceneManager.LoadScene(number));
        door.DOLocalMoveY(10, 2);
    }

    public void ClickExitBtn()
    {
        DataManager.Instance.SaveData();
        Application.Quit();
    }
}