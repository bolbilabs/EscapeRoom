using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue ()
    {
        //if (!GameControl.isFrozen)
        //{
            GameControl.GetInstance().dialogueManager.StartDialogue(dialogue);
        //}
    }
}
