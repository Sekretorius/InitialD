using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase : Case
{
    public Collider2D player;
    public Collider2D destination;

    void Awake()
    {
        caseName = "HOME, SWEET HOME";
        id = 0;
        Goals = new ReachGoal[1];
        Goals[0] = new ReachGoal("LEAVE THE OFFICE",0,1);
        completed = false;
    }

    public override void OnEvent()
    {
        if (player.IsTouching(destination))
        {
            GetComponentInParent<StoryLineManager>().ShowOnScreen();
            Goals[0].Increment(1);
            Complete();
        }
    }

}
