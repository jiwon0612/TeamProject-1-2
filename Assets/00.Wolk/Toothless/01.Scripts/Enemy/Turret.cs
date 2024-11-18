using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D;
using DG.Tweening;
using UnityEngine.Events;

public class Turret : MonoBehaviour, IHitable
{
    [Header("TurretSetting")] [SerializeField]
    private float turretSearchRange;

    [SerializeField] private float turretAttackCollTime;
    [SerializeField] private float lookSpeed;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Event")] public UnityEvent OnShootEvent;

    private Transform _turretLag;
    private Transform _turretBody;
    [SerializeField] private Transform _turretPo;

    private LineRenderer _turretAim;

    private bool _isCanShoot;
    private float _lastShootTime;
    private bool _isDead;


    private void Awake()
    {
        _turretLag = transform.Find("Lag");
        _turretBody = transform.Find("Body");
        //_turretPo = transform.Find("Po");
        
        _isDead = false;
        _turretAim = _turretPo.GetComponent<LineRenderer>();
        
        Initialized();
    }

    private void Initialized()
    {
        _turretAim.positionCount = 2;
    }

    private void Update()
    {
        if (_isDead) return;
        
        _isCanShoot = _lastShootTime < Time.time;

        DrawLine();
        if (CheckToPlayerInRange())
        {
            RaycastHit point;
            if (DetectSometing(out point)) return;

            LookPlayer();
            if (Aiming())
            {
                if (_lastShootTime < Time.time)
                {
                    Shoot();
                }
            }
        }
    }

    private void LateUpdate()
    {
        _turretAim.enabled = _isCanShoot;
    }


    private bool Aiming()
    {
        RaycastHit point;
        bool isHit = Physics.Raycast(_turretPo.position, _turretPo.forward, out point, turretSearchRange, whatIsPlayer);

        //Debug.Log(point);
        //Debug.DrawRay();

        return isHit;
    }

    public void Shoot()
    {
        _lastShootTime = Time.time + turretAttackCollTime;
        // Bullet bullet = Instantiate(bulletPrefab);
        // bullet.transform.position = _turretPo.position;
        // bullet.InitAndFire(_turretPo.forward, _turretPo.rotation);
        
        Bullet bullet = PoolManager.Instance.Pop("Bullet") as Bullet;
        bullet.transform.position = _turretPo.position;
        bullet.InitAndFire(_turretPo.forward, _turretPo.rotation);

        OnShootEvent?.Invoke();
    }

    private bool DetectSometing(out RaycastHit point)
    {
        Vector3 dir = GameManager.Instance.player.transform.position - _turretPo.position;
        bool isHit = Physics.Raycast(_turretPo.position, dir.normalized, out point, turretSearchRange);

        if (!isHit)
        {
            return false;
        }
        else
        {
            if (((1 << point.collider.gameObject.layer) & whatIsPlayer) != 0)
                return false;
            else
                return true;
        }
    }

    private void LookPlayer()
    {
        Vector3 point = GameManager.Instance.player.transform.position + new Vector3(0, -0.7f, 0);
        Vector3 dir = (point - transform.position).normalized;

        Quaternion lookDirQuaternion = Quaternion.LookRotation(dir);
        Vector3 lookDir = lookDirQuaternion.eulerAngles;

        // float x = Mathf.Lerp(_turretBody.localPosition.y, lookDir.y, lookSpeed * Time.deltaTime);
        // float y = Mathf.Lerp(_turretPo.localPosition.x, lookDir.x, lookSpeed * Time.deltaTime);

        // _turretBody.localEulerAngles = new Vector3(0,y,0);
        // _turretPo.localEulerAngles = new Vector3(x,0,0);

        _turretBody.DOLocalRotate(new Vector3(0, lookDir.y, 0), lookSpeed).SetEase(Ease.Linear);
        _turretPo.DOLocalRotate(new Vector3(lookDir.x, 0, 0), lookSpeed).SetEase(Ease.Linear);
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

        RaycastHit point;
        bool hit = Physics.Raycast(_turretPo.position, _turretPo.forward, out point, turretSearchRange);
        if (hit)
        {
            _turretAim.SetPosition(1, point.point);
        }
        else
        {
            Vector3 dir = _turretPo.position + _turretPo.forward * turretSearchRange;
            _turretAim.SetPosition(1, dir);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_turretPo.position, turretSearchRange);
        Gizmos.color = Color.white;
    }

    public void Hit()
    {
        this.enabled = false;
        _turretBody.parent = null;
        _turretLag.parent = null;
        _turretPo.parent = null;
    }
}