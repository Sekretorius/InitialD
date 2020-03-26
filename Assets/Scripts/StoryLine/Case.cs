using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour   
{
    public int id;
    public string caseName;
    public Goal[] Goals;
    public bool completed;
    public List<string> Rewards;

    void Update()
    {
        OnEvent();
    }

    public virtual void OnEvent() { }

    public virtual void Complete()
    {
        if (Check())
        {
            completed = true;
            Debug.Log("Case closed!");
           // Destroy(this);
        }
    }

    public void GiveRewards() { }

    public bool Check()
    {
        foreach (Goal goal in Goals)
            if (goal.completed == false)
                return false;
        return true;
    }

}
