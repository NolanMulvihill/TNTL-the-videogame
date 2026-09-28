using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity = -9.81f;
    private Vector2 moveInput;
    private float verticalVelocity;

    [Header("Look Settings")]
    [SerializeField] private Transform cameraTransform; // Drag your Main Camera here
    [SerializeField] private float lookSensitivity = 1.5f; // Adjust up if using Mac trackpad
    private Vector2 lookInput;
    private float xRotation = 0f;

    void Start()
    {
        // Locks and hides the cursor for continuous mouse tracking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Called automatically by PlayerInput ("Send Messages" behavior)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Called automatically by PlayerInput when "Look" action triggers
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        // Up/Down: Pitch the camera only
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Left/Right: Yaw the entire player body
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        if (controller != null)
        {
            // 1. Calculate gravity
            if (controller.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; // Keep snapped to slopes/ground
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            // 2. Move relative to where the player is facing (local space)
            Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
            moveDirection.y = verticalVelocity;

            // 3. Move the character
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
            transform.Translate(move * Time.deltaTime, Space.World);
        }
    }
}