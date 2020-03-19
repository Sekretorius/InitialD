using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MovementControler
{
    public bool isHiden = false;
    private bool canMove = true;
    protected override void ComputeMovement()
    {
        if (canMove)
        {
            move.x = Input.GetAxisRaw("Horizontal");

            if (Input.GetKey(KeyCode.Space))
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
        }
    }
    public void FreezeMovement()
    {
        canMove = canMove == true ? false : true;
    }
    public void Hide()
    {
        isHiden = isHiden == true ? false : true;
    }
}
