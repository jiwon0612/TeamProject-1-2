using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Serializable]
    public struct TutorialTextAndTrigger
    {
        public string text;
        public TutorialTrigger trigger;
        public float duration;
    }

    public List<TutorialTextAndTrigger> textAndTriggers;
    
    [SerializeField] private LayerMask whatIsTarget;
    
    public Tween TutorialTween { get; set; }

    private TextMeshProUGUI _textMesh;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();

        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        for (int i = 0; i < textAndTriggers.Count; i++)
        {
            Debug.Log(i);
            textAndTriggers[i].trigger.Initialized(this,textAndTriggers[i].text, textAndTriggers[i].duration,whatIsTarget);
        }
    }

    public void ShowTutorial(string text, float duration, TutorialTrigger trigger)
    {
        if (TutorialTween.IsActive()) return;
        
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        GameManager.Instance.SetCurser(true);
        
        Time.timeScale = 0;
        
        TutorialTween = DOTween.To(() => _textMesh.text, x => _textMesh.text = x, text, duration)
            .OnComplete(() => trigger.IsComplete = true).SetEase(Ease.Linear);
    }

    public void ClickPanel()
    {
        if (TutorialTween.IsActive())
        {
            TutorialTween.Complete();
        }
        else if (!TutorialTween.IsActive())
        {
            TutorialTween.Complete();
            _textMesh.text = "";
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            Time.timeScale = 1;
        }
    }
}