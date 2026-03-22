using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Player player;
    private PlayerControlls controlls;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float speed;
    private float verticalVelocity;

    private Vector3 movementDirection;
    private Vector2 moveInput;
    private bool isRunning;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        speed= walkSpeed;
        AssignInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
        
    }

    

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        //bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", isRunning);

    }

    private void ApplyRotation()
    {
        
            Vector3 lookingDirection = player.aim.GetMousePosition() - transform.position;
            lookingDirection.y = 0f;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;

         
    }
    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
        }
    }

    private void ApplyGravity()
    {
        if(characterController.isGrounded == false)
        {
            
            verticalVelocity -= 9.81f * Time.deltaTime;
            movementDirection.y=verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f;
        }
    }

   
    private void AssignInputEvents()
    {
        controlls = player.controlls;

        
        controlls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controlls.Character.Movement.canceled += context => moveInput = Vector2.zero;
        

        controlls.Character.Run.performed += context =>
        {
            if (movementDirection.magnitude > 0)
            {

                speed = runSpeed;
                isRunning = true;
            }

        };
        controlls.Character.Run.canceled += context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }
}
