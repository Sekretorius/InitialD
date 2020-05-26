using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatable : Interactable
{
    private DialogueManager manager;
    private StoryLineManager storyManager;
    protected new void Start()
    {
        base.Start();
        storyManager = FindObjectOfType<StoryLineManager>();
        manager = FindObjectOfType<DialogueManager>();
    }
    protected override void OnEvent()
    {
        if (IsInteractable && manager.Chat == false && storyManager.onMission && storyManager.@case.completed && storyManager.@case.IsTouchingNPC() && storyManager.@case.Accepted == false)
        {
            storyManager.@case.GiveRewards();
            gameObject.GetComponent<DialogueTrigger>().OnTriggerRewardDialogue();
            storyManager.@case.Destroy_Case();
        }
        else if (IsInteractable && Input.GetButtonDown("Chat") && manager.Chat == false && storyManager.onMission && storyManager.@case.completed == false && storyManager.@case.IsTouchingNPC())
            //storyManager.@case.Busy();
            gameObject.GetComponent<DialogueTrigger>().OnTriggerBusyDialogue();
        else if (IsInteractable && Input.GetButtonDown("Chat") && manager.Chat == false)
        {
            gameObject.GetComponent<DialogueTrigger>().OnTriggerDialogue();
            Debug.Log("Gerai");
        }
        else if (manager.Chat == true && IsInteractable && Input.GetButtonDown("Chat"))
        {
            manager.DisplayDialogue();
            Debug.Log("Blogai");
        }
        else if (IsInteractable && manager.Chat == false)
            gameObject.GetComponent<DialogueTrigger>().OnInteraction();

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && storyManager.onMission && storyManager.@case.IsTouchingNPC() && storyManager.@case.completed)
        {
            Debug.Log("Mission exited");
            storyManager.@case.Destroy_Case();
            SetFalse();
            manager.StopDialogue();
            FindObjectOfType<DialogueManager>().DisableInteractions();
        }
        else if (collision.CompareTag("Player"))
        {
            SetFalse();
            manager.StopDialogue();
            FindObjectOfType<DialogueManager>().DisableInteractions();
        }
    }


}
