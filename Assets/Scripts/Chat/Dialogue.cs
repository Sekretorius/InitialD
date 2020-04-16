using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public bool isPlayerFirst; // Is the player gonna talk first.
    public int priority; // Is the player gonna talk first.
    public int CaseId;

    [TextArea(1,100)]
    public string[] sentences;
    [TextArea(1, 100)]
    public string[] responses;

    public Dialogue(string[] sentences, string[] responses, bool isPlayerFirst, int priority, int CaseId)
    {
        this.isPlayerFirst = isPlayerFirst;
        this.priority = priority;
        this.CaseId = CaseId;
        this.sentences = sentences;
        this.responses = responses;
    }

}
