using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenManager : MonoBehaviour
{
    public string mainScene;
    public string mainMenu;
    private GameObject player;
    public Transform startPosition;

    private void Start()
    {
            
    }

    public void OnStartNewGame()
    {
        if (TryGetComponent(out SceneLoader loader))
        {
            loader.SetNewScene(mainScene, "ChangeScene");
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    public void OnLoadSavedGame()
    {
        LevelData level = SaveScript.LoadLevel();
        if (TryGetComponent(out SceneLoader loader))
        {
            loader.SetNewScene(level.Level, "LoadSave");
        }
    }
    public void OnSceneSave()
    {
        Debug.Log("Saving");
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                if(player.TryGetComponent(out Player saveControler))
                {
                    saveControler.SavePlayer();
                }
            }
        }
        if(TryGetComponent(out Level levelSaveControler))
        {
            levelSaveControler.setLevel(SceneManager.GetActiveScene().name);
            levelSaveControler.SaveLevel();
        }
    }
    public void OnMainMenu()
    {
        if (TryGetComponent(out SceneLoader loader))
        {
            loader.SetNewScene(mainMenu, "LoadMenu");
        }
    }
}
