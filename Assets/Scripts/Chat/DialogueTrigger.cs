using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject NPC { get; private set; }
    public GameObject Player;
    public List<Dialogue> dialogues;
    public Dialogue busy;
    public Dialogue reward;
    public bool repeat;
    public bool mission;

    public void OnTriggerDialogue()
    {
        NPC = gameObject;
        FindObjectOfType<DialogueManager>().StartDialogue(GetDialogue(), Player, NPC);
        gameObject.GetComponent<ShowUI>().Disappear();
    }

    public void OnTriggerBusyDialogue()
    {
        NPC = gameObject;
        FindObjectOfType<DialogueManager>().StartDialogue(busy, Player, NPC);
        gameObject.GetComponent<ShowUI>().Disappear();
    }

    public void OnTriggerRewardDialogue()
    {
        NPC = gameObject;
        FindObjectOfType<DialogueManager>().StartDialogue(reward, Player, NPC);
        gameObject.GetComponent<ShowUI>().Disappear();
    }

    public void OnInteraction()
    {
        if (Player != null)
        {
            NPC = gameObject;
            FindObjectOfType<DialogueManager>().StartInteractions(Player, NPC, mission);
            gameObject.GetComponent<ShowUI>().Disappear();
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public Dialogue GetDialogue()
    {
        Dialogue temp = dialogues[0];
        foreach (Dialogue dialogue in dialogues)
        {
            if (temp.priority < dialogue.priority)
                temp = dialogue;
        }
        return temp;
    }

    public void RemoveDialogue()
    {
        if (dialogues.Count > 1)
        {
            dialogues.Remove(GetDialogue());
        }
    }
}
