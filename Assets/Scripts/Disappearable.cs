using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Disappearable : Interactable
{
    GameObject Player;
    Tilemap tileMap;
    private bool exited;
    private bool inside;

    // Start is called before the first frame update
    private void OnValidate()
    {
        Player = GameObject.FindWithTag("Player");
        tileMap = GetComponent<Tilemap>();
        exited = true;
        inside = false;
    }

    protected override void OnEvent()
    {
        if (IsInteractable && !inside)
        {
            StartCoroutine(FadeToNothing(0.5f, tileMap));
            inside = true;
            exited = false;
        } else if (!exited && !IsInteractable)
        {
            StartCoroutine(FadeToSomething(0.5f, tileMap));
            exited = true;
            inside = false;
        }
    }

    public IEnumerator FadeToNothing(float t, Tilemap i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeToSomething(float t, Tilemap i)
    {
        //i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i.color.a < 1)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }


}
