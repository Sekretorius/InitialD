﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceEnter : Interactable
{
    public GameObject Player;
    public GameObject Location;

    public UIFader Fade;

    private bool fade;
    private bool fadeOut;
    private bool animation;
    public bool isEntering { get; private set; }

    private Vector3 position;


    protected new void Start()
    {
        base.Start();
        Fade = FindObjectOfType<UIFader>();
        
        position = new Vector3();
        fade = false;
        fadeOut = false;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    OnEvent();

    //}

    protected override void OnEvent()
    {
        if (IsInteractable && !animation)
        {
            animation = true;
            fade = true;
        }
        StartFade(); // fading animation
    }

    public void Teleport()
    {
        Player.transform.position = new Vector3(Location.transform.position.x, Location.transform.position.y, 0);
    }

    public void StartFade()
    {
        if (fade)
        {
            isEntering = false;
            position = Player.transform.position;
            Player.GetComponent<PlayerControler>().FreezeMovement(false);
            fadeOut = true;
            fade = false;
            Fade.FadeIn();
        }
        if (fadeOut && Fade.done)
        {
            isEntering = true;
            Player.GetComponent<PlayerControler>().FreezeMovement(true);
            fadeOut = false;
            Teleport();
            Fade.FadeOut();
            animation = false;
        }
    }
}