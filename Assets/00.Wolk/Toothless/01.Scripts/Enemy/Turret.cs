using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class Turret : MonoBehaviour, IHitable
{
    public struct Parts
    {
        public Transform parts;
        public Rigidbody rigidbody;
        public MeshCollider meshCollider;
    }
    
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

    private List<Parts> _partsList;

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
        
        _partsList = new List<Parts>();
        _partsList.Add(new Parts() { parts = _turretLag, rigidbody = _turretLag.GetComponent<Rigidbody>(), meshCollider = _turretLag.GetComponent<MeshCollider>() });
        _partsList.Add(new Parts(){parts = _turretBody, rigidbody = _turretBody.GetComponent<Rigidbody>(), meshCollider = _turretBody.GetComponent<MeshCollider>() });
        _partsList.Add(new Parts() {parts = _turretPo, rigidbody = _turretPo.GetComponent<Rigidbody>(), meshCollider = _turretPo.GetComponent<MeshCollider>() });
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
        Vector3 dir = GameManager.Instance.player.transform.position - _turretPo.position ;
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
        Vector3 point = GameManager.Instance.player.transform.position + new Vector3(0, -1f, 0);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_turretPo.position, turretSearchRange);
        Gizmos.color = Color.white;
    }

    public Transform[] Hit()
    {
        this.enabled = false;
        _turretAim.positionCount = 0;
        Transform[] parts = new Transform[_partsList.Count];
        for (int i = 0; i < _partsList.Count; i++)
        {
            _partsList[i].parts.parent = null;
            _partsList[i].rigidbody.useGravity = true;
            _partsList[i].rigidbody.isKinematic = false;
            _partsList[i].meshCollider.enabled = true;
            parts[i] = _partsList[i].parts;
        }
        
        Destroy(gameObject);
        return parts;
    }
}