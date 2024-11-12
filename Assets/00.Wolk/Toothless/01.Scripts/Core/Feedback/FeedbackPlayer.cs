using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FeedbackPlayer : MonoBehaviour
{
    private List<Feedback> _feedbackToPlayer;

    private void Awake()
    {
        _feedbackToPlayer = GetComponents<Feedback>().ToList();
    }

    public void PlayFeedback()
    {
        FinishFeedback();
        _feedbackToPlayer.ForEach(f => f.PlayFeedback());
    }
    
    private void FinishFeedback() => _feedbackToPlayer.ForEach(f => f.StopFeedback());
}
