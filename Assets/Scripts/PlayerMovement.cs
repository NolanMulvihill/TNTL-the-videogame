using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private CharacterController controller; // Or Rigidbody / Rigidbody2D
    [SerializeField] private float gravity = -9.81f;
    private Vector2 moveInput;
    private float verticalVelocity;

    // Called automatically by PlayerInput ("Send Messages" behavior)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (controller != null)
        {
            // 1. Calculate gravity
            if (controller.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; // Small constant downward force to stay snapped to ground
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            // 2. Combine horizontal movement and vertical velocity
            Vector3 move = new Vector3(moveInput.x * moveSpeed, verticalVelocity, moveInput.y * moveSpeed);

            // 3. Move the character once per frame
            controller.Move(move * Time.deltaTime);
        }
        else
        {
            // Fallback if no CharacterController is attached
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}