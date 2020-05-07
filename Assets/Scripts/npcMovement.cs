using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class npcMovement : MovementControler
{
    public float right;
    public float left;

    private float timer;
    private float x;

    new void Start()
    {
        base.Start();
        right = transform.position.x + right;
        left = transform.position.x - left;
        timer = 3;
    }

    protected override void ComputeMovement()
    {       
        Idle();
        move.x = x;
    }
    protected override void ComputeAnimation()
    {
        Turn(x);
    }
    void Idle()
    {
        Random random = new Random();
        if (timer <= 0)
        {
            timer = random.Next(1, 6);
            x = random.Next(-1, 2);
        }
        else if (right <= transform.position.x )
            x = -1;
        else if (left >= transform.position.x)
            x = 1;
        timer -= Time.deltaTime;
    }
}
