using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string description;
    public int count;
    public int current;
    public bool completed;

    public void Increment(int amount)
    {
        current = Mathf.Min(current + amount, count);
        GameObject.Find("caseManager").GetComponent<StoryLineManager>().GoalUpdate();
        if (current >= count)
        {
            completed = true;
            Debug.Log("Goal was compleated!");
        }      
    }

    public virtual string getDescription()
    {
        return description + " " + current + "/" + count;
    }

}
