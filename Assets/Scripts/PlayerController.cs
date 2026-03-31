using NUnit.Framework.Constraints;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputs;
    private CharacterController controller;

    [SerializeField]private Vector2 moveInput;

    public float moveSpeed = 200f;
    public float rotationSpeed = 200f;
    public float gravity = -9.81f;
    public float verticalVelocity = 0f;
    public float jumpForce = 10f;

    public float pushForce = 4f;

    private bool IsDashing;
    public float dashTimer = 0.2f;
    public float dashDuration = 0.2f;
    public float dashForce = 10;

    private bool isRunning;

    private void Awake()
    {
        inputs = new();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputs.Player.Jump.performed += OnJump;
        inputs.Player.Sprint.performed += OnDash;
    }

    void Start()
    {
        
    }

    void Update()
    {
        OnMove();
        //OnSimpleMove();
    }

    public void OnMove()
    {
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        Vector3 moveDir = transform.forward * moveSpeed * moveInput.y;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;


        moveDir.y = verticalVelocity;

        if (IsDashing)
        {
            moveDir = transform.forward * dashForce * (dashTimer / dashDuration);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
                IsDashing = false;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded)
        {
            return;
        }

        verticalVelocity = jumpForce;
    }

    //public void OnSimpleMove()
    //{
    //transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
    //Vector3 moveDir = transform.forward * moveSpeed * moveInput.y;
    //controller.SimpleMove(moveDir);
    //}



    private void OnDash(InputAction.CallbackContext context)
    {
        IsDashing = true;
        dashTimer = dashDuration;
    }

    private void OnRun(InputAction.CallbackContext context)
    {

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print(hit.gameObject.name);
        Vector3 pushDir = (hit.transform.position - transform.position).normalized;

        if (hit.rigidbody != null && hit.rigidbody.linearVelocity == Vector3.zero)
        {
            print(hit.gameObject.name);
            hit.rigidbody.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }
}
