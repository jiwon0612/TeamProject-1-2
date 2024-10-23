using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerState
{
    protected Player _player;
    protected PlayerStateMachine _stateMachine;

    protected PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }
    
    public virtual void StateEnter() {}
    
    
    public virtual void StateUpdate() {}
    
    public virtual void StateExit() {}
}
