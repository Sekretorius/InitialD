using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatable : Interactable
{

    // Update is called once per frame
    void Update()
    {
        OnEvent();
    }

    public override void OnEvent()
    {
        if (isInteractable && Input.GetKey(KeyCode.E))
            gameObject.GetComponent<DialogueTrigger>().OnTriggerDialogue();
    }

}
