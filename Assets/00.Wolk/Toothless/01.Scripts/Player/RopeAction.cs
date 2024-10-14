using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RopeAction : MonoBehaviour,IPlayerComponent
{
    private LineRenderer _line;
    private Vector3 _grapplePoint;
    [SerializeField] private LayerMask whatIsWall;
    
    private Player _player;
    
    public void Initialize(Player player)
    {
        _player = player;
        _line = GetComponent<LineRenderer>();
        
        _line.enabled = false;
    }
}
