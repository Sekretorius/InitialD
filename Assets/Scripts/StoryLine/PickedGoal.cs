using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedGoal : Goal
{
    public PickedGoal(string description)
    {
        this.description = description;
    }

    public PickedGoal(string description, int current, int count)
    {
        this.description = description;
        this.current = current;
        this.count = count;
    }


}
