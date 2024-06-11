using System;
using UnityEditor.PackageManager;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;

    private CharacterController characterController;
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    [Header("Movement Info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    private float speed;
    private Vector3 movementDirection;
    private float verticalVelocity;
    private bool isRunning;

    public Vector2 moveInput { get; private set; }


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        speed = walkSpeed;
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

        animator.SetFloat("xVelocity", xVelocity, 0.1f,Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("IsRunning", playRunAnimation);
    }



    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.GetPlayerAim().GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();
        
        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);

      
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
        if (!characterController.isGrounded)
        {
            verticalVelocity -= 9.81f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -0.5f;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;
        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Character.Run.performed += context =>
        {
                speed = runSpeed;
                isRunning = true;
        };
        controls.Character.Run.canceled += context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }
}
