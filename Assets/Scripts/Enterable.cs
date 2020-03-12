using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enterable : Interactable
{
    public GameObject Player;
    public GameObject Location;

    // Update is called once per frame
    void Update()
    {
        OnEvent();
    }

    public override void OnEvent()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
            Player.transform.position = new Vector3(Location.transform.position.x, Location.transform.position.y,0);       
    }

}
