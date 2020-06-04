using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enterable : Interactable
{
    private GameObject Player;
    public GameObject Location;
    [SerializeField] AudioClip Door;
    [SerializeField] AudioSource Source;
    private UIFader Fade;

    public bool Locked;
    public int fee;

    private bool fade;
    private bool fadeOut;
    public bool isEntering { get; private set; }

    private Vector3 position;


    protected new void Start()
    {
        Door =(AudioClip) Resources.Load("Door");
        Source = GameObject.Find("Sound").GetComponent<AudioSource>();
        base.Start();
        Fade = FindObjectOfType<UIFader>();
        position = new Vector3();
        fade = false;
        fadeOut = false;
    }
    new void FixedUpdate()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        base.FixedUpdate();
    }

    protected override void OnEvent()
    {
        if (Player != null)
        {
            if (IsInteractable && (Input.GetButtonDown("Interact")) && Locked == false)
            {
                fade = true;
                Source.PlayOneShot(Door);
            } else if (IsInteractable && (Input.GetButtonDown("Interact")) && Locked == true )
            {
                MoneySystem money = GameObject.Find("PlayerStats").GetComponent<MoneySystem>();
                if (money.Cash >= fee)
                {
                    money.Add(-fee);
                    fade = true;
                    Source.PlayOneShot(Door);
                }
            }
            StartFade(); // fading animation
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void Teleport()
    {
        Player.transform.position = new Vector3(Location.transform.position.x, Location.transform.position.y, 0);
        GameObject.Find("Far Background").GetComponent<Parallax>().ReloadParallax();

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
        }
    }
}
