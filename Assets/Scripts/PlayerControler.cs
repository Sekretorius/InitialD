using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MovementControler
{
    public bool isHiden = false;
    private bool isShooting = false;
    protected override void ComputeMovement()
    {
        if (canMove)
        {
            move.x = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Crouch") && !IsSliding && move.x != 0)
            {
                IsSliding = true;
                slideDirection = move.x > 0 ? 1 : -1;
                slideSpeedX = rgbd.velocity.x + slideSpeedModifier * slideDirection;
            }
            if(Input.GetKey(KeyCode.S) && !isHolding)
            {
                IsCrawling = true;
            }
            if (!Input.GetKey(KeyCode.S) && IsCrawling && !IsBlockedByCeilling)
            {
                IsCrawling = false;
            }

            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button1))
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                isShooting = true;
            }
        }
    }
    protected override void ComputeAnimation()
    {
        if (!IsSliding && !IsCrawling)
        {
            Turn(move.x, 0);
        }
        if (IsCrawling && !IsSliding)
        {
            if (move.x == 0)
            {
                anim.SetBool("IsInCrawlPosition", true);
                anim.SetBool("IsCrawlling", false);
            }
            else
            {
                anim.SetBool("IsCrawlling", true);
            }
        }
        else
        {
            anim.SetBool("IsInCrawlPosition", false);
            anim.SetBool("IsCrawlling", false);
        }
        if (isHolding)
        {
            anim.SetBool("IsCarrying", true);
        }
        else
        {
            anim.SetBool("IsCarrying", false);
        }
        if (move.x != 0 && anim != null)
        {
            anim.SetBool("IsWalking", true);
        }
        else if (anim != null)
        {
            anim.SetBool("IsWalking", false);
        }
        if (IsSliding)
        {
            anim.SetBool("IsSliding", true);
        }
        else
        {
            anim.SetBool("IsSliding", false);
        }
        if(jump)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }
        if (isShooting)
        {
            anim.SetBool("IsShooting", true);
            isShooting = false;
        }
        else
        {
            anim.SetBool("IsShooting", false);
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
