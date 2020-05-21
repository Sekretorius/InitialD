using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public string level;
    public void setLevel(string Id)
    {
        level = Id;
    }
    public void SaveLevel()
    {
        SaveScript.SaveLevel(this);
    }
}
