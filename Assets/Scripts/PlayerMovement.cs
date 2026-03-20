using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControlls controlls;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    public Vector3 movementDirection;
    private float verticalVelocity;

    [Header("Aim info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;

    

    private Vector2 moveInput;
    private Vector2 aimInput;


    private void Awake()
    {
        controlls = new PlayerControlls();
        controlls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controlls.Character.Movement.canceled += context => moveInput = Vector2.zero;
        controlls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controlls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ApplyMovement();
        AimTowardsMouse();
        AnimatorControllers();
        
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);  

    }

    private void AimTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lookingDirection = hitInfo.point - transform.position;
            lookingDirection.y = 0f;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;

            aim.position=new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
    }
    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * walkSpeed);
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

    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();    
    }
}
