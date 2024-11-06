using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    [Header("Setting")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float swhingSpeed;
    [SerializeField] private float airSpeed;
    
    private float _speed;

    [Header("JumpSetting")] 
    [SerializeField] private Transform groundCheck;

    [SerializeField] private Vector3 groundCheckerSize;
    [SerializeField] private LayerMask whatIsGround;
    public float jumpForce = 3f;

    public bool IsCanMove { get; set; }
    public NotifyValue<bool> isGround;

    public Rigidbody RigidCompo { get; private set; }
    private Vector3 _playerVelocity;

    private Player _player;
    private RopeAction _rope;

    public void Initialize(Player player)
    {
        _player = player;
        RigidCompo = GetComponent<Rigidbody>();
        isGround = new NotifyValue<bool>();
        IsCanMove = false;
        _rope = player.GetComp<RopeAction>();
        _speed = moveSpeed;
    }

    private void FixedUpdate()
    {
        IsGroundChecker();

        CheckIsSwhing();
        
        SetMovement(_player.GetComp<InputReader>().MovementDir);

        if (!IsCanMove && !_rope.IsSwhinging && isGround.Value)
            RigidCompo.velocity = _playerVelocity;
        
        // if (!isGround.Value)
        //     SetInAirMovement(_player.GetComp<InputReader>().MovementDir);
    }

    private void CheckIsSwhing()
    {
        if (_rope.IsSwhinging)
            _speed = swhingSpeed;
        else
            _speed = moveSpeed;
    }

    public void IsGroundChecker()
    {
        Collider[] colliders =
            Physics.OverlapBox(groundCheck.position, groundCheckerSize, groundCheck.rotation, whatIsGround);
        isGround.Value = colliders.Length > 0;
    }

    public void SetMovement(Vector2 input)
    {
        if (IsCanMove) return;

        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        Vector3 dir = transform.TransformDirection(moveDir) * _speed;

        _playerVelocity = new Vector3(dir.x, RigidCompo.velocity.y, dir.z);
    }
    
    // public void SetInAirMovement(Vector2 input)
    // {
    //     if (input.x > 0) RigidCompo.AddForce(new Vector3(0, airSpeed, 0) * Time.deltaTime);
    //     if (input.x < 0) RigidCompo.AddForce(new Vector3(0, -airSpeed, 0) * Time.deltaTime);
    // }

    public void StopMove() => _playerVelocity = new Vector3(0, RigidCompo.velocity.y, 0);

    public void Jump()
    {
        if (isGround.Value == false) return;
        
        RigidCompo.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  
    }

    public void Dash()
    {
        if (!isGround.Value && _rope.IsSwhinging)
        {
            _rope.RopeDash(transform.forward);
            Debug.Log(10);
        }
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