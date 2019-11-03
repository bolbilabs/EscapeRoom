using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MexicanJumpingBean : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider thisCollider;

    public float jumpForce;

    public bool isGrounded = false;

    public bool isCoroutine = false;

    public float maxRandTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        thisCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrounded && !isCoroutine)
        {
            StartCoroutine("waitForTime");
        }
    }

   
    public IEnumerator waitForTime()
    {
        isCoroutine = true;
        float timewaiting = Random.Range(0.0f, maxRandTime);
        yield return new WaitForSeconds(timewaiting);
        if (isGrounded)
        {
            Jump();
        }
        yield return new WaitForSeconds(0.5f);
        isCoroutine = false;
    }

    void Jump()
    {
        rb.AddTorque(new Vector3(Random.Range(0f,200f), Random.Range(0f, 200f), Random.Range(0f, 200f)), ForceMode.VelocityChange);
        rb.AddForce(new Vector3(Random.Range(-1f, 1f) * jumpForce, jumpForce, Random.Range(-1f, 1f) * jumpForce), ForceMode.VelocityChange);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ground")
        {
            isGrounded = false;
        }
    }


    //function IsGrounded(): boolean {
    //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1);
    //}

    //bo
}
