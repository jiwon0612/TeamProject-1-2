using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static bool IsDestroyed = false;

    public static T Instance
    {
        get
        {
            if (IsDestroyed)
            {
                instance = null;
            }
            if (instance == null)
            { 
                instance = GameObject.FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} 싱글톤 못찾음");
                }
                else
                {
                    IsDestroyed = false;
                }
            }
            return instance;
        }
    }

    private void OnDisable()
    {
        IsDestroyed = true;
    }
}
