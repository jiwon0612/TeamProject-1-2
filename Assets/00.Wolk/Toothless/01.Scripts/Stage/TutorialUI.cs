using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Serializable]
    public struct TutorialTextAndTrigger
    {
        public string text;
        public TutorialTrigger trigger;
    }
    
    public List<TutorialTextAndTrigger> textAndTriggers;
    
    private TextMeshProUGUI _textMesh;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void ShowTutorial(string text)
    {
        
    }
}
