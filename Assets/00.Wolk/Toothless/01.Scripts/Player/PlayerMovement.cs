using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float speed;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpForce = 3f;
    
    private CharacterController _charController;
    private Vector3 _playerVelocity;
    private bool isGround;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGround = _charController.isGrounded;
    }

    public void SetMovement(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        
        _charController.Move(transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        
        _playerVelocity.y += gravity * Time.deltaTime;
        if (isGround && _playerVelocity.y < 0)
            _playerVelocity.y = -2f;
        _charController.Move(_playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGround)
            _playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
    }
}
