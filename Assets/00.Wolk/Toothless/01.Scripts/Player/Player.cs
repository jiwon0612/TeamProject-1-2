    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public enum PlayerStateType
    {
        Idle,
        Move,
        Jump,
        Fall
    }
    
    [field : SerializeField] public InputReader InputCompo { get; private set; }

    public Dictionary<Type, IPlayerComponent> _components;
    
    // public PlayerStateMachine StateMachine { get; private set; }
    //
    // public string CurrentState;

    private void Awake()
    {
        _components = new Dictionary<Type, IPlayerComponent>();
        GetComponentsInChildren<IPlayerComponent>().ToList().ForEach(x => _components.Add(x.GetType(), x));
        _components.Add(InputCompo.GetType(), InputCompo);
        _components.Values.ToList().ForEach(x => x.Initialize(this));
        
        // StateMachine = new PlayerStateMachine();
        //
        // foreach (PlayerStateType item in Enum.GetValues(typeof(PlayerStateType)))
        // {
        //     string enumName = item.ToString();
        //     Type t = Type.GetType($"Player{enumName}State");
        //     if (t != null)
        //     {
        //         var state = Activator.CreateInstance(t, this , StateMachine) as PlayerState;
        //         StateMachine.AddState(state, item);
        //     }
        //     else
        //         Debug.LogWarning($"없어 Player{enumName}State");
        // }
        //
        // StateMachine.Initialize(PlayerStateType.Idle);

        GetComp<InputReader>().OnJumpEvent += GetComp<PlayerMovement>().Jump;
        GetComp<InputReader>().OnDeshEvent += GetComp<PlayerMovement>().Dash;
    }

    private void OnDisable()
    {
        GetComp<InputReader>().OnJumpEvent -= GetComp<PlayerMovement>().Jump;
        GetComp<InputReader>().OnDeshEvent -= GetComp<PlayerMovement>().Dash;
    }

    public T GetComp<T>() where T : class
    {
        Type type = typeof(T);
        if (_components.TryGetValue(type, out IPlayerComponent compo))
        {
            return compo as T;
        }
        return default;
    }

    private void Update()
    {
        // CurrentState = StateMachine.CurrentState.ToString();
        // StateMachine.CurrentState.StateUpdate();
        
        GetComp<PlayerMovement>().SetMovement(GetComp<InputReader>().MovementDir);
    }
}