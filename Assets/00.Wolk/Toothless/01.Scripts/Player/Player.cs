using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    private PlayerMovement _playerMoveComp;
    private PlayerLook _playerLook;

    private void Awake()
    {
        _playerMoveComp = GetComponent<PlayerMovement>();
        _playerLook = GetComponent<PlayerLook>();
        
        inputReader.OnJumpEvent += _playerMoveComp.Jump;

    }

    private void OnDisable()
    {
        inputReader.OnJumpEvent -= _playerMoveComp.Jump;
        
    }

    private void FixedUpdate()
    {
        _playerMoveComp.SetMovement(inputReader.MovementDir);

    }

    private void LateUpdate()
    {
        _playerLook.SetPlayerLook(inputReader.MouseDir);
    }
}
