using System.Collections;
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

    public override void OnEvent()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.T) && manager.Chat == false)
            gameObject.GetComponent<DialogueTrigger>().OnTriggerDialogue();
        else if (manager.Chat == true && isInteractable && Input.GetKeyDown(KeyCode.T))
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
