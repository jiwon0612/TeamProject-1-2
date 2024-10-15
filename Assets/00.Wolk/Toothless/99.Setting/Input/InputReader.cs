using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, Console.IPlayerActions, IPlayerComponent
{
    private Console _console;
    private Player _player;

    public Vector2 MovementDir { get; private set; }
    public Vector2 MouseDir { get; private set; }
    public event Action OnJumpEvent;
    public event Action<bool> OnShootEvent;

    private void OnEnable()
    {
        if (_console == null)
        {
            _console = new Console();
            _console.Player.SetCallbacks(this);
        }

        _console.Enable();
    }
    
    public void Initialize(Player player)
    {
        _player = player;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        MovementDir = dir.normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDir = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnShootEvent?.Invoke(true);
        if (context.canceled)
            OnShootEvent?.Invoke(false);
    }

    
}