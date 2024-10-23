using static Player;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void StateEnter()
    {
        base.StateEnter();

        _player.GetComp<PlayerMovement>().isGround.OnValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(bool prev, bool next)
    {
        if (!next && _player.GetComp<PlayerMovement>().RigidCompo.velocity.y >= 0)
            _stateMachine.ChangeState(PlayerStateType.Idle);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        _player.GetComp<PlayerMovement>().SetMovement(_player.GetComp<InputReader>().MovementDir);
    }

    public override void StateExit()
    {
        _player.GetComp<PlayerMovement>().isGround.OnValueChanged -= HandleValueChanged;
        base.StateExit();
    }
}
