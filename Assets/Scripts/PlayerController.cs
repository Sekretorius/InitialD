using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectPhysics
{
    public Vector2 move = Vector2.zero;
    public float JumpTakeOffSpeed = 15;
    public float MaxSpeed = 7;
    public bool jump = false;
    private int direction = 0;
    public Animator Anim;

    public void Start()
    {
        //Anim = gameObject.GetComponent<Animator>();
    }
    protected override void ComputeVelocity()
    {
        move.y = 0;
        move.x = Input.GetAxisRaw("Horizontal");
        turn(move.x);

        if(move.x != 0)
        {
            Anim.SetBool("IsWalking", true);
        }
        else
        {
            Anim.SetBool("IsWalking", false);
        }

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
    private void turn(float dir)
    {
        if (dir > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (dir < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
