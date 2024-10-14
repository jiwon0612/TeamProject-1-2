using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    [Header("Setting")] 
    [SerializeField] private float speed;
    
    [Header("JumpSetting")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 groundCheckerSize;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float jumpForce = 3f;
    
    public bool IsCanMove { get; set; }
    
    private Rigidbody _rigid;
    private Vector3 _playerVelocity;
    private bool _isGround;
    
    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        _rigid = GetComponent<Rigidbody>();
        IsCanMove = true;

        _player.InputCompo.OnJumpEvent += Jump;
    }

    private void OnDisable()
    {
        _player.InputCompo.OnJumpEvent -= Jump;
        
    }

    private void FixedUpdate()
    {
        _isGround = IsGroundChecker();

        if (IsCanMove)
        {
            SetMovement(_player.InputCompo.MovementDir);
            _rigid.velocity = _playerVelocity;
        }
    }

    public bool IsGroundChecker()
    {
        Collider[] colliders = Physics.OverlapBox(groundCheck.position,groundCheckerSize, groundCheck.rotation,whatIsGround);
        return colliders.Length > 0;
    }

    public void SetMovement(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        Vector3 dir = transform.TransformDirection(moveDir) * speed;
        
        _playerVelocity = new Vector3(dir.x, _rigid.velocity.y, dir.z);
    }

    public void Jump()
    {
        if (_isGround && _playerVelocity.y <= 0)
            _rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckerSize);
            Gizmos.color = Color.white;
        }
    }

#endif
}
