using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovementControler
{
    public Transform target;
    public Transform firePoint;
    private bool foundTarget = false;
    private bool canJump = false;
    private bool isFollowing = false;
    private bool isNear = false;
    private float angleOnTimer = 0;
    private float minDistance = 2f;

    public float time = 5f;
    public float rightBorder = 2f;
    public float leftBorder = 2f;

    private float turnTimer;
    private float moveTimer;
    private float moveDirection;
    private bool chasePlayer = false;
    public GameObject bulletPrefab;


    new void Start()
    {
        base.Start();
        rightBorder = transform.position.x + rightBorder;
        leftBorder = transform.position.x - leftBorder;
        moveTimer = time;
        turnTimer = time;
    }

    public void SetTarget()
    {
        if (targetTransform != null)
        {
            target = targetTransform;
            isFollowing = true;
            chasePlayer = isChasing;
        }
        else
        {
            isFollowing = false;
        }
    }

    protected override void ComputeMovement()
    {
        isNear = false;
        SetTarget();
        if (canMove)
        {
            if (target == null || (!isFollowing && !chasePlayer)) // kai nėra taikinio
            {
                if (move.x == 0)
                {
                    TurnAroundOnTimer();
                }
                MoveOnTimer();
            }
            if ((isFollowing || chasePlayer) && target != null)  // sekimas taikinio
            {
                if (!(Vector2.Distance(transform.position, target.position) >= minDistance))
                {
                    isNear = true;
                }
                if (!isNear)
                {
                    MoveTowardsTarget(target.position);


                }
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                if (Random.Range(0f, 1f) > 0.98f)
                {
                    if (target.position.x - transform.position.x > 0)
                    {
                        firePoint = transform.Find("ShootPointRight");
                    }
                    else
                    {
                        firePoint = transform.Find("ShootPointLeft");
                    }
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                }
                   
            }
            if (IsBlocked && obsticle != null && canJumpOver) // kliūties peršokimas
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
        if (isFollowing)
        {
            anim.SetBool("IsHoldingGun", true);
        }
        else
        {
            anim.SetBool("IsHoldingGun", false);
        }
        if (move.x != 0 && anim != null)
        {
            anim.SetBool("IsWalking", true);
        }
        else if (anim != null)
        {
            anim.SetBool("IsWalking", false);
        }
    }
    void TurnAround(float angle)
    {
        transform.eulerAngles = new Vector3(0, angle, 0);
    }
    void MoveOnTimer()
    {
        if (moveTimer <= 0)
        {
            moveTimer = Random.Range(1, 6);
            moveDirection = Random.Range(-1, 2);
        }
        else if (rightBorder <= transform.position.x)
            moveDirection = -1;
        else if (leftBorder >= transform.position.x)
            moveDirection = 1;
        moveTimer -= Time.deltaTime;
        Vector2 targetDirection = new Vector2(transform.position.x + moveDirection, transform.position.y);
        if(!MoveTowardsTarget(targetDirection))
        {
            moveDirection = 0;
        }
    }
    void TurnAroundOnTimer()
    {
        if (turnTimer <= 0)
        {
            turnTimer = time;
            angleOnTimer = angleOnTimer == 0 ? 180 : 0;
            TurnAround(angleOnTimer);
        }
        turnTimer -= Time.deltaTime;
    }
    bool MoveTowardsTarget(Vector2 target)
    {
        float diff = transform.position.x - target.x;
        float direction = diff > 0 ? -1 : 1;
        if (diff <= 0.2f && diff >= -0.2f)
        {
            direction = 0;
        }
        if (IsBlocked && obsticle != null && !canJumpOver)
        {
            float blockingDirection = transform.position.x - obsticle.position.x;
            float directionCantMove = blockingDirection > 0 ? -1 : 1;
            if(directionCantMove != direction)
            {
                IsBlocked = false;
            }
        }
        if (!IsBlocked || canJumpOver)
        {
            move = new Vector2(direction, move.y);
            return true;
        }
        return false;
    }
}
