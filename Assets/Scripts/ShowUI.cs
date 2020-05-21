using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{

    public GameObject UI_object;
    // Start is called before the first frame update
    void Start()
    {
        if(UI_object != null)
            UI_object.SetActive(false);
    }

    public void Disappear()
    {
        UI_object.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (UI_object != null)
                UI_object.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (UI_object != null)
                UI_object.SetActive(false);
    }

}
