using UnityEngine;
using static Player;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        var movement = _player.GetComp<PlayerMovement>();

        Vector2 dir = _player.GetComp<InputReader>().MovementDir;

        if (dir == Vector2.zero)
        {
            _stateMachine.ChangeState(PlayerStateType.Idle);
            return;
        }
        
        movement.SetMovement(dir);
        
    }
}
