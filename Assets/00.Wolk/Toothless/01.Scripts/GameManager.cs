using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("GameSetting")] 
    [SerializeField] private bool _isCanRope;
    
    [Space]
    public Player player;
    
    private void Start()
    {
        SetCurser(false);
        SetRope(_isCanRope);
    }

    private void SetCurser(bool setting)
    {
        Cursor.visible = setting;
        Cursor.lockState = setting == true ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SetRope(bool setting)
    {
        player.GetComp<RopeAction>().IsCanShoot = setting;
    }
    
}
