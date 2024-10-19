using static Player;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    private PlayerMovement _playerMovement;
    
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        _playerMovement = _player.GetComp<PlayerMovement>();
    }

    public override void StateEnter()
    {
        base.StateEnter();
        _playerMovement.StopMove();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_player.GetComp<InputReader>().MouseDir != Vector2.zero)
        {
            _stateMachine.ChangeState(PlayerStateType.Move);
            return;
        }
    }

}
