using static Player;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void StateEnter()
    {
        base.StateEnter();
        _player.GetComp<PlayerMovement>().isGround.OnValueChanged += HandleValueChanged;
        _player.GetComp<InputReader>().OnJumpEvent += _player.GetComp<PlayerMovement>().Jump;
    }

    private void HandleValueChanged(bool prev, bool next)
    {
        if (next == false)
            _stateMachine.ChangeState(PlayerStateType.Fall);
    }

    public override void StateExit()
    {
        _player.GetComp<PlayerMovement>().isGround.OnValueChanged -= HandleValueChanged;
        _player.GetComp<InputReader>().OnJumpEvent -= _player.GetComp<PlayerMovement>().Jump;
        base.StateExit();

    }
}
