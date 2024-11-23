using System;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class RopeAction : MonoBehaviour, IPlayerComponent
{
    [Header("SwhingSetting")] [SerializeField]
    private float horizontalForce;

    [SerializeField] private float forwardForce;
    [SerializeField] private float extendCableSpeed;

    private LineRenderer _line;
    private Vector3 _grapplePoint;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float _maxDistance;
    private Transform _cam;

    [Header("GrappleingSetting")] [SerializeField]
    private float spring;

    [SerializeField] private float damper;
    [SerializeField] private float massScale;
    [SerializeField] private float animationTime;

    [Header("DashSetting")] [SerializeField]
    private float dashPower;
    [SerializeField] private float dashCoolTime;

    public UnityEvent OnDeshEvent;
    public UnityEvent<float> OnDashCollTimeEvent;

    public SpringJoint Joint { get; private set; }
    private Player _player;
    private Rigidbody _rigid;
    private Coroutine _animationCoroutine;
    private float _dashTimer;
    private bool _isDashCoolTime;

    public bool IsSwhinging { get; private set; }
    public bool IsTryGrapple { get; private set; }
    public bool IsCanShoot { get; set; }

    public void Initialize(Player player)
    {
        _player = player;
        _line = GetComponent<LineRenderer>();
        _cam = Camera.main.transform;
        _rigid = _player.GetComp<PlayerMovement>().RigidCompo;
        IsCanShoot = true;
        _isDashCoolTime = false;

        _player.InputCompo.OnShootEvent += HandleShootEvent;
    }

    private void HandleShootEvent(bool isShooting)
    {
        if (!IsCanShoot) return;
        
        if (isShooting)
            TryToShootGrapple();
        else
            StopGrapple();
    }

    private void TryToShootGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.position, _cam.forward, out hit, _maxDistance, whatIsWall))
        {
            _animationCoroutine = StartCoroutine(RopeAnimation(hit.point, () => StartGrapple(hit)));
            
        }
        else
        {
            Vector3 dir = _cam.transform.position + _cam.forward * 15;
            _animationCoroutine = StartCoroutine(RopeAnimation(dir, () => StopGrapple()));
        }
    }

    private IEnumerator RopeAnimation(Vector3 point, Action callback = null)
    {
        _line.positionCount = 2;
        float percent = 0f;
        Vector3 startPoint = gunTip.position;
        Vector3 curretnPoint = Vector3.zero;
        IsTryGrapple = true;
        
        while (true)
        {
            if (percent >= 1f)
            {
                _player.GetComp<RotateGun>().transform.rotation = Quaternion.Euler(Vector3.zero);
                _line.positionCount = 2;
                callback?.Invoke();
                IsTryGrapple = false;
                yield break;
            }
            
            _player.GetComp<RotateGun>().transform.LookAt(point);
            percent += Time.deltaTime * animationTime;
            curretnPoint = Vector3.Lerp(startPoint, point, percent);
            _line.SetPosition(1, curretnPoint);
            _line.SetPosition(0, gunTip.position);
            yield return null;
        }
    }
    
    private void StartGrapple(RaycastHit hit)
    {
        IsSwhinging = true;
        _grapplePoint = hit.point;
        Joint = _player.AddComponent<SpringJoint>();
        Joint.autoConfigureConnectedAnchor = false;
        Joint.connectedAnchor = _grapplePoint;

        float distance = Vector3.Distance(_player.transform.position, _grapplePoint);

        Joint.maxDistance = distance * 0.8f;
        Joint.minDistance = distance * 0.25f;

        Joint.spring = spring;
        Joint.damper = damper;
        Joint.massScale = massScale;

    }

    private void Update()
    {
        if (_isDashCoolTime)
        {
            _dashTimer += Time.deltaTime;
            OnDashCollTimeEvent?.Invoke(_dashTimer / dashCoolTime);
            if (_dashTimer >= dashCoolTime)
            {
                Debug.Log("초기화");
                _isDashCoolTime = false;
                _dashTimer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsSwhinging)
            SwhingMovement(_player.GetComp<InputReader>().MovementDir);
    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    private void DrawGrapple()
    {
        if (!Joint) return;

        _line.SetPosition(0, gunTip.position);
        _line.SetPosition(1, _grapplePoint);
    }

    private void StopGrapple()
    {
        StopCoroutine(_animationCoroutine);
        IsSwhinging = false;
        _line.positionCount = 0;
        Destroy(Joint);
    }

    public bool IsGrappling()
    {
        return Joint != null;
    }

    public Vector3 GetGrapplePoint() => _grapplePoint;

    public void SwhingMovement(Vector2 input)
    {
        if (input.x > 0) _rigid.AddForce(_player.transform.right * horizontalForce * Time.deltaTime);
        if (input.x < 0) _rigid.AddForce(-_player.transform.right * horizontalForce * Time.deltaTime);

        if (input.y > 0) _rigid.AddForce(_player.transform.forward * forwardForce * Time.deltaTime);
        if (input.y < 0) _rigid.AddForce(-_player.transform.forward * forwardForce * Time.deltaTime);

        if (Keyboard.current.spaceKey.isPressed)
        {
            Vector3 directionPoint = GetGrapplePoint() - transform.position;
            _rigid.AddForce(directionPoint.normalized * extendCableSpeed);

            float distanceFromPoint = Vector3.Distance(transform.position, GetGrapplePoint());

            Joint.maxDistance = distanceFromPoint * 0.75f;
            Joint.minDistance = distanceFromPoint * 0.25f;
        }
    }

    public void RopeDash(Vector3 dir)
    {
        if (_isDashCoolTime) return;
        
        _isDashCoolTime = true;
        _rigid.AddForce(new Vector3(dir.x, 0, dir.z) * dashPower, ForceMode.Impulse);
        OnDeshEvent?.Invoke();
    }
}