﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatable : Interactable
{
    private DialogueManager manager;
    private StoryLineManager storyManager;
    // Update is called once per frame
    void Update()
    {
        OnEvent();
    }
    private void OnValidate()
    {
        storyManager = FindObjectOfType<StoryLineManager>();
        manager = FindObjectOfType<DialogueManager>();
    }
    protected override void OnEvent()
    {
        if (IsInteractable && manager.Chat == false && storyManager.active && storyManager.@case.completed && storyManager.@case.IsTouchingNPC() && storyManager.@case.Accepted == false)
        {
            storyManager.@case.GiveRewards();
            storyManager.@case.Destroy();
        }
        else if (IsInteractable && Input.GetKeyDown(KeyCode.T) && manager.Chat == false && storyManager.active && storyManager.@case.completed == false && storyManager.@case.IsTouchingNPC())
            storyManager.@case.Busy();
        else if (IsInteractable && Input.GetKeyDown(KeyCode.T) && manager.Chat == false)
            gameObject.GetComponent<DialogueTrigger>().OnTriggerDialogue();
        else if (IsInteractable && manager.Chat == false)
            gameObject.GetComponent<DialogueTrigger>().OnInteraction();
        else if (manager.Chat == true && IsInteractable && Input.GetKeyDown(KeyCode.T))
            manager.DisplayDialogue();

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && storyManager.active && storyManager.@case.IsTouchingNPC() && storyManager.@case.completed)
        {
            Debug.Log("Mission exited");
            storyManager.@case.Destroy();
            SetFalse();
            manager.StopDialogue();
            FindObjectOfType<DialogueManager>().DisableInteractions();
        }
        else if (collision.tag == "Player")
        {
            SetFalse();
            manager.StopDialogue();
            FindObjectOfType<DialogueManager>().DisableInteractions();
        }
    }


}
