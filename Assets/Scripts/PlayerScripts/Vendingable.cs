using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendingable : Interactable
{
    public int cost;
    public int uses;

    private MoneySystem MSystem;
    private HealthSystem HSystem;
    private Text text;


    protected void Start()
    {
        MSystem = FindObjectOfType<MoneySystem>();
        HSystem = GameObject.Find("PlayerStats").GetComponent<HealthSystem>();
        text = GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
    }

    protected override void OnEvent()
    {
        if (IsInteractable && Input.GetButtonDown("Interact") && uses > 0)
        {
            if (MSystem.Cash <= 0)
            {
                text.text = "TOO BROKE TO USE";
            }
            else if(!HSystem.IsFullHealth())
            {
                uses--;
                MSystem.Add(cost);
                HSystem.Heal(2);
                // Health increase logic
                if (uses > 0)
                    text.text = "PRESS E TO USE";
                else
                    text.text = "OUT OF ORDER";
            }
            else
            {
                text.text = "TOO HEALTHY TO USE";
            }

        }
    }

    public void Refesh(int count)
    {
        uses = count;
    }

}
