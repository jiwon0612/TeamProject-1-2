using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject _stage1;
    [SerializeField] private GameObject _stage2;
    [SerializeField] private GameObject _stage3;
    [SerializeField] private GameObject _stage4;
    [SerializeField] private GameObject _stage5;

    private void Start()
    {
        DataManager.Instance.Initialized(() => SetStage());
    }

    public void SetStage()
    {
        var data = DataManager.Instance.LoadData(true);
        
        _stage1.SetActive(data.Stage1);
        _stage2.SetActive(data.Stage2);
        _stage3.SetActive(data.Stage3);
        _stage4.SetActive(data.Stage4);
        _stage5.SetActive(data.Stage5);
    }
}
