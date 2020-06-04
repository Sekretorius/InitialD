using System.Collections;
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

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        //health = GameObject.Find("PlayerStats")HealthSystem;
        //Canvas = GameObject.FindGameObjectWithTag("UI");
        Canvas = GameObject.Find("Canvas");
        health = GameObject.Find("PlayerStats").GetComponent<HealthSystem>();
        health.OnDeath += OnPlayerDeath;
        //fade = deathCanvas.GetComponentInChildren<DeathFader>();

        //deathCanvas = GameObject.Find("Canvas_Death");


        //deathCanvas.SetActive(false);
        GameManager = GameObject.Find("GameManager");
    }

    private void FixedUpdate()
    {
        if (deathCanvas == null && !active)
        {
            deathCanvas = GameObject.FindGameObjectWithTag("Death");
        }
        else if(!active)
        {
            deathCanvas.SetActive(false);
            active = true;
        }
    }

    public void SetEvent()
    {
        deathCanvas.SetActive(false);
    }


    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
        deathCanvas.SetActive(true);
        Canvas.SetActive(false);
        StartCoroutine(ExampleCoroutine());

    }

    public void NormalSpeed()
    {
        Time.timeScale = 1;
    }

    IEnumerator ExampleCoroutine()
    {

        for (int i = 0; i < 50; i++)
        {
            Time.timeScale = 1 - i * 0.02f;
            yield return new WaitForSeconds(0.01f);           
        }
    }

    public void ReloadScene()
    {
        GameManager = GameObject.Find("GameManager");
        if (GameManager.TryGetComponent(out SceneLoader loader))
        {
            loader.SetNewScene(SceneManager.GetActiveScene().name, "ToCheckPoint");
            health.SetHearts(5);
        }
    }
}
