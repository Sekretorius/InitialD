using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovementControler
{
    private Transform target { get; set; }
    public bool foundTarget = false;
    public bool canJump = false;
    public bool canMove = true;
    private Animator animator;
    private float timer;
    public float time = 5f;
    private float angleOnTimer = 0;
    private float minDistance = 2f;
    public void SetTarget(Transform targetTransform)
    {
        if(targetTransform != null)
        {
            foundTarget = true;
        }
        else
        {
            foundTarget = false;
        }
        target = targetTransform;
    }
    protected override void ComputeMovement()
    {
        if (target == null || !foundTarget)
        {
            TurnAroundOnTimer();
        }
        if (foundTarget && target != null)
        {
            if (Vector2.Distance(transform.position, target.position) >= minDistance)
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }

            MoveTowardsTarget(target);

            if (isBlocked && obsticle != null && canJumpOver)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
        }
    }
    void TurnAround(float angle)
    {
        transform.eulerAngles = new Vector3(0, angle, 0);
    }
    void TurnAroundOnTimer()
    {
        if (timer <= 0)
        {
            timer = time;
            angleOnTimer = angleOnTimer == 0 ? 180 : 0;
            TurnAround(angleOnTimer);
        }
        timer -= Time.deltaTime;
    }
    void MoveTowardsTarget(Transform target)
    {
        float diff = transform.position.x - target.position.x;
        float direction = diff > 0 ? -1 : 1;
        if (diff <= 0.2f && diff >= -0.2f)
        {
            direction = 0;
        }
        if (isBlocked && obsticle != null)
        {
            float blockingDirection = transform.position.x - obsticle.position.x;
            float directionCantMove = blockingDirection > 0 ? -1 : 1;
            if(directionCantMove == direction)
            {
                canMove = false;
            }
        }
        if (canMove)
        {
            move = new Vector2(direction, move.y);
        }
    }
}
