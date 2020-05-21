using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MovementControler
{
    public bool isHiden = false;
    private bool IsShooting = false;
    private bool IsHoldingGun = false;
    private GameObject saveManager;
    new void Start()
    {
        base.Start();
        canCrawlSlide = true;
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if(saveManager == null)
        {
            if (TryGetComponent(out Player data))
            {
                data.addSettings(speed, JumpTakeOffSpeed);
            }
        }
    }
    protected override void ComputeMovement()
    {
        if (canMove)
        {
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(KeyCode.S) && !IsSliding && move.x != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Slide") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Slide_standUp") && !IsCrawling && !IsClimbing && !isHolding) // 
            {
                IsSliding = true;
                IsCrawling = true;
                slideDirection = move.x > 0 ? 1 : -1;
                slideSpeedX = Mathf.Abs(rgbd.velocity.x) + slideSpeed;
            }
            if((Input.GetKey(KeyCode.S) && !IsSliding && move.x == 0) && !IsClimbing && !isHolding)
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
        }
    }
    protected override void ComputeAnimation()
    {
        if (IsHoldingGun)
        {
            anim.SetBool("IsHoldingGun", true);
        }
        else
        {
            anim.SetBool("IsHoldingGun", false);
        }
        if (!IsSliding && !IsCrawling)
        {
            Turn(move.x);
        }
        if (IsCrawling && !IsSliding && !IsClimbing)
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
        if (IsSliding && !IsClimbing)
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
        if (IsShooting && anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot_1"))
        {
            IsShooting = false;
            anim.SetBool("IsShooting", false);
        }
    }
    public void HoldingGun(bool isHolding)
    {
        IsHoldingGun = isHolding;
    }
    public bool Shoot()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot_1"))
        {
            anim.SetBool("IsShooting", true);
            IsShooting = true;
            return true;
        }
        return false;
    }
    public void FreezeMovement(bool isFrozen)
    {
        canMove = isFrozen;
    }
    public void Hide(bool hide)
    {
        isHiden = hide;
    }
    public void AddSettings(Player player)
    {
        speed = player.speed;
        JumpTakeOffSpeed = player.jumpSpeed;
        transform.position = player.playerPosition;
    }
}
