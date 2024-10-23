using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        SetCurser(false);
    }

    private void SetCurser(bool setting)
    {
        Cursor.visible = setting;
        Cursor.lockState = setting == true ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
}
