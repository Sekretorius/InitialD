﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cashable : Interactable
{
    public int cash;
    public float fadeSpeed;
    public MoneySystem System;

    private bool cashed;

    private void OnValidate()
    {
        System = FindObjectOfType<MoneySystem>();
        cashed = false;
    }

    protected override void OnEvent()
    {
        if (IsInteractable && cashed == false)
        {
            System.Add(cash);
            cashed = true;
            StartCoroutine(FadeImageToZeroAlpha(fadeSpeed));
        }
    }

    public IEnumerator FadeImageToZeroAlpha(float t)
    {
        SpriteRenderer image = GetComponent<SpriteRenderer>();
        image.color = new Color(1, 1, 1, t);
        while (image.color.a > 0.0f)
        {
            image.color = new Color(1, 1, 1, image.color.a - (Time.deltaTime / t));
            yield return null;
        }
       gameObject.SetActive(false);
    }

}