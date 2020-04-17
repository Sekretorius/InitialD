using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class MovementControler : MonoBehaviour
{
    public float speed = 10f;
    public float crawlSpeed = 7F;
    public float JumpTakeOffSpeed = 30f;
    public float movementSmoothing = .05f;
    public float gravityModifier = 6f;
    public float slideSpeedModifier = 2f; 
    public float minSlideSpeed = 5f;
    public float slideDrag = 0.15f;

    protected Animator anim;
    protected BoxCollider2D boxCollider;
    protected float minGroungNormalY = 0.65f;
    protected Vector2 targetVelocity;
    protected Vector2 move;
    protected Rigidbody2D rgbd;
    protected bool ground = false;
    protected Vector3 velocity = Vector2.zero;
    protected bool jump = false;
    protected bool IsBlocked = false;
    protected Transform obsticle = null;
    protected bool canJumpOver = false;
    protected bool IsSliding = false;
    protected float slideDirection;
    protected float slideSpeedX;
    protected float minSlopeNomal = 0.95F;

    protected float slopeAngle = 0;
    public Vector2 slopeNormal = Vector2.zero;
    protected bool IsBlockedByCeilling = false;

    protected bool IsJumping = false;
    protected bool IsCrawling = false;
    public bool isHolding = false;
    public bool canCarrie = true;
    public float inpickableTimmer = 0.15f;
    protected float timerToPick;
    protected Transform closestPickable;
    protected float distanceToPickable = -1;
    protected bool slope = false;
    protected Vector2 colliderDimensions;
    public Vector2 facingNormal;

    protected bool canMove = true;

    protected bool nullifiedImput = false;

    protected void Start()
    {
        timerToPick = inpickableTimmer;
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        colliderDimensions = boxCollider.bounds.size;
        facingNormal = transform.up;
    }
    private void FixedUpdate()
    {
        if (rgbd != null)
        {
            if (!nullifiedImput)
            {
                IsBlockedByCeilling = CheckCeiling();
                CheckSlope();
                ComputeMovement();
                Movement();
                ComputeAnimation();
            }
        }
        else
        {
            if (TryGetComponent(out Rigidbody2D rig))
            {
                rgbd = rig;
            }
        }
        CanHold();
        move = Vector2.zero;
        ground = false;
    }
    abstract protected void ComputeMovement();
    abstract protected void ComputeAnimation();

    public void Movement()
    {
        if (IsBlockedByCeilling && ground)
        {
            IsCrawling = true;
        }
        if (IsSliding && slideSpeedX != 0 && Input.GetAxisRaw("Horizontal") == slideDirection)
        {
            Vector2 slideVelocity = new Vector2(slideSpeedX, rgbd.velocity.y);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, slideVelocity, ref velocity, movementSmoothing);
            targetVelocity = new Vector2(rgbd.velocity.x, rgbd.velocity.y);
            float slopeDirection = slopeNormal.x > 0 ? 1 : -1;
            
            if (slideDirection != slopeDirection || !slope)
            {
                slideSpeedX -= slideDrag * slideDirection;
            }
            if (Mathf.Abs(slideSpeedX) <= minSlideSpeed || IsBlocked)
            {
                slideSpeedX = 0;
            }
        }
        else
        {
            IsSliding = false;
        }
        if (IsCrawling)
        {
            targetVelocity = new Vector2(move.x * crawlSpeed, rgbd.velocity.y);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
        }
        if (!IsSliding && !IsCrawling)
        {
            Turn(move.x, 0);
            targetVelocity = new Vector2(move.x * speed, rgbd.velocity.y);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
        }
        else
        {
            AlignWithGround();
        }
        if (jump && ground)
        {
            IsJumping = true;
            targetVelocity = new Vector2(rgbd.velocity.x, JumpTakeOffSpeed);
        }
        else if (!jump)
        {
            IsJumping = false;
            if (rgbd.velocity.y > 0)
            {
                targetVelocity = new Vector2(rgbd.velocity.x, rgbd.velocity.y * .5f);
            }
        }
        rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (!ground)
        {
            rgbd.velocity += Physics2D.gravity * gravityModifier * Time.fixedDeltaTime;
        }
    }
    private void CheckSlope()
    {
        RaycastHit2D[] resultsFromBack = new RaycastHit2D[15];
        Vector2 currentColliderDimensions = boxCollider.bounds.size;
        float deltaOffsetY = transform.position.y - currentColliderDimensions.y / 2 * 3;
        float distanceToTarget = Vector2.Distance(transform.position, new Vector2(transform.position.x, deltaOffsetY));
        boxCollider.Cast(-transform.up, resultsFromBack, distanceToTarget);
        foreach (RaycastHit2D result in resultsFromBack)
        {
            if (ground && result)
            {
                if (result.collider.gameObject != gameObject)
                {
                    if (result.normal.y > 0.65)
                    {
                        slope = true;
                        slopeNormal = result.normal;
                        slopeAngle = result.collider.transform.eulerAngles.z;
                        return;
                    }
                }
            }
            slopeNormal = Vector2.zero;
            slope = false;
        }
    }
    private void AlignWithGround()
    {
        facingNormal = slopeNormal;
        transform.up = Vector2.Lerp(transform.up, slopeNormal, 0.7f);
        if (move.x != 0)
        {
            Turn(move.x, transform.eulerAngles.z * move.x);
        }
    }
    public bool CheckCeiling()
    {
        float deltaOffsetY = boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2;
        Vector2 castOrigin = new Vector2(transform.position.x, deltaOffsetY);
        Vector2 fixedDimensions = new Vector2(colliderDimensions.x, colliderDimensions.x);
        LayerMask mask = ~LayerMask.GetMask("Player", "PickableObject");
        RaycastHit2D[] results = Physics2D.BoxCastAll(castOrigin, fixedDimensions, 0, Vector2.up, colliderDimensions.y + 0.5f, mask);
        foreach (RaycastHit2D result in results)
        {
            if (result && result.collider.gameObject != gameObject)
            {
                if (result.normal.y < 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                ground = true;
            }
            else if(!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("NPC"))
            {
                IsBlocked = true;
                obsticle = collision.collider.transform;
            }
            else
            {
                slideSpeedX = 0;
            }
            if (collision.collider.CompareTag("Player") && gameObject.CompareTag("NPC_Ignored"))
            {
                IsBlocked = false;
                Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
            }

        }
    }
    private void CanHold()
    {
        if (IsSliding || IsCrawling)
        {
            canCarrie = false;
        }
        else
        {
            if (timerToPick <= 0)
            {
                canCarrie = true;
                timerToPick = inpickableTimmer;
            }
            if (!canCarrie)
            {
                timerToPick -= Time.fixedDeltaTime;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                ground = true;
                IsJumping = false;
            }
            else
            {
                IsBlocked = true;
                obsticle = collision.collider.transform;
                if(!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("NPC"))
                {
                    float obsticleHeight = 0;
                    float height = 0;
                    if(obsticle.TryGetComponent(out Collider2D obsticleCollider))
                    {
                        obsticleHeight = obsticleCollider.bounds.size.y;
                    }
                    if(TryGetComponent(out Collider2D collider))
                    {
                        height = collider.bounds.size.y;
                    }
                    float cordY = transform.position.y - height / 2;
                    float colCordY = collision.transform.position.y + obsticleHeight / 2;
                    float diff = colCordY - cordY;
                    if (diff <= height)
                    {
                        canJumpOver = true;
                    }
                    else
                    {
                        canJumpOver = false;
                    }
                }
                else
                {
                    canJumpOver = false;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        ground = false;
        IsBlocked = false;
        obsticle = null;
    }
    protected void Turn(float dir, float angle)
    {
        if (dir > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else if (dir < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, angle);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, angle);
        }
    }
    public void SetGroundState(bool grounded)
    {
        ground = grounded; 
    }
    public void NullifyMovement(bool state)
    {
        nullifiedImput = state;
    }
    public bool PickableClosestToPlayer(Transform pos)
    {
        if(pos == closestPickable)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PickObject script) && gameObject.CompareTag("Player"))
        {
            bool reachable = false;
            Vector2 targetDirection = collision.transform.position - transform.position;
            float distanceToTarget = Vector2.Distance(collision.transform.position, transform.position);
            Vector2 colliderDimensions1 = script.colliderDimensions;
            LayerMask mask = ~LayerMask.GetMask("Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, distanceToTarget, mask);
            if (hit)
            {
                if(hit.collider.gameObject == collision.gameObject)
                {
                    bool cantPick = PickObject.CheckCeiling(GetComponent<BoxCollider2D>(), collision.gameObject,colliderDimensions1, 0.3f);
                    if (!cantPick)
                    {
                        reachable = true;
                    }
                }
            }
            if (reachable)
            {
                float distance = Vector2.Distance(collision.transform.position, transform.position);
                if (closestPickable == null)
                {
                    closestPickable = collision.transform;
                    distanceToPickable = distance;
                }
                else
                {
                    Vector2 colliderDimensions2 = closestPickable.GetComponent<PickObject>().colliderDimensions;
                    bool cantPick = PickObject.CheckCeiling(GetComponent<BoxCollider2D>(), closestPickable.gameObject, colliderDimensions2, 0.3f);
                    if (cantPick)
                    {
                        closestPickable = collision.transform;
                        distanceToPickable = distance;
                    }
                    else if(distanceToPickable > distance && !isHolding)
                    {
                        closestPickable = collision.transform;
                        distanceToPickable = distance;
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == closestPickable)
        {
            closestPickable = null;
            distanceToPickable = -1;
        }
    }
}
