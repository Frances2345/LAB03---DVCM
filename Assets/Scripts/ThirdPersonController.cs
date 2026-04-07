using NUnit.Framework.Constraints;
using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    private InputSystem_Actions inputs;
    private CharacterController controller;
    public CinemachineCamera characterCamera;

    [SerializeField] private Vector2 moveInput;

    public float moveSpeed = 20f;
    public float runMultiplier = 2f;
    public float rotationSpeed = 20f;


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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputs.Player.Jump.performed += OnJump;
        inputs.Player.Sprint.performed += OnDash;

        inputs.Player.Sprint.started += ctx => isRunning = true;
        inputs.Player.Sprint.started += ctx => isRunning = false;
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
        Vector3 CameraForward = characterCamera.transform.forward;
        CameraForward.y = 0f;
        CameraForward.Normalize();

        if (moveInput != Vector2.zero)
        {

            Quaternion targetQuaternion = Quaternion.LookRotation(CameraForward);
            //transform.rotation = targetQuaternion;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotationSpeed* Time.deltaTime);

        }

        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;
        Vector3 moveDir = CameraForward * currentSpeed * moveInput.y;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            moveDir.y = verticalVelocity;
        }

        if (IsDashing)
        {
            moveDir = transform.forward * dashForce;
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                IsDashing = false;
            }
        }

        controller.Move(moveDir * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
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
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

}
