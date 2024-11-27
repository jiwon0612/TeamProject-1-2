using UnityEngine;
using System;
using System.IO;
using Exception = System.Exception;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] private string fileName = "SaveData";
    private string _path;
    
    public Action<StageData> OnComplete;
    
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

    public void Initialized()
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
            Debug.Log(jsonString);
            
            FileStream fs = File.Open(_path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(jsonString);
            sw.Close();
            Debug.Log("Create data file");
        }
        OnComplete?.Invoke(StageData);
        OnComplete = null;
    }
    
    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(StageData);
            Debug.Log(json);
            File.WriteAllText(_path, string.Empty);
            FileStream fs = File.Open(_path, FileMode.Open);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(json);
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
            string jsonData = sr.ReadToEnd();
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

[Serializable]
public class StageData
{
    public bool Stage1 = true;
    public bool Stage2 = false;
    public bool Stage3 = false;
    public bool Stage4 = false;
    public bool Stage5 = false;
    
    public float MasterVolume = 0.5f;
    public float SFXVolume = 0.5f;
    public float BGMVolume = 0.5f;

    public float DPIValue = 30;
}