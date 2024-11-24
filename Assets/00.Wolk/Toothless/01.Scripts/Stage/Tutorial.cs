using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

public class Tutorial : MonoBehaviour
{
    [Serializable]
    struct TutorialTextAndBox
    {
        public string text;
        public TutorialTrigger trigger;
        public Action OnTrigger;
    }
    
    [SerializeField] private List<TutorialTextAndBox> tutorialTrigger;
    [SerializeField] private TextMeshProUGUI tutorialText;
    
    private Tween _textTween;
    private bool _isWrite;
    
    private void Awake()
    {
        for (int i = 0; i < tutorialTrigger.Count; i++)
        {
            
        }
    }

    public void StartTutorial(string text)
    {
        Time.timeScale = 0;
        //UI키기
        _textTween = DOTween.To(() => tutorialText.text, x => tutorialText.text = x, text, 1f).SetUpdate(true)
            .OnComplete(() => _isWrite = false);
        
    }

    public void SkipTutorial()
    {
        if (_isWrite)
        {
            _textTween.Complete();
        }

        {
            Time.timeScale = 1;
            //UI끄기
        }
    }
}
