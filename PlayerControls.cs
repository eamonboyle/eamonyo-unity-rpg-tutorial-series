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
    private float rotation;

    [SerializeField]
    private bool run = true;

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
        Vector2 normalizedInputs = inputs;

        // rotation
        Vector3 characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed, 0);
        transform.eulerAngles = characterRotation;

        // running walking
        float speed = walkSpeed;

        if (run)
        {
            speed *= runSpeed;

            if (normalizedInputs.y < 0)
            {
                speed = speed / 2;
            }
        }

        // veloctiy
        velocity = (transform.forward * normalizedInputs.y + transform.right * normalizedInputs.x) * speed;

        // moving character controller
        controller.Move(velocity);
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

        // jumping
    }
}
