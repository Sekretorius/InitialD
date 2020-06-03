using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public static SceneLoader control;
    public bool change = false;
    private string changeState = " ";
    private bool IsChangingScene = false;

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
        if (TryGetComponent(out SceneFader controler))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (change && !IsChangingScene)
            {
                controler.FadeIn();
                IsChangingScene = true;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else if(controler.done && change)
            {
                ChangeScene(sceneToLoad);
            }
        }
    }
    public void SetNewScene(string sceneName, string state)
    {
        sceneToLoad = sceneName;
        change = true;
        changeState = state;
    }
    private void ChangeScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (TryGetComponent(out PlayerManager playerControler))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (TryGetComponent(out PlayerManager controler))
            {
                if (player == null)
                {
                    controler.CreatePlayer();
                }
            }
            if (changeState == "ChangeScene" && GameObject.Find("Check").activeInHierarchy)
            {
                Transform pos = GameObject.Find("Check").transform;
                pos.position = new Vector3(pos.position.x,pos.position.y,pos.position.z);
                playerControler.SetPosition(pos.position);
                GameObject.Find("Check").SetActive(false);
            }
            else if (changeState == "ChangeScene")
            {
                Transform pos = GameObject.FindGameObjectWithTag("Spawn").transform;
                playerControler.SetPosition(pos.position);
            }
            if (changeState == "LoadSave")
            {
                playerControler.LoadPlayer();
            }
        }
        change = false;
        IsChangingScene = false;
        if (TryGetComponent(out SceneFader faderControler))
        {
            faderControler.IsFadingOut = true;
        }
    }
}
