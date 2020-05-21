using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterScene : Interactable
{
    public string sceneToLoad;
    private GameObject GameManager;
    protected new void Start()
    {
        base.Start();
        GameManager = GameObject.Find("GameManager");
    }

    protected override void OnEvent()
    {
        if (IsInteractable && (Input.GetButtonDown("Interact")))
        {
            if (GameManager.TryGetComponent(out SceneLoader loader))
            {
                loader.SetNewScene(sceneToLoad.name, "ChangeScene");
            }
        }
    }
}
