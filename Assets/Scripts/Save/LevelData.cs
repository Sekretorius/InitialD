using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string Level;

    public LevelData(Level level)
    {
        this.Level = level.level;
    }
}
