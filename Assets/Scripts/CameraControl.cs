using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class CameraControl : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float rotationY = 0F;

    public float moveSpeed = 0.5f;

    private Rigidbody rb;

    private void Start() 
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        rotationY = transform.rotation.x;
        transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        rb = GetComponent<Rigidbody>();
    }



    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }

        if (Input.GetKey("w"))
        {
            //transform.Translate(Vector3.forward * moveSpeed);
            rb.MovePosition(rb.position + transform.forward * moveSpeed);

        }
        if (Input.GetKey("s"))
        {
            //transform.Translate(-Vector3.forward * moveSpeed);
            rb.MovePosition(rb.position - transform.forward * moveSpeed);
        }
        if (Input.GetKey("a"))
        {
            //transform.Translate(-Vector3.right * moveSpeed);
            rb.MovePosition(rb.position - transform.right * moveSpeed);
        }
        if (Input.GetKey("d"))
        {
            //transform.Translate(Vector3.right * moveSpeed);
            rb.MovePosition(rb.position + transform.right * moveSpeed);
        }

        if (Input.GetKey("e"))
        {
            //transform.Translate(Vector3.right * moveSpeed);
            rb.MovePosition(rb.position + Vector3.up * moveSpeed);
        }
        if (Input.GetKey("q"))
        {
            //transform.Translate(Vector3.right * moveSpeed);
            rb.MovePosition(rb.position + -Vector3.up * moveSpeed);
        }
    }
}
