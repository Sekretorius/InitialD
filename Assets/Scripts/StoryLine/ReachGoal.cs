using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal : Goal
{

    public ReachGoal(string description, int current, int count)
    {
        this.description = description;
        this.current = current;
        this.count = count;
    }


}
