using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    bool isPlayer = false;
    public CameraControl cameraControl;
    public PlayerControl playerControl;

    public Rigidbody playerRb;
    public Rigidbody cameraRb;

    private void Start()
    {
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        cameraRb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Changes control from camera to the player
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (isPlayer)
            {
                playerRb.constraints = RigidbodyConstraints.FreezeAll;
                cameraRb.constraints = RigidbodyConstraints.FreezeRotation;
                playerControl.enabled = false;
                cameraControl.enabled = true;
                isPlayer = false;
            }
            else
            {
                playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                cameraRb.constraints = RigidbodyConstraints.FreezeAll;
                playerControl.enabled = true;
                cameraControl.enabled = false;
                isPlayer = true;
            }
        }
    }
}
