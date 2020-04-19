using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendingable : Interactable
{
    public int cost;
    public int uses;

    private MoneySystem System;
    private Text text;


    private void OnValidate()
    {
        System = FindObjectOfType<MoneySystem>();
        text = GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
    }

    protected override void OnEvent()
    {
        if (IsInteractable && Input.GetKeyDown(KeyCode.E) && uses > 0)
        {
            uses--;
            System.Add(cost);
            // Health increase logic
            if (uses > 0)
                text.text = "PRESS E TO USE";
            else
                text.text = "OUT OF ORDER";
        }
    }

    public void Refesh(int count)
    {
        uses = count;
    }

}
