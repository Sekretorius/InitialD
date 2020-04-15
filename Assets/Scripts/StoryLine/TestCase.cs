using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase : Case
{
    public CapsuleCollider2D player;
    public Collider2D NPC;

    public Collider2D destination;
    public List<Item> Rewards;

    void Awake()
    {
        caseName = "HOME, SWEET HOME";
        id = 1;
        Goals = new ReachGoal[1];
        Goals[0] = new ReachGoal("LEAVE THE OFFICE",0,1);
        EndGoalText = "GO BACK TO THE NPC";
        completed = false;
    }

    public override void OnEvent()
    {
        if (player.IsTouching(destination) && completed == false && Begin == true)
        {
            GetComponentInParent<StoryLineManager>().GoalUpdate();
            Goals[0].Increment(1);
            Complete();
        }
    }


}
