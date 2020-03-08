using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject NPC { get; private set; }
    public GameObject Player;
    public Dialogue dialogue;
    public bool repeat;
    public int importance;

    public void OnTriggerDialogue()
    {
        NPC = gameObject;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, Player, NPC);
        gameObject.GetComponent<ShowUI>().Disappear();
    }
}
