using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public bool repeat;
    public int importance;

    public void OnTriggerDialogue()
    {
        foreach (string item in dialogue.sentences)
        {
            Debug.Log(item);
        }
        
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        
    }
}
