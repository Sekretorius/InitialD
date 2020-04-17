using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour   
{
    public StoryLineManager Manager { get; private set; }

    public GameObject NPC;
    public List<Item> rewards;
    public int CashReward { get; set; }
    public string caseName { get; set; }
    public bool completed { get; set; }
    public bool Begin { get; set; }
    public bool Accepted { get; set; }

    public Dialogue RewardSpeach { get; set; }
    public Dialogue BusySpeach { get; set; }
    public string EndGoalText { get;set; }
    public Goal[] Goals;

    private void OnValidate()
    {
        Manager = GetComponentInParent<StoryLineManager>();
    }

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
            Manager.ShowOnScreen();
          //  Destroy(this);
        }
    }

    public void OnDestroy()
    {
        
    }

    public void Destroy()
    {
        Manager.@case = null;
        Manager.ShowOnScreen();
        int count = 0;
        foreach (Dialogue d in NPC.GetComponent<DialogueTrigger>().dialogues)
        {
            if (this == d.Case)
                NPC.GetComponent<DialogueTrigger>().dialogues.RemoveAt(count);
            count++;
        }
        Destroy(this);
    }

    public void Busy()
    {
        Manager.dialogueManager.StartDialogue(BusySpeach, Manager.Player, NPC);
    }

    public virtual void GiveRewards() {
        Manager.dialogueManager.StartDialogue(RewardSpeach, Manager.Player, NPC);
        foreach(Item item in rewards)
            Manager.Inv.Add(item);
        if (CashReward > 0)
            Manager.Cash.Add(CashReward);
        Accepted = true;
        Manager.onMission = false;
    }

    public bool IsTouchingNPC()
    {
        return Manager.Player.GetComponent<Collider2D>().IsTouching(NPC.GetComponent<Collider2D>());
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
