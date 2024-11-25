using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private StageData setData;
    [SerializeField] private int number;

    private void OnCollisionEnter(Collision other)
    {
        if (((1 << other.gameObject.layer) & whatIsTarget) != 0)
        {
            if (setData.Stage1)
                DataManager.Instance.StageData.Stage1 = true;
            if (setData.Stage2)
                DataManager.Instance.StageData.Stage2 = true;
            if (setData.Stage3)
                DataManager.Instance.StageData.Stage3 = true;
            if (setData.Stage4)
                DataManager.Instance.StageData.Stage4 = true;
            if (setData.Stage5)
                DataManager.Instance.StageData.Stage5 = true;
            
            SceneManager.LoadScene(number);
        }
    }
}
