using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy control;
    private bool state = false;
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != null)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenuTemp") && state)
        {
            disableChildren();
        }
        else if (!SceneManager.GetActiveScene().name.Equals("MainMenuTemp") && !state)
        {
            enableChildren();
        }
    }

    void disableChildren()
    {
        state = false;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Canvas canvas))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    void enableChildren()
    {
        state = true;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Canvas canvas))
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
