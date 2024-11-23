using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Exception = System.Exception;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] private string fileName = "SaveData";
    private string _path;
    
    public StageData StageData { get; set; }
    
    private void Awake()
    {
        _path = Application.persistentDataPath + $"/{fileName}.txt";
        Debug.Log(_path);
    }
    
    private void Start()
    {
        Initialized();
    }
    
    public void Initialized(Action OnComplete = null)
    {
        FileInfo fi = new FileInfo(_path);
        if (fi.Exists)
        {
            LoadData(true);
            Debug.Log("Load Data");
        }
        else
        {
            StageData = new StageData();
            string jsonString = JsonUtility.ToJson(StageData);
            
            FileStream fs = File.Open(_path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(jsonString);
            sw.Close();
            Debug.Log("Create data file");
        }
        OnComplete?.Invoke();
    }
    
    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(StageData);
            
            FileStream fs = File.Open(_path, FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
    public StageData LoadData(bool isUpdate  = false)
    {
        try
        {
            FileStream fs = File.Open(_path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string jsonData = sr.ReadLine();
            sr.Close();
            
            StageData data = JsonUtility.FromJson<StageData>(jsonData);
            if (isUpdate) StageData = data;
            return data;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }
}

public class StageData
{
    public bool Stage1 = true;
    public bool Stage2 = false;
    public bool Stage3 = false;
    public bool Stage4 = false;
    public bool Stage5 = false;
    
    public float MasterVolume = 1;
    public float SFXVolume = 1;
    public float BGMVolume = 1;
}