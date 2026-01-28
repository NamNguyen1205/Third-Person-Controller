using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput inputActions;

    private CharacterController controller;

    [Header("*********Movement*********")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movement;
    [Header("Jump")]
    [SerializeField] private float JumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = .5f;
    private Vector2 moveDirection;
    [SerializeField]private float verticalVelocity;

    private void Awake()
    {
        inputActions = new PlayerInput();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // assign input actions here

        inputActions.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveDirection = Vector2.zero;

        //jump
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(controller.isGrounded)
        {
            verticalVelocity = JumpForce;
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        movement = new Vector3(moveDirection.x, 0, moveDirection.y) * Time.deltaTime * moveSpeed;

        if(controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        movement.y = verticalVelocity * Time.deltaTime;

        // Debug.Log("Move Direction: " + moveDirection);
        if(movement.magnitude > 0.01f)
        {
            controller.Move(movement);
        }
    }

    
}
