﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDeath : MonoBehaviour
{

    //private GameObject playerStats;
    private HealthSystem health;
    private DeathFader fade;
    private GameObject deathCanvas;
    private GameObject Canvas;
    private GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        //health = GameObject.Find("PlayerStats")HealthSystem;
        deathCanvas = GameObject.Find("Canvas_Death");
        //Canvas = GameObject.FindGameObjectWithTag("UI");
        Canvas = GameObject.Find("Canvas");
        health = GetComponent<HealthSystem>();
        fade = deathCanvas.GetComponentInChildren<DeathFader>();
        health.OnDeath += OnPlayerDeath;
        deathCanvas.SetActive(false);
        GameManager = GameObject.Find("GameManager");
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
        deathCanvas.SetActive(true);
        Canvas.SetActive(false);
    }

    public void ReloadScene()
    {
        GameManager = GameObject.Find("GameManager");
        if (GameManager.TryGetComponent(out SceneLoader loader))
        {
            loader.SetNewScene(SceneManager.GetActiveScene().name, "ChangeScene");
        }
    }
}