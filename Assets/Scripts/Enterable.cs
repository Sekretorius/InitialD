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
    private bool freeze;

    private Vector3 position;


    void Start()
    {
        Fade = FindObjectOfType<UIFader>();
        position = new Vector3();
        fade = false;
        fadeOut = false;
        freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnEvent();
        StartFade(); // fading animation
        FreezePlayer(); // stoping players movement during the fadeIn part
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

    public void FreezePlayer()
    {
        if(freeze)
            Player.transform.position = position;
    }

    public void StartFade()
    {
        if (fade)
        {
            position = Player.transform.position;
            freeze = true;
            fadeOut = true;
            fade = false;
            Fade.FadeIn();
        }
        if (fadeOut && Fade.done)
        {
            freeze = false;
            fadeOut = false;
            Teleport();
            Fade.FadeOut();
        }
    }

}
