using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFirstCase : Case
{
    public Collider2D First;
    public Collider2D Second;

    public Inventory inv;

    public bool FirstGoal;
    public bool SecondGoal;
    public bool Picked;
    public int slot;

    protected new void Start()
    {
        base.Start();
        caseName = "MEDICINE HUNT";
        Goals = new Goal[3];
        Goals[0] = new ReachGoal("ENTER THE OLD HAGS HOUSE", 0, 1);
        Goals[1] = new PickedGoal("FIND THE HAGDS MEDICINE", 0, 1);
        Goals[2] = new ReachGoal("LEAVE THE OLD HAGS HOUSE", 0, 1);
        EndGoalText = "GO BACK TO THE LADIE";
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        completed = false;
        FirstGoal = false;
        SecondGoal = false;
        Picked = false;
        Accepted = false;
        CashReward = 25;
        slot = -1;
    }

    protected override void OnEvent()
    {
        Enter();
        Exit();
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
        if (completed == false && FirstGoal && Begin && inv.Exists("Pills") != -1)
        {
            slot = inv.Exists("Pills");
            Manager.GoalUpdate();
            Goals[1].Increment(1);
            Picked = true;
        }
    }

    public void Exit()
    {
        if (Manager.Player.GetComponent<Collider2D>().IsTouching(Second) && completed == false && Begin && FirstGoal && Picked && SecondGoal == false)
        {
            Manager.GoalUpdate();
            Goals[2].Increment(1);
            SecondGoal = false;
            Complete();
        }
    }

    public override void Extra()
    {
        inv.Remove(slot);
    }
}


