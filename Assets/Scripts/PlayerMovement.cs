using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")]
    public float gravity = -9.81f; // Gravity, set to Earth's gravity. Note it's negative because it pulls objects downward.
    Vector3 verticalVelocity; // This will store our accumulated downward movement due to gravity.

    [Header("Movement")]
    public CharacterController controller;
    public Transform orientation;
    public float speed = 8f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float groundDrag;

    public float jumpHeight = 3f;
    public float airMultiplier;
    bool readyToJump;

    
    [Header("Ground Check")]
    public LayerMask groundMask;
    bool grounded;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    private void Start() 
    {
        readyToJump = true;
    }

    void Update()
    {
        Gravity();
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(grounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -3f;
            readyToJump = true;
        }
        MovePlayer();
        if(Input.GetKey(jumpKey))
            Jump();
    }

    private void MovePlayer()
    {
        // calculate movement direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + orientation.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void Gravity()
    {
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
