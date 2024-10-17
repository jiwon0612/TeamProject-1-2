using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    [Header("Setting")] 
    [SerializeField] private float speed;

    [Header("JumpSetting")] [SerializeField]
    private Transform groundCheck;

    [SerializeField] private Vector3 groundCheckerSize;
    [SerializeField] private LayerMask whatIsGround;
    public float jumpForce = 3f;

    public bool IsCanMove { get; set; }
    public NotifyValue<bool> isGround;

    public Rigidbody RigidCompo { get; private set; }
    private Vector3 _playerVelocity;

    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        RigidCompo = GetComponent<Rigidbody>();
        isGround = new NotifyValue<bool>();
        IsCanMove = false;
    }

    private void FixedUpdate()
    {
        IsGroundChecker();

        if (!IsCanMove)
            RigidCompo.velocity = _playerVelocity;
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
        Vector3 dir = transform.TransformDirection(moveDir) * speed;

        _playerVelocity = new Vector3(dir.x, RigidCompo.velocity.y, dir.z);
    }

    public void StopMove() => _playerVelocity = new Vector3(0, RigidCompo.velocity.y, 0);

    public void Jump() => _player.StateMachine.ChangeState(Player.PlayerStateType.Jump);

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