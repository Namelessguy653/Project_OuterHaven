using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Placeholder_Controller: MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;  // Degrees per second for smooth turning
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ApplyGravity();
    }

    void Move()
    {
        // Get input from keyboard
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Only move if there is input (direction.magnitude > 0)
        if (direction.magnitude >= 0.1f)
        {
            // Calculate the target angle based on camera facing direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Smoothly rotate towards the target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move the player in the direction they're facing
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Jumping logic (optional)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Stick to the ground
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player according to gravity
        controller.Move(velocity * Time.deltaTime);
    }
}
