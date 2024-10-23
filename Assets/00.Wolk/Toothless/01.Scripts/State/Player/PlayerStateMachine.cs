using System.Collections.Generic;
using static Player;

public class PlayerStateMachine
{
    private PlayerState _currentState;
    public PlayerState CurrentState { get; private set; }

    private Dictionary<PlayerStateType, PlayerState> _playerStateDictionary = new ();

    public void Initialize(PlayerStateType startType)
    {
        CurrentState = _playerStateDictionary[startType];
        CurrentState.StateEnter();
    }

    public void AddState(PlayerState state, PlayerStateType type) => _playerStateDictionary.Add(type, state);

    public void ChangeState(PlayerStateType newState)
    {
        CurrentState.StateExit();
        CurrentState = _playerStateDictionary[newState];
        CurrentState.StateEnter();
    }
}