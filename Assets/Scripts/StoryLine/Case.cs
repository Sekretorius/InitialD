using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour   
{
    public List<Item> rewards;
    public int id { get; set; }
    public string caseName { get; set; }
    public bool completed { get; set; }
    public bool Begin { get; set; }
    public bool Accepted { get; set; }
    public string EndGoalText { get;set; }

    public Goal[] Goals;

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

            Goal[] update = new Goal[Goals.Length + 1];

            int count = 0;
            foreach (Goal goal in Goals)
                update[count++] = goal;

            update[count] = new EndGoal(EndGoalText);
            Goals = update;
            GetComponentInParent<StoryLineManager>().ShowOnScreen();
          //  Destroy(this);
        }
    }

    public void OnDestroy()
    {
        
    }

    public virtual void GiveRewards() {
        GetComponentInParent<StoryLineManager>().@case = null;
        GetComponentInParent<StoryLineManager>().ShowOnScreen();
        Destroy(this);
    }

    public bool Check()
    {
        foreach (Goal goal in Goals)
            if (goal.completed == false)
                return false;
        return true;
    }

    public class EndGoal : Goal
    {
        public EndGoal(string description)
        {
            this.description = description;
        }

        public override string getDescription()
        {
            return description;
        }
    }

}
