using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class RopeAction : MonoBehaviour, IPlayerComponent
{
    private LineRenderer _line;
    private Vector3 _grapplePoint;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float _maxDistance;
    private Transform _cam;

    private SpringJoint _joint;
    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        _line = GetComponent<LineRenderer>();
        _cam = Camera.main.transform;

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
            _grapplePoint = hit.point;
            _joint = _player.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _grapplePoint;

            float distance = Vector3.Distance(_player.transform.position, _grapplePoint);

            _joint.maxDistance = distance * 0.8f;
            _joint.minDistance = distance * 0.25f;

            _joint.spring = 4.5f;
            _joint.damper = 7f;
            _joint.massScale = 4.5f;

            _line.positionCount = 2;
        }
    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    private void DrawGrapple()
    {
        if (!_joint) return;

        _line.SetPosition(0, gunTip.position);
        _line.SetPosition(1, _grapplePoint);
    }

    private void StopGrapple()
    {
        _line.positionCount = 0;
        Destroy(_joint);
    }

    public bool IsGrappling()
    {
        return _joint != null;
    }

    public Vector3 GetGrapplePoint() => _grapplePoint;
}