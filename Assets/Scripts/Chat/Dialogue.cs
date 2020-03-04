using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool isPlayerFirst; // Is the player gonna talk first.

    [TextArea(1,100)]
    public string[] sentences;
    [TextArea(1, 100)]
    public string[] responses;
}
