using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControls : MonoBehaviour
{
    public ControlBindings controls;

    public float walkSpeed = 0.03f, runSpeed = 4, rotateSpeed = 1f;

    private Vector2 inputs;
    private Vector3 velocity;

    private float currentSpeed;
    private float rotation;
    private float gravity = -15;
    private float velocityY;
    private float terminalVelocity = -25;
    private float jumpSpeed;
    private float jumpHeight = 4;

    private Vector3 jumpDirection;

    private bool run = true;
    private bool jumping;
    private bool jump;

    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Movement();
    }

    private void Movement()
    {
        Vector2 normalizedInputs = Vector2.zero;

        if (controller.isGrounded)
        {
            normalizedInputs = inputs;
            currentSpeed = walkSpeed;

            if (run)
            {
                currentSpeed *= runSpeed;

                if (normalizedInputs.y < 0)
                {
                    currentSpeed = currentSpeed / 2;
                }
            }
        }
        else
        {
            normalizedInputs = Vector2.Lerp(normalizedInputs, Vector2.zero, 0.025f);
        }

        // rotation
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        if (jump && controller.isGrounded)
        {
            Jump();
        }

        // apply gravity
        if (!controller.isGrounded && velocityY > terminalVelocity)
        {
            velocityY += gravity * Time.deltaTime;
        }

        // apply inputs
        if (!jumping)
        {
            velocity = (transform.forward * normalizedInputs.y + transform.right * normalizedInputs.x) * currentSpeed + Vector3.up * velocityY;
        }
        else
        {
            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;
        }


        // moving character controller
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (jumping)
            {
                jumping = false;
            }

            velocityY = 0;
        }
    }

    private void Jump()
    {
        if (!jumping)
        {
            jumping = true;
        }

        jumpDirection = (transform.forward * inputs.y + transform.right * inputs.x).normalized;

        jumpSpeed = currentSpeed;

        velocityY = Mathf.Sqrt(-gravity * jumpHeight);
    }

    private void GetInputs()
    {
        // forwards
        if (Input.GetKey(controls.forwards))
        {
            inputs.y = 1;
        }

        // backwards
        if (Input.GetKey(controls.backwards))
        {
            if (Input.GetKey(controls.forwards))
            {
                inputs.y = 0;
            }
            else
            {
                inputs.y = -1;
            }
        }

        // stand still
        if (!Input.GetKey(controls.forwards) && !Input.GetKey(controls.backwards))
        {
            inputs.y = 0;
        }

        // strafe right
        if (Input.GetKey(controls.strafeRight))
        {
            inputs.x = 1;
        }

        // strafe left
        if (Input.GetKey(controls.strafeLeft))
        {
            if (Input.GetKey(controls.strafeRight))
            {
                inputs.x = 0;
            }
            else
            {
                inputs.x = -1;
            }
        }

        // if no strafe
        if (!Input.GetKey(controls.strafeRight) && !Input.GetKey(controls.strafeLeft))
        {
            inputs.x = 0;
        }

        // rotate right
        if (Input.GetKey(controls.rotateRight))
        {
            rotation = 1;
        }

        // rotate left
        if (Input.GetKey(controls.rotateLeft))
        {
            if (Input.GetKey(controls.rotateRight))
            {
                rotation = 0;
            }
            else
            {
                rotation = -1;
            }
        }

        // if no rotation
        if (!Input.GetKey(controls.rotateRight) && !Input.GetKey(controls.rotateLeft))
        {
            rotation = 0;
        }

        if (Input.GetKeyDown(controls.walkRun))
        {
            if (run)
            {
                run = false;
            }
            else
            {
                run = true;
            }
        }

        // jumping
        jump = Input.GetKey(controls.jump);
    }
}
