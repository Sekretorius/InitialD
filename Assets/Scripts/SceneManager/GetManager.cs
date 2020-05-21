using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetManager : MonoBehaviour
{
    public void OnGetManagerSave()
    {
        GameObject manager = GameObject.Find("GameManager");
        if(manager != null && manager.TryGetComponent(out LoadScreenManager saveManager))
        {
            saveManager.OnSceneSave();
        }
    }
    public void OnGetManagerMenu()
    {
        GameObject manager = GameObject.Find("GameManager");
        if (manager != null && manager.TryGetComponent(out LoadScreenManager saveManager))
        {
            saveManager.OnMainMenu();
        }
    }
    public void OnGetManagerNewGame()
    {
        GameObject manager = GameObject.Find("GameManager");
        if (manager != null && manager.TryGetComponent(out LoadScreenManager saveManager))
        {
            saveManager.OnStartNewGame();
        }
    }
    public void OnGetManagerLoadGame()
    {
        GameObject manager = GameObject.Find("GameManager");
        if (manager != null && manager.TryGetComponent(out LoadScreenManager saveManager))
        {
            saveManager.OnLoadSavedGame();
        }
    }
}
