using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Turret : MonoBehaviour
{
    [Header("TurretSetting")] 
    [SerializeField] private float turretSearchRange;
    [SerializeField] private float turretAttackCollTime;
    [SerializeField] private float lookSpeed;
    
    
    private Transform _turretLag;
    private Transform _turretBody;
    [SerializeField] private Transform _turretPo;

    private LineRenderer _turretAim;

    private void Awake()
    {
        _turretLag = transform.Find("Lag");
        _turretBody = transform.Find("Body");
        //_turretPo = transform.Find("Po");

        _turretAim = _turretPo.GetComponent<LineRenderer>();
        
        Initialized();
    }

    private void Initialized()
    {
        _turretAim.positionCount = 2;
    }
    
    private void Update()
    {
        DrawLine();

        if (CheckToPlayerInRange())
        {
            LookPlayer();
        }
    }

    private void LookPlayer()
    {
        Vector3 point = GameManager.Instance.player.transform.position + new Vector3(0, -1f, 0);
        Vector3 dir = (point - transform.position).normalized;
        
        Quaternion lookDirQuaternion = Quaternion.LookRotation(dir);
        Vector3 lookDir = lookDirQuaternion.eulerAngles;

        // float x = Mathf.Lerp(_turretBody.localPosition.y, lookDir.y, lookSpeed * Time.deltaTime);
        // float y = Mathf.Lerp(_turretPo.localPosition.x, lookDir.x, lookSpeed * Time.deltaTime);

        _turretBody.DOLocalRotate(new Vector3(0, lookDir.y, 0), lookSpeed).SetEase(Ease.Linear);
        _turretPo.DOLocalRotate(new Vector3(lookDir.x, 0, 0), lookSpeed).SetEase(Ease.Linear);
        // _turretBody.localEulerAngles = new Vector3(0,y,0);
        // _turretPo.localEulerAngles = new Vector3(x,0,0);
    }

    private bool CheckToPlayerInRange()
    {
        float distance = Vector3.Distance(_turretPo.position, 
            GameManager.Instance.player.transform.position);
        
        return distance <= turretSearchRange;
    }

    private void DrawLine()
    {
        _turretAim.SetPosition(0, _turretAim.transform.position);
        Vector3 dir = _turretPo.position + _turretPo.forward * turretSearchRange;
        _turretAim.SetPosition(1, dir);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_turretPo.position, turretSearchRange);
        Gizmos.color = Color.white;
    }
}
