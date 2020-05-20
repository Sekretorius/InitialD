using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESC : MonoBehaviour
{
    public bool active;
    private GameObject canvas;
    private GameObject canvas_disable;

    private void Start()
    {
        active = false;
        canvas = GameObject.FindWithTag("Escape");
        canvas_disable = GameObject.FindWithTag("UI");
        canvas.SetActive(false);
    }

    void Update()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (active)
            {
                Time.timeScale = 1;
                canvas.SetActive(false);
                canvas_disable.SetActive(true);
                active = false;
            }
            else
            {
                Time.timeScale = 0;
                canvas_disable.SetActive(false);
                canvas.SetActive(true);
                active = true;
            }
        }
    }

    public void UnFreeze()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
        canvas_disable.SetActive(true);
        active = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

}
