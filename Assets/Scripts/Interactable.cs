using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; private set; }
    public Transform interactingObject;

    protected void Start()
    {
        interactingObject = null;
        isInteractable = false;
    }
    private void Update()
    {
        OnEvent();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInteractable = true;
            interactingObject = collision.transform;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInteractable = true;
            if (interactingObject == null)
            {
                interactingObject = collision.transform;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInteractable = false;
        }
    }

    protected virtual void OnEvent()
    {
        if (isInteractable && Input.GetKey(KeyCode.E))
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void SetFalse()
    {
        isInteractable = false;
    }
}
