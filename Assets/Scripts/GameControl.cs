using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public bool isPlayer = false;
    public CameraControl cameraControl;
    public CharacterInputController playerControl;

    public Rigidbody playerRb;
    public Rigidbody cameraRb;

    public float timer = 300f;

    public TextMeshProUGUI timerText;
    public Animator recordingAnim;

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
                recordingAnim.SetBool("isPlayer", false);
            }
            else
            {
                playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                cameraRb.constraints = RigidbodyConstraints.FreezeAll;
                playerControl.enabled = true;
                cameraControl.enabled = false;
                isPlayer = true;
                recordingAnim.SetBool("isPlayer", true);
            }
        }


        if (isPlayer)
        {
            timer -= Time.deltaTime;
        }

        string minutes = ((int)timer / 60).ToString("d2");
        string seconds = ((int)(timer % 60)).ToString("d2");
        string milliseconds = ((int)((timer - ((int)(timer)))*100)).ToString("d2");

        timerText.text = minutes + ":" + seconds + ":" + milliseconds;
    }
}
