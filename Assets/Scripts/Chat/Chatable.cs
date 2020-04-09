﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatable : Interactable
{
    public DialogueManager manager;
    // Update is called once per frame
    void Update()
    {
        OnEvent();
    }
    protected override void OnEvent()
    {
        if (IsInteractable && Input.GetKeyDown(KeyCode.E) && manager.Chat == false)      
            gameObject.GetComponent<DialogueTrigger>().OnInteraction();
        
        if (IsInteractable && Input.GetKeyDown(KeyCode.T) && manager.Chat == false)
            gameObject.GetComponent<DialogueTrigger>().OnTriggerDialogue();
        else if (manager.Chat == true && IsInteractable && Input.GetKeyDown(KeyCode.T))
            manager.DisplayDialogue();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SetFalse();
            manager.StopDialogue();
        }
    }


}
