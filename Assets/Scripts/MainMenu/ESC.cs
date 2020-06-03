using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESC : MonoBehaviour
{
    private bool active;
    private GameObject canvas;
    private GameObject canvas_disable;
    private bool EscState = false;
    private bool UIState = false;

    private void Start()
    {
        active = false;
        canvas = GameObject.FindWithTag("Escape");
        canvas_disable = GameObject.FindWithTag("UI");
        canvas.SetActive(false);
        EscState = false;
        UIState = true;
    }

    public void changeState(bool escSt, bool UISt)
    {
        EscState = escSt;
        UIState = UISt;
    }
    void Update()
    {
        OnEvent();
    }
    private void OnEvent()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenuTemp"))
        {
            EscState = false;
            UIState = false;
        }
        if (canvas.activeInHierarchy != EscState)
        {
            canvas.SetActive(EscState);
        }
        if (canvas_disable.activeInHierarchy != UIState)
        {
            canvas_disable.SetActive(UIState);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (active)
            {
                EscState = false;
                UIState = true;
                Time.timeScale = 1;
                canvas.SetActive(false);
                canvas_disable.SetActive(true);
                active = false;
            }
            else
            {
                UIState = false;
                EscState = true;
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
        active = false
        EscState = false;
        UIState = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

}
