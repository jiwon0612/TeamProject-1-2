using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    
    [Space]
    public Player player;
    
    private void Start()
    {
        SetCurser(!isLockCursor);
        SetRope(_isCanRope);
    }

    private void SetCurser(bool setting)
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

    private void OnDrawGizmosSelected()
    {
        if (zoonPoint == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(zoonPoint.position, size);
        Gizmos.color = Color.white;
    }
}
