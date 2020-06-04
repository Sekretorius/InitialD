using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCase : Case
{
    public Collider2D First;
    //public Collider2D Second;

    public Inventory inv;

    public bool FirstGoal;
    public bool Picked;
    public int slot;

    protected new void Start()
    {
        base.Start();
        caseName = "EXPENSIVE CAT";
        Goals = new Goal[2];
        Goals[0] = new ReachGoal("GO RIGHT", 0, 1);
        Goals[1] = new PickedGoal("TRY TO FIDN THE CAT", 0, 1);
        EndGoalText = "GOOD ENOUGH, BRING THE \"CAT\" BACK HOME";
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        completed = false;
        FirstGoal = false;
        Picked = false;
        Accepted = false;
        CashReward = 25;
        slot = -1;
    }

    protected override void OnEvent()
    {
        Enter();
        //Exit();
        PickedUp();
    }

    public void Enter()
    {
        if (Manager.Player.GetComponent<Collider2D>().IsTouching(First) && completed == false && FirstGoal == false && Begin)
        {
            Manager.GoalUpdate();
            Goals[0].Increment(1);
            FirstGoal = true;
        }
    }

    public void PickedUp()
    {
        if (completed == false && FirstGoal && Begin && inv.Exists("Chalice") != -1)
        {
            slot = inv.Exists("Chalice");
            Manager.GoalUpdate();
            Goals[1].Increment(1);
            Complete();
        }
    }

    //public void Exit()
    //{
    //    if (Manager.Player.GetComponent<Collider2D>().IsTouching(Second) && completed == false && Begin && FirstGoal && Picked && SecondGoal == false)
    //    {
    //        Manager.GoalUpdate();
    //        Goals[2].Increment(1);
    //        SecondGoal = false;
    //        Complete();
    //    }
    //}

    public override void Extra()
    {
        inv.Remove(slot);
    }
}


