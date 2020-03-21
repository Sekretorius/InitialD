using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enterable : Interactable
{
    public GameObject Player;
    public GameObject Location;

    private UIFader Fade;

    private bool fade;
    private bool fadeOut;

    private Vector3 position;


    void Start()
    {
        Fade = FindObjectOfType<UIFader>();
        position = new Vector3();
        fade = false;
        fadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnEvent();
        StartFade(); // fading animation
    }

    public override void OnEvent()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
            fade = true;     
    }

    public void Teleport()
    {
        Player.transform.position = new Vector3(Location.transform.position.x, Location.transform.position.y, 0);
    }

    public void StartFade()
    {
        if (fade)
        {
            position = Player.transform.position;
            Player.GetComponent<PlayerControler>().FreezeMovement();
            fadeOut = true;
            fade = false;
            Fade.FadeIn();
        }
        if (fadeOut && Fade.done)
        {
            Player.GetComponent<PlayerControler>().FreezeMovement();
            fadeOut = false;
            Teleport();
            Fade.FadeOut();
        }
    }

}
