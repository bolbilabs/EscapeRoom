using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    //private Queue<Sprite> images;

    public TextMeshProUGUI dialogueText;


    public TextMeshProUGUI shortDialogueText;

    public TextMeshProUGUI longDialogueText;


    //public Animator animator;

    //public Image image;

    public float timeDelay = 1.0f;

    private bool coroutineRunning = false;
    private string currentSentence;
    
    public bool inCutscene = false;

    //private Rigidbody2D rb;
    
    private MonoBehaviour[] scriptTriggers;

    public int lineSize = 38;

    public int bigLineSize = 38;
    public int smallLineSize = 0;

    private int currentLine = 0;
    private int lineOffset = 0;

    private int currentChar = 0;

    public Animator windowAnim;

    //private bool hasImage = false;

    public CharacterInputController characterInput;
    public CameraControl cameraInput;


    //public AudioSource cameraAudio;
    //public AudioClip openSound;
    //public AudioClip closeSound;

    bool again;

    //public bool finalCutscene = false;


    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
        //images = new Queue<Sprite>();
        scriptTriggers = new MonoBehaviour[0];
        
    }


    void Start()
    {
        //windowAnim = shortDialogueText.transform.parent.GetComponent<Animator>();
    }

    public void StartDialogue (Dialogue dialogue)
    {

        inCutscene = true;

        currentLine = 0;

        currentChar = 0;

        lineOffset = 0;

        windowAnim.SetBool("IsOpen", true);
        characterInput.enabled = false;
        cameraInput.enabled = false;
        //if (!again && !finalCutscene)
        //{
        //    cameraAudio.PlayOneShot(openSound);
        //}

        //GameControl.FreezeOverworld();


        sentences = new Queue<string>();
        //images = new Queue<Sprite>();
        scriptTriggers = new MonoBehaviour[0];


        //rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;


        //Debug.Log("Starting conversation with " + dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        //foreach (Sprite picture in dialogue.face)
        //{
        //    images.Enqueue(picture);
        //}

        scriptTriggers = dialogue.scriptTriggers;

        //DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (!coroutineRunning)
        {
            if (sentences.Count == 0)
            {
                //if (finalCutscene)
                //{
                //    return;
                //}
                EndDialogue();
                return;
            }


            //if (images.Peek() != null)
            //{
            //    hasImage = true;
            //    image.sprite = images.Dequeue();
            //    image.gameObject.SetActive(true);
            //    shortDialogueText.gameObject.SetActive(true);
            //    longDialogueText.gameObject.SetActive(false);
            //    dialogueText = shortDialogueText;
            //}
            //else
            //{
            //    hasImage = false;
            //    images.Dequeue();
            //    image.gameObject.SetActive(false);
            //    shortDialogueText.gameObject.SetActive(false);
            //    longDialogueText.gameObject.SetActive(true);
            //    dialogueText = longDialogueText;
            //}
            currentLine = 0;

            currentChar = 0;

            lineOffset = 0;
            string sentence = sentences.Dequeue();
            //Debug.Log(sentence);
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));


        }
        else
        {
            dialogueText.text = "<mspace=0.75em>" + currentSentence;
            StopAllCoroutines();
            coroutineRunning = false;
        }




    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "<mspace=0.75em>";
        currentSentence = sentence;
        coroutineRunning = true;
        foreach (char letter in sentence.ToCharArray())
        {
            bool ignore = false;
            if (letter != '&' && letter != '%' && letter != '@')
            {
                dialogueText.text += letter;
            }
            //if (letter == ' ')
            //{
            //    string subStr = sentence.Substring(currentChar + 1);
            //    lineOffset = 0;
            //    bool escape = false;
            //    foreach (char letter2 in subStr.ToCharArray())
            //    {
            //        if (!escape)
            //        {
            //            if (letter2 == ' ')
            //            {
            //                escape = true;
            //                break;
            //            }
            //            if (currentLine + lineOffset > lineSize - 1)
            //            {
            //                dialogueText.text += '\n';
            //                //currentLine = -1;
            //                escape = true;
            //                ignore = true;
            //                break;
            //            }
            //            if (letter != '&' && letter != '%')
            //            {
            //                lineOffset++;
            //            }
            //        }
            //    }
            //}
            if (letter == '&')
            {
                yield return new WaitForSeconds(timeDelay);
                currentLine -= 1;

            }
            if (letter == '%')
            {
                dialogueText.text += '\n';
                currentLine = -1;
            }
            if (!ignore)
            {
                currentChar++;
                currentLine++;
            }
            yield return new WaitForSeconds(timeDelay);
        }
        coroutineRunning = false;
    }

    public void EndDialogue()
    {
        //Debug.Log("End of Conversation");
        //animator.SetBool("IsOpen", false);
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        inCutscene = false;

        windowAnim.SetBool("IsOpen", false);
        //GameControl.UnfreezeOverworld();
        if (GetComponent<GameControl>().isPlayer)
        {
            characterInput.enabled = true;
            cameraInput.enabled = false;
        }
        else
        {
            characterInput.enabled = false;
            cameraInput.enabled = true;
        }

        again = false;
        if (scriptTriggers.Length > 0)
        {
            foreach (MonoBehaviour script in scriptTriggers)
            {
                if (script != null)
                {
                    script.enabled = true;
                    if (script is TriggerDialogueStart)
                    {
                        again = true;
                    }
                }
            }
        }

       
        //if (!again && !finalCutscene)
        //{
        //    cameraAudio.PlayOneShot(closeSound);
        //}

    }


    void Update()
    {
        if (inCutscene && Input.GetKeyDown("f"))
        {
            DisplayNextSentence();
        }
    }
}
