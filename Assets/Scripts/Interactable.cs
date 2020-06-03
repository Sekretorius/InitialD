using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool IsInteractable { get; private set; }
    public Transform interactingObject;

    protected void Start()
    {
        interactingObject = null;
        IsInteractable = false;
    }
    protected void Update()
    {
        OnEvent();
    }
    protected void FixedUpdate()
    {
        OnEventFixed();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInteractable = true;
            interactingObject = collision.transform;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInteractable = true;
            if (interactingObject == null)
            {
                interactingObject = collision.transform;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInteractable = false;
        }
    }

    protected virtual void OnEvent()
    {
        if (IsInteractable && Input.GetButton("Interact") && gameObject.CompareTag("Interact"))
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;

    }
    protected virtual void OnEventFixed()
    {

    }
    public void SetFalse()
    {
        IsInteractable = false;
    }
}
