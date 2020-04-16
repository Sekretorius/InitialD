using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour   
{
    public GameObject Player { get; private set; }
    public StoryLineManager Manager { get; private set; }
    public DialogueManager dialogueManager { get; private set; }
    public Inventory Inv { get; private set; }

    public GameObject NPC;
    public List<Item> rewards;
    public int id { get; set; }
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
        Player = GameObject.FindWithTag("Player");
        Manager = GetComponentInParent<StoryLineManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        Inv = FindObjectOfType<Inventory>();
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
            GetComponentInParent<StoryLineManager>().ShowOnScreen();
          //  Destroy(this);
        }
    }

    public void OnDestroy()
    {
        
    }

    public void Destroy()
    {
        GetComponentInParent<StoryLineManager>().@case = null;
        GetComponentInParent<StoryLineManager>().ShowOnScreen();
        Destroy(this);
    }

    public void Busy()
    {
        dialogueManager.StartDialogue(BusySpeach, Player, NPC);
    }

    public virtual void GiveRewards() {
        dialogueManager.StartDialogue(RewardSpeach, Player, NPC);
        foreach(Item item in rewards)
            Inv.Add(item);
        Accepted = true;
    }

    public bool IsTouchingNPC()
    {
        return Player.GetComponent<Collider2D>().IsTouching(NPC.GetComponent<Collider2D>());
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
