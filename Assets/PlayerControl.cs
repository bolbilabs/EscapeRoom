using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 0.2f;
    public float turnSpeed = 0.1f;
    Vector3 angleVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angleVelocity = new Vector3(0, turnSpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.MovePosition(rb.position + transform.forward * moveSpeed);
        }
        if (Input.GetKey("s"))
        {
            rb.MovePosition(rb.position - transform.forward * moveSpeed);
        }
        if (Input.GetKey("a"))
        {
            Quaternion deltaRotation = Quaternion.Euler(-angleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        if (Input.GetKey("d"))
        {
            Quaternion deltaRotation = Quaternion.Euler(angleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
