using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectPhysics
{
    public Vector2 move = Vector2.zero;
    public float JumpTakeOffSpeed = 15;
    public float MaxSpeed = 7;
    public bool jump = false;
    protected override void ComputeVelocity()
    {
        move.y = 0;
        move.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.Space))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        if (jump && grounded)
        {
            velocity.y = JumpTakeOffSpeed;
        }
        else if (!jump)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }
        targetVelocity = move * MaxSpeed;
    }
}
