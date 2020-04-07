using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MovementControler
{
    public bool isHiden = false;
    protected override void ComputeMovement()
    {
        if (canMove)
        {
            move.x = Input.GetAxisRaw("Horizontal");

            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button1))
            {
                jump = true;
            }
            else
            {
                jump = false;
            }   
        }
    }
    protected override void ComputeAnimation()
    {
        Turn(move.x);
        if (move.x != 0 && anim != null)
        {
            anim.SetBool("IsWalking", true);
        }
        else if (anim != null)
        {
            anim.SetBool("IsWalking", false);
        }
    }
    public void FreezeMovement(bool isFrozen)
    {
        canMove = isFrozen;
    }
    public void Hide(bool hide)
    {
        isHiden = hide;
    }
}
