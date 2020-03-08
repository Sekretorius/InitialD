using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public bool isInteractable { get; private set; }
    public bool exit { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        isInteractable = false;
        exit = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            isInteractable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            isInteractable = false;
        exit = true;
    }

    public virtual void OnEvent()
    {
        if (isInteractable && Input.GetKey(KeyCode.E))
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
