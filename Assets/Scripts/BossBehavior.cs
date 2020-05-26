using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MovementControler
{
    private Transform target;
    private float normalSpeed;
    private bool foundTarget = false;
    private bool canJump = false;
    private bool isAttacking = false;
    private int attackPhase = -1;
    public float runningTimmerDmg = -1;
    private int runningAttackDir = 0;
    private bool isFollowing = false;
    private bool isNear = false;
    private float angleOnTimer = 0;
    private float minDistance = 2f;
    private float attackTimmer = 0;
    private float atRunningTimer = 0;
    private Vector2 rangeOfAttackTime = new Vector2(3, 10);
    public float time = 5f;
    public float rightBorder = 2f;
    public float leftBorder = 2f;

    private float turnTimer;
    private float moveTimer;
    private float moveDirection;
    private bool chasePlayer = false;

    new void Start()
    {
        normalSpeed = speed;
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
        SetTarget();
        isNear = false;
        Attack();
        if (canMove && !isAttacking)
        {
            if (target == null || (!isFollowing && !chasePlayer)) // kai nėra taikinio
            {
                if (move.x == 0)
                {
                    TurnAroundOnTimer();
                }
                MoveOnTimer();
            }
            if ((isFollowing || chasePlayer) && target != null) // sekimas taikinio
            {
                if (!(Vector2.Distance(transform.position, target.position) >= minDistance))
                {
                    isNear = true;
                }
                if (!isNear)
                {
                    MoveTowardsTarget(target.position);
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
    private void Attack()
    {
        anim.SetBool("StartedAttack", false);
        if (attackTimmer <= 0 && !isAttacking && isFollowing)
        {
            string phaseName = "Attack" + attackPhase;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(phaseName))
            {
                if (!(Vector2.Distance(transform.position, target.position) >= 4f))
                {
                    isNear = true;
                }
                if (!isNear)
                {
                    attackPhase = 0;
                }
                else
                {
                    attackPhase = (int)Random.Range(0, 2);
                }

                isAttacking = true;
                anim.SetBool("StartedAttack", true);
                anim.SetBool("IsAttacking", true);
                anim.SetInteger("AttackPhase", attackPhase);
                if (attackPhase == 0)
                {
                    runningTimmerDmg = 0.5f;
                    speed = 10;
                    atRunningTimer = Random.Range(2, 4);
                    runningAttackDir = target.position.x - transform.position.x < 0 ? -1 : 1;
                }
            }
        }
        if (isAttacking)
        {
            string phaseName = "Attack" + attackPhase;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(phaseName))
            {
                anim.SetBool("StartedAttack", true);
            }
            switch (attackPhase)
            {
                case 0:
                    RunningAttack(runningAttackDir);
                    break;
                case 1:
                    StompAttack();
                    break;
                case 2:
                    break;
            }
        }
        if(!isAttacking && attackTimmer > 0)
        {
            anim.SetBool("IsAttacking", false);
            attackTimmer -= Time.fixedDeltaTime;
        }
    }
    private void RunningAttack (int direction)
    {
        if (obsticle != null && IsBlocked)
        {
            if (!obsticle.tag.Equals("Player") && !obsticle.tag.Equals("Enemy") && !obsticle.tag.Equals("NPC"))
            {
                atRunningTimer = 0;
            }
        }
        if (atRunningTimer <= 0)
        {
            isAttacking = false;
            speed = normalSpeed;
            attackTimmer = Random.Range(rangeOfAttackTime.x, rangeOfAttackTime.y);
        }
        if(isAttacking)
        {
            move = new Vector2(direction, 0);
        }
        if(atRunningTimer > 0)
        {
            atRunningTimer -= Time.fixedDeltaTime;
        }
        if(runningTimmerDmg > 0)
        {
            runningTimmerDmg -= Time.fixedDeltaTime;
        }
    }
    private void StompAttack()
    {
        attackTimmer = Random.Range(rangeOfAttackTime.x, rangeOfAttackTime.y);
        isAttacking = false;
    }
    private void StompAttackKnockback()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 4);
        foreach (Collider2D col in results)
        {
            if (col.gameObject.TryGetComponent(out Rigidbody2D rig) && col.gameObject != gameObject)
            {
                int targetDirection = col.transform.position.x - transform.position.x < 0 ? -1 : 1;
                rig.AddForce(new Vector2(targetDirection * 1000f, 1000f));
            }
        }
        GameObject player = GameObject.Find("PlayerStats");
        if (player.TryGetComponent(out HealthSystem system))
        {
            system.Damage(2);
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
        if (!MoveTowardsTarget(targetDirection))
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
            if (directionCantMove != direction)
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
    new void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.tag.Equals("Player") && isAttacking)
        {
            if(attackPhase == 0 && runningTimmerDmg <= 0)
            {
                runningTimmerDmg = 0.5f;
                GameObject player = GameObject.Find("PlayerStats");
                if(player.TryGetComponent(out HealthSystem system))
                {
                    system.Damage(1);
                }
                collision.rigidbody.AddForce(new Vector2(runningAttackDir * 3000f, 0));
            }
        }
    }
    new void OnCollisionExit2D(Collision2D collision)
    {
    }
}
