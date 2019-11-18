using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameControl : MonoBehaviour
{
    public bool isPlayer = false;
    public CameraControl cameraControl;
    public CharacterInputController playerControl;
    public Animator playerAnim;

    public Rigidbody playerRb;
    public Rigidbody cameraRb;

    public static float timer = 300f;

    public TextMeshProUGUI timerText;
    public Animator recordingAnim;

    public static GameControl instance;
    public bool isPersistant;

    public DialogueManager dialogueManager;

    public virtual void Awake()
    {
        if (isPersistant)
        {
            if (!instance)
            {
                instance = this as GameControl;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this as GameControl;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        cameraRb.constraints = RigidbodyConstraints.FreezeRotation;
        dialogueManager = GetComponent<DialogueManager>();
        cameraControl = Camera.main.GetComponent<CameraControl>();
        cameraRb = Camera.main.GetComponent<Rigidbody>();
        GameObject player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<CharacterInputController>();
        playerRb = player.GetComponent<Rigidbody>();
        timerText = GameObject.FindWithTag("Timer").GetComponent<TextMeshProUGUI>();
        recordingAnim = GameObject.FindWithTag("Record").GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        cameraRb = Camera.main.GetComponent<Rigidbody>();
        GameObject player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<CharacterInputController>();
        playerRb = player.GetComponent<Rigidbody>();
        timerText = GameObject.FindWithTag("Timer").GetComponent<TextMeshProUGUI>();
        recordingAnim = GameObject.FindWithTag("Record").GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();

        if (Input.GetKeyDown(KeyCode.R)) {
            isPlayer = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        if (!dialogueManager.inCutscene)
        {
            // Changes control from camera to the player
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (isPlayer)
                {
                    playerRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    cameraRb.constraints = RigidbodyConstraints.FreezeRotation;
                    playerControl.enabled = false;
                    cameraControl.enabled = true;
                    isPlayer = false;
                    recordingAnim.SetBool("isPlayer", false);
                    playerAnim.SetBool("isFalling", true);
                }
                else
                {
                    playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                    cameraRb.constraints = RigidbodyConstraints.FreezeAll;
                    playerControl.enabled = true;
                    cameraControl.enabled = false;
                    isPlayer = true;
                    recordingAnim.SetBool("isPlayer", true);
                    playerAnim.SetBool("isFalling", false);

                }
            }
        }


        if (isPlayer && !dialogueManager.inCutscene)
        {
            timer -= Time.deltaTime;
        }

        string minutes = ((int)timer / 60).ToString("d2");
        string seconds = ((int)(timer % 60)).ToString("d2");
        string milliseconds = ((int)((timer - ((int)(timer)))*100)).ToString("d2");

        timerText.text = minutes + ":" + seconds + ":" + milliseconds;
    }
}

