using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;


public class RopeAction : MonoBehaviour, IPlayerComponent
{
    [Header("SwhingSetting")] 
    [SerializeField] private float horizontalForce;
    [SerializeField] private float forwardForce;
    [SerializeField] private float extendCableSpeed;

    private LineRenderer _line;
    private Vector3 _grapplePoint;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float _maxDistance;
    private Transform _cam;

    public SpringJoint Joint { get; private set; }
    private Player _player;
    private Rigidbody _rigid;

    public bool IsSwhinging { get; private set; }

    public void Initialize(Player player)
    {
        _player = player;
        _line = GetComponent<LineRenderer>();
        _cam = Camera.main.transform;
        _rigid = _player.GetComp<PlayerMovement>().RigidCompo;

        _player.InputCompo.OnShootEvent += HandleShootEvent;
        
    }

    private void HandleShootEvent(bool isShooting)
    {
        if (isShooting)
            StartGrapple();
        else
            StopGrapple();
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.position, _cam.forward, out hit, _maxDistance, whatIsWall))
        {
            IsSwhinging = true;
            _grapplePoint = hit.point;
            Joint = _player.AddComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
            Joint.connectedAnchor = _grapplePoint;

            float distance = Vector3.Distance(_player.transform.position, _grapplePoint);

            Joint.maxDistance = distance * 0.8f;
            Joint.minDistance = distance * 0.25f;

            Joint.spring = 4.5f;
            Joint.damper = 7f;
            Joint.massScale = 4.5f;

            _line.positionCount = 2;
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
        if (input.y < 0)
        {
            float extendedDistance = Vector3.Distance(transform.position, GetGrapplePoint()) + extendCableSpeed;
            
            Joint.maxDistance = extendedDistance * 1.2f;
            Joint.minDistance = extendedDistance * 0.85f;
        }

        if (Keyboard.current.spaceKey.isPressed)
        {
            Debug.Log("엄제유");
            Vector3 directionPoint = GetGrapplePoint() - transform.position;
            _rigid.AddForce(directionPoint.normalized * extendCableSpeed);
            
            float distanceFromPoint = Vector3.Distance(transform.position,GetGrapplePoint());
            
            Joint.maxDistance = distanceFromPoint * 0.75f;
            Joint.minDistance = distanceFromPoint * 0.25f;
        }
    }
}