using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item;
    public bool IsInteractable { get; private set; }
    public Transform interactingObject;

    protected void Start()
    {
        interactingObject = null;
        IsInteractable = false;
    }
    private void Update()
    {
        OnEvent();
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
        if (IsInteractable && Input.GetButton("Interact") && gameObject.CompareTag("PickableItem"))
        {
            var sript = GameObject.Find("Inventory").GetComponent<Inventory>();
           if( sript.Add(item))
                Destroy(gameObject);
        }
    }

    public void SetFalse()
    {
        IsInteractable = false;
    }


    private void OnValidate()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (item != null && item.Icon != null)
        {
            spriteRenderer.sprite = item.Icon;
        }


    }
}

   
