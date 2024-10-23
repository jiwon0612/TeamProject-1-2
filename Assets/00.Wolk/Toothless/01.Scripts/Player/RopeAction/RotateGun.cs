using System;
using UnityEngine;

public class RotateGun : MonoBehaviour, IPlayerComponent
{
    private RopeAction _ropeCompo;
    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;

        _ropeCompo = _player.GetComp<RopeAction>();
    }

    private void Update()
    {
        if (!_ropeCompo.IsGrappling())
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            return;
        }
        
        transform.LookAt(_ropeCompo.GetGrapplePoint());
    }
}
