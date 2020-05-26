using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase2 : Case
{
    public Collider2D First;
    public Collider2D Second;

    public bool FirstGoal;
    public bool SecondGoal;

    protected new void Start()
    {
        base.Start();
        caseName = "OFFICE HUNT";
        Goals = new ReachGoal[2];
        Goals[0] = new ReachGoal("REACH YOUR OFFCE",0,1);
        Goals[1] = new ReachGoal("EXIT YOUR OFFICE",0,1);
        EndGoalText = "GO BACK TO THE NPC";
        completed = false;
        FirstGoal = false;
        SecondGoal = false;
        Accepted = false;
        CashReward = 25;

        //RewardSpeach = new Dialogue(
        //    new string[] { "HERE YOU GO LAD",
        //                    },
        //    new string[] { "THANKS, I GUESS",
        //                    },
        //    false, 1, null
        //);

        //BusySpeach = new Dialogue(
        //    new string[] { "YOU LOOK BUSY, MAYBE NEXT TIME",
        //            },
        //    new string[] { "...",
        //            },
        //    false, 1, null
        //);

    }

    protected override void OnEvent()
    {
        Enter();
        Exit();
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

    public void Exit()
    {
        if (Manager.Player.GetComponent<Collider2D>().IsTouching(Second) && completed == false && Begin && FirstGoal && SecondGoal == false)
        {
            Manager.GoalUpdate();
            Goals[1].Increment(1);
            SecondGoal = false;
            Complete();
        }
    }

    //public override void GiveRewards()
    //{
    //    if (player.GetComponent<Collider2D>().IsTouching(NPC.GetComponent<Collider2D>()) && completed && Accepted == false)
    //    {
    //        FindObjectOfType<DialogueManager>().StartDialogue(RewardSpeach, player, NPC);
    //        Accepted = true;
    //    }
    //    else if (FindObjectOfType<DialogueManager>().Chat == false && completed && Accepted)
    //    {
    //        base.GiveRewards();
    //    }
    //}




}
