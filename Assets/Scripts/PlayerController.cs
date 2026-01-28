using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput inputActions;

    private CharacterController controller;
    private Animator anim;
    [SerializeField] private Transform cameraPos;

    [Header("*********Movement*********")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 movement;
    [SerializeField] private float rotationSpeed;
    [Header("Jump")]
    [SerializeField] private float JumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = .5f;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    [SerializeField]private float verticalVelocity;

    private void Awake()
    {
        inputActions = new PlayerInput();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // assign input actions here

        inputActions.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveDirection = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookDirection = ctx.ReadValue<Vector2>();

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
        
        var lookRotation = Quaternion.Euler(0, cameraPos.eulerAngles.y, 0); 
        var moveInputDirection = lookRotation * new Vector3(moveDirection.x, 0, moveDirection.y);
        movement = moveInputDirection * Time.deltaTime * moveSpeed;

        if(movement.magnitude > 0)
        {
            var t = rotationSpeed * Time.deltaTime;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            Quaternion nextRotation = Quaternion.Lerp(currentRotation, targetRotation, t);
            transform.rotation = nextRotation;
        }

        if(controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        movement.y = verticalVelocity * Time.deltaTime;

        if(movement.magnitude > 0)
        {
            controller.Move(movement);
            
        }

        if(moveInputDirection.magnitude > 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
        
        // anim.SetFloat("Speed", movement.normalized.magnitude);
    }

    

    
}
