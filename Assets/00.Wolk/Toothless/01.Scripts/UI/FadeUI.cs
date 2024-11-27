using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    private RectTransform _fadeUp;
    private RectTransform _fadeDown;
    
    public UnityEvent OnFadeOut;
    public UnityEvent OnFadeIn;

    private void Awake()
    {
        _fadeUp = transform.Find("FadeUp").GetComponent<RectTransform>();
        _fadeDown = transform.Find("FadeDown").GetComponent<RectTransform>();
        
        _fadeUp.gameObject.SetActive(false);
        _fadeDown.gameObject.SetActive(false);
        FadeIn();
    }

    public void FadeOut(Action callback = null)
    {
        // Time.timeScale = 0;
        //
        // _fadeUp.gameObject.SetActive(true);
        // _fadeDown.gameObject.SetActive(true);
        //
        // _fadeUp.position = new Vector3(960,1620f,0);
        // _fadeDown.position = new Vector3(960,-540f,0);
        //
        // _fadeUp.DOLocalMoveY(540f, fadeDuration).SetEase(Ease.InOutQuad).SetUpdate(true)
        //     .OnComplete(() => OnFadeOut?.Invoke());
        // _fadeDown.DOLocalMoveY(-540f, fadeDuration).SetEase(Ease.InOutQuad).SetUpdate(true)
        //     .OnComplete(() =>
        //     {
        //         callback?.Invoke();
        //         Time.timeScale = 1;
        //         _fadeUp.gameObject.SetActive(false);
        //         _fadeDown.gameObject.SetActive(false);
        //     });
        //
        callback?.Invoke();
    }

    public void FadeIn(Action callback = null)
    {
        Time.timeScale = 0;
        
        _fadeUp.gameObject.SetActive(true);
        _fadeDown.gameObject.SetActive(true);
        
        _fadeUp.position = new Vector3(960,1080f,0);
        _fadeDown.position = new Vector3(960,0f,0);
        
        _fadeUp.DOLocalMoveY(1080f, fadeDuration).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() => OnFadeIn?.Invoke());
        _fadeDown.DOLocalMoveY(-1080f, fadeDuration).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                Time.timeScale = 1;
                callback?.Invoke();
                _fadeUp.gameObject.SetActive(false);
                _fadeDown.gameObject.SetActive(false);
            });
        
        
    }
}
