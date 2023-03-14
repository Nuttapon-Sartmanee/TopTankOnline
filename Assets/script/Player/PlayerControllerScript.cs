using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerScript : NetworkBehaviour
{
    public float speed = 3.0f;
    public float rotationSpeed = 1.0f;

    public GameObject TankModel;
    private Animator animator;
    private Rigidbody rb;
    private bool running;

    void Start()
    {
        animator = TankModel.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        running = false;
    }

    void moveForward()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            // move forward only
            if (verticalInput > 0.01f)
            {
                float translation = verticalInput * speed;
                translation *= Time.fixedDeltaTime;
                rb.MovePosition(rb.position + this.transform.forward * translation);

                if (!running)
                {
                    running = true;
                    animator.SetBool("Walking", true);
                }
            }

            if (verticalInput < -0.01f)
            {
                float translation = -verticalInput * speed;
                translation *= Time.fixedDeltaTime;
                rb.MovePosition(rb.position - this.transform.forward * translation);

                if (!running)
                {
                    running = true;
                    animator.SetBool("Walking", true);
                }
            }
        }

        else if (running)
        {
            running = false;
            animator.SetBool("Walking", false);
        }
    }

    void turn()
    {
        float rotation = Input.GetAxis("Horizontal");
        if (rotation != 0)
        {
            rotation *= rotationSpeed;
            Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        moveForward();
        turn();
    }
}
