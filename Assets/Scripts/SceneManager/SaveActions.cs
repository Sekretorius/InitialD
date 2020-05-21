using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveActions : MonoBehaviour
{
    GameObject player;
    void OnSaveButtonClick()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null)
        {
            if(player.TryGetComponent(out Player controler))
            {
                controler.SavePlayer();
            }
        }
    }
}
