using UnityEngine;
using static Player;

public class PlayerJumpState : PlayerAirState
{
    private PlayerMovement _movement;
    
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void StateEnter()
    {
        base.StateEnter();
        _movement = _player.GetComp<PlayerMovement>();
        _movement.StopMove();
        _movement.RigidCompo.AddForce(Vector3.up * _movement.jumpForce, ForceMode.Impulse);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (_movement.RigidCompo.velocity.y < 0)
        {
            _stateMachine.ChangeState(PlayerStateType.Fall);
            return;
        }
    }
}
