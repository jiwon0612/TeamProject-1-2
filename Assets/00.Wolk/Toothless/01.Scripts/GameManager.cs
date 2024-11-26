using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("GameSetting")] 
    [SerializeField] private bool _isCanRope;
    [SerializeField] private bool isLockCursor;

    [Header("StageSetting")] 
    [SerializeField] private Vector3 size;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private Transform zoonPoint;
    [SerializeField] private Transform point;

    public UnityEvent OnGameBGM;
    
    [Space]
    public Player player;
    
    private void Start()
    {
        OnGameBGM?.Invoke();
        SetCurser(!isLockCursor);
        SetRope(_isCanRope);
    }

    public void SetCurser(bool setting)
    {
        Cursor.visible = setting;
        Cursor.lockState = setting == true ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SetRope(bool setting)
    {
        if (player == null) return;
        
        player.GetComp<RopeAction>().IsCanShoot = setting;
    }

    private void Update()
    { 
        Collider[] col = Physics.OverlapBox(zoonPoint.position, size, Quaternion.identity, whatIsTarget);

        if (col.Length > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnDrawGizmos()
    {
        if (zoonPoint == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(zoonPoint.position, size);
        Gizmos.color = Color.white;
    }
}
