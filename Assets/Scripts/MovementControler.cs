using System.Collections.Generic;
using UnityEngine;

public abstract class MovementControler : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float crawlSpeed = 5f;
    [SerializeField] protected float JumpTakeOffSpeed = 60f;
    [SerializeField] protected float movementSmoothing = 0.05f;
    [SerializeField] protected float gravityModifier = 6f;
    [SerializeField] protected float slideSpeed = 8f;
    [SerializeField] protected float minSlideSpeed = 5f;
    [SerializeField] protected float slideDragForce = 15f;

    protected Animator anim;
    protected Rigidbody2D rgbd;
    protected BoxCollider2D boxCollider;
    protected Vector2 colliderDimensions;
    protected Vector2 targetVelocity;
    protected Vector2 move;
    protected Vector2 groundNormal = Vector2.zero;
    protected Vector3 velocity = Vector2.zero;

    protected float minGroungNormalY = 0.65f;
    public float facingDirection;

    protected bool nullifiedImput = false;
    protected bool canMove = true;
    protected bool ground = false;
    protected bool jump = false;
    protected bool IsBlocked = false;
    protected bool hasCollided = false;
    protected Transform obsticle = null;
    protected bool canJumpOver = false;
    
    protected bool IsSliding = false;
    protected bool IsJumping = false;
    public bool IsCrawling = false;
    public bool isHolding = false;

    protected bool slope = false;
    protected float slopeAngle = 0;
    protected float slideDirection;
    protected float slideSpeedX;
    protected float minSlopeNomal = 0.25F;
    protected Vector2 slopeNormal = Vector2.zero;

    protected bool IsBlockedByCeilling = false;
    public bool canCarrie = true;
    public float inpickableTimmer = 0.15f;
    protected float timerToPick;
    protected Transform closestPickable;
    protected float distanceToPickable = -1;

    public bool IsOnLadder = false;
    public bool IsClimbing = false;
    public bool IsOnTopOfLadder = false;
    protected PlatformEffector2D ladderEffector = null;
    protected bool canCrawlSlide = false;

    protected float MaxJumpHeight = 0;

    protected void Start()
    {
        timerToPick = inpickableTimmer;
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        colliderDimensions = boxCollider.bounds.size;
    }

    protected void FixedUpdate()
    {
        if (rgbd != null)
        {
            if (!nullifiedImput)
            {
                IsBlockedByCeilling = CheckCeiling();
                CheckSlope();
                ComputeMovement();
                ClimbLadder(move);
                if (!IsClimbing)
                {
                    Movement();
                }
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

    private void ClimbLadder(Vector2 direction)
    {
        float climbSpeed = 10f;
        float climbSidewaysSpeed = 8f;
        if (!IsOnLadder || ground)
        {
            IsClimbing = false;
            rgbd.gravityScale = 1f;
        }
        if(IsClimbing && direction.y < 0)
        {
            ground = false;
        }
        if (IsOnTopOfLadder)
        {
            if(ladderEffector != null && direction.y < 0)
            {
                ladderEffector.surfaceArc = 0;
                IsClimbing = true;
            }
            else if (ladderEffector != null)
            {
                ladderEffector.surfaceArc = 180;
            }
            if (jump && !IsJumping)
            {
                IsJumping = true;
                targetVelocity = new Vector2(rgbd.velocity.x, JumpTakeOffSpeed);
                rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
                IsClimbing = false;
                return;
            }
        }
        if (IsOnLadder || IsOnTopOfLadder)
        {
            if (direction.y > 0)
            {
                IsClimbing = true;
                targetVelocity = new Vector2(0, climbSpeed);
            }
            else if (direction.y < 0 && !ground)
            {
                IsClimbing = true;
                targetVelocity = new Vector2(0, -climbSpeed);
            }
            else if (IsClimbing && !ground)
            {
                rgbd.velocity = Vector2.zero;
                targetVelocity = Vector2.zero;
            }
            if (direction.x != 0 && IsClimbing)
            {
                targetVelocity = new Vector2(climbSidewaysSpeed * direction.x, targetVelocity.y);
                Turn(direction.x);
            }
            if (IsClimbing || !ground)
            {
                rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
            }
        }
        if((IsOnLadder && !ground) || IsOnTopOfLadder)
        {
            rgbd.freezeRotation = true;
            transform.eulerAngles = Vector3.zero;
            rgbd.gravityScale = 0;
        }

    }
    public void Movement()
    {
        if (move.x != 0)
        {
            facingDirection = move.x;
        }
        if (canCrawlSlide)
        {
            if (IsBlockedByCeilling && ground)
            {
                IsCrawling = true;
            }
            if (IsSliding && slideSpeedX != 0 && Input.GetAxisRaw("Horizontal") == slideDirection)
            {
                Vector2 slideVelocity = new Vector2(slideSpeedX * slideDirection, rgbd.velocity.y);
                rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, slideVelocity, ref velocity, movementSmoothing);
                targetVelocity = new Vector2(rgbd.velocity.x, rgbd.velocity.y);
                float slopeDirection = slopeNormal.x > 0 ? 1 : -1;
                if (slideDirection != slopeDirection || !slope)
                {
                    slideSpeedX -= slideDragForce * Time.fixedDeltaTime;
                }
                else
                {
                    slideSpeedX += 0.01f;
                }
                if (hasCollided || slideSpeedX <= 1f)
                {
                    slideSpeedX = 0;
                }
            }
            else
            {
                IsSliding = false;
            }
            if (IsCrawling && !IsSliding)
            {
                targetVelocity = new Vector2(move.x * crawlSpeed, rgbd.velocity.y);
                rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
            }
            if ((!IsSliding && !IsCrawling) || IsJumping)
            {
                Turn(facingDirection);
                rgbd.freezeRotation = true;
                transform.eulerAngles = new Vector3(0, 0, 0);
                targetVelocity = new Vector2(move.x * speed, rgbd.velocity.y);
                rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
            }
            else if (!IsJumping)
            {
                Turn(facingDirection);
                AlignWithGround();
            }
        }
            if (jump && ground && !IsJumping)
            {
                IsJumping = true;
                if (!IsCrawling && !IsSliding)
                {
                    targetVelocity = new Vector2(rgbd.velocity.x, JumpTakeOffSpeed);
                }
                else if (IsCrawling || IsSliding)
                {
                    targetVelocity = new Vector2(rgbd.velocity.x, JumpTakeOffSpeed / 1.5f);
                }
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
    private void AlignWithGround()
    {
        if(transform.up.y < 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        LayerMask mask = ~LayerMask.GetMask("Player", "NPC", "Enemy");
        RaycastHit2D[] hitResults = new RaycastHit2D[15];
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(mask);
        boxCollider.Cast(-Vector2.up, contactFilter2D, hitResults, 2f);
        bool nearGround = false;
        Vector2 nearGroundNormal = Vector2.zero;
        foreach (RaycastHit2D result in hitResults)
        {
            if (result && result.collider.gameObject != gameObject)
            {
                if (result.normal.y > 0.25f)
                {
                    nearGroundNormal = result.normal;
                    nearGround = true;
                    break;
                }
            }
        }
        if (nearGround)
        {
            Transform bcTransform = boxCollider.transform;

            Vector3 worldPosition = bcTransform.TransformPoint(0, 0, 0);

            Vector2 size = boxCollider.bounds.size / 2;

            Vector3 corner1 = new Vector2(-size.x, -size.y);
            Vector3 corner2 = new Vector2(size.x, -size.y);
            Vector3 corner3 = new Vector2(0, -size.y);

            corner1 = RotatePointAroundPivot(corner1, Vector3.zero, bcTransform.eulerAngles);
            corner2 = RotatePointAroundPivot(corner2, Vector3.zero, bcTransform.eulerAngles);
            corner3 = RotatePointAroundPivot(corner2, Vector3.zero, bcTransform.eulerAngles);

            corner1 = worldPosition + corner1;
            corner2 = worldPosition + corner2;
            corner3 = worldPosition + corner3;

            RaycastHit2D hitLeft = Physics2D.Raycast(corner1, -Vector2.up, 10f, mask);
            RaycastHit2D hitRight = Physics2D.Raycast(corner2, -Vector2.up, 10f, mask);
            RaycastHit2D hitCenter = Physics2D.Raycast(corner3, -Vector2.up, 10f, mask);

            ContactPoint2D[] points = new ContactPoint2D[15];
            int contactCount = boxCollider.GetContacts(points);
            bool canRotateByContacts = true;
            float distanceLeft = Vector2.Distance(corner1, hitLeft.point);
            float distanceRight = Vector2.Distance(corner2, hitRight.point);
            rgbd.freezeRotation = false;
            if (!hitLeft && !hitCenter && facingDirection == -1)
            {
                rgbd.AddForceAtPosition(-Vector2.up * 10, corner1);
            }
            if (!hitRight && !hitCenter && facingDirection == 1)
            {
                rgbd.AddForceAtPosition(-Vector2.up * 10, corner2);
            }
            if (hitLeft && hitLeft.normal.y > 0.25f)
            {
                if (distanceLeft > 2f)
                {
                    canRotateByContacts = false;
                }
            }
            if (hitRight && hitRight.normal.y > 0.25f)
            {
                if (distanceRight > 2f)
                {
                    canRotateByContacts = false;
                }
            }
            rgbd.freezeRotation = true;

            if (hitRight && hitLeft && canRotateByContacts)
            {
                if (distanceLeft >= distanceRight)
                {
                    rgbd.AddForceAtPosition(-Vector2.up * 50, corner1);
                }
                else
                {
                    rgbd.AddForceAtPosition(-Vector2.up * 50, corner2);
                }
                rgbd.freezeRotation = false;
            }
            float angle = Vector2.Angle(transform.up, Vector2.up);
            if (!canRotateByContacts && angle > 60)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            float angle = Vector2.Angle(transform.up, Vector2.up);
            rgbd.gravityScale = 1f;
            if (angle > 30)
            {
                rgbd.freezeRotation = true;
            }
        }
    }
    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir; 
        point = dir + pivot;
        return point; 
    }

    private void CheckSlope() // taisyti
    {
        RaycastHit2D[] resultsFromBack = new RaycastHit2D[15];
        Vector2 currentColliderDimensions = boxCollider.bounds.size;
        float deltaOffsetY = transform.position.y - currentColliderDimensions.y / 2 * 3;
        boxCollider.Cast(-transform.up, resultsFromBack, currentColliderDimensions.y, true);
        slope = false;
        foreach (RaycastHit2D result in resultsFromBack)
        {
            if (result)
            {
                if (result.collider.gameObject != gameObject)
                {
                    if (result.normal.y > minSlopeNomal && result.normal.y < 0.9f) // && result.normal.y < minSlopeNomal
                    {
                       
                        slope = true;
                        slopeNormal = result.normal;
                        return;
                    }
                    else
                    {
                        slope = false;
                        slopeNormal = Vector2.zero;
                    }
                }
            }
        } 
    }
    public bool CheckCeiling()
    {
        float deltaOffsetY = boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2;
        Vector2 castOrigin = new Vector2(transform.position.x, deltaOffsetY);
        Vector2 fixedDimensions = new Vector2(colliderDimensions.x, colliderDimensions.x);
        LayerMask mask = ~LayerMask.GetMask("Player", "PickableObject", "NPC", "Enemy");
        RaycastHit2D[] results = Physics2D.BoxCastAll(castOrigin, fixedDimensions, 0, Vector2.up, colliderDimensions.y + 0.3f, mask);
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
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            if (collision.gameObject.TryGetComponent(out PlatformEffector2D effector))
            {
                ladderEffector = effector;
                IsOnTopOfLadder = true;
                return;
            }
        }
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                groundNormal = contact.normal;
                ground = true;
                IsJumping = false;
            }
            else if (!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("NPC"))
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
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            if(collision.gameObject.TryGetComponent(out PlatformEffector2D effector))
            {
                ladderEffector = effector;
                IsOnTopOfLadder = true;
                return;
            }
        }
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if(contact.normal.y < minSlopeNomal)
            {
                hasCollided = true;
            }
            if (contact.normal.y > minGroungNormalY)
            { 
                groundNormal = contact.normal;
                ground = true;
            }
            else
            {
                IsBlocked = true;
                obsticle = collision.collider.transform;
                if (!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("NPC") && !collision.collider.CompareTag("Enemy"))
                {
                    
                    float obsticleHeight = 0;
                    float height = 0;
                    float colCordY = 0;
                    if (obsticle.TryGetComponent(out Collider2D obsticleCollider))
                    {
                        obsticleHeight = obsticleCollider.bounds.size.y;
                        colCordY = obsticleCollider.bounds.center.y + obsticleHeight / 2;
                        if(obsticleCollider is BoxCollider2D)
                        {
                            colCordY += ((BoxCollider2D)obsticleCollider).edgeRadius;
                        }
                    }
                    if (TryGetComponent(out Collider2D collider))
                    {
                        height = boxCollider.bounds.size.y;
                    }
                    float cordY = boxCollider.bounds.center.y - height / 2 - boxCollider.edgeRadius + height / 1.5f;
                    LayerMask mask = ~LayerMask.GetMask("Enemy");
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCollider.bounds.center.x, cordY), new Vector2(facingDirection, 0), boxCollider.bounds.size.x, mask);
                    Debug.DrawRay(new Vector2(boxCollider.bounds.center.x, cordY), new Vector2(facingDirection, 0)* boxCollider.bounds.size.x, Color.red);
                    if (!hit)
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
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            if (collision.gameObject.TryGetComponent(out PlatformEffector2D effector))
            {
                ladderEffector = effector;
                IsOnTopOfLadder = false;
            }
        }
        groundNormal = Vector2.zero;
        ground = false;
        IsBlocked = false;
        obsticle = null;
        hasCollided = false;
    }
    public void Turn(float dir)
    {
        if (TryGetComponent(out SpriteRenderer render))
        {
            if (dir != 0)
            {
                facingDirection = dir;
                if (dir > 0)
                {

                    render.flipX = false;
                }
                else if (dir < 0)
                {
                    render.flipX = true;
                }
            }
        }
        if (!gameObject.tag.Equals("Player"))
        {
            if (dir != 0)
            {
                facingDirection = dir;
                if (dir > 0)
                {

                    GetComponentInChildren<SpriteRenderer>().flipX = false;
                }
                else if (dir < 0)
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
            }
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
        if (pos == closestPickable)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Ladder"))
        {
            if (collision.gameObject.TryGetComponent(out PlatformEffector2D effector))
            {
                ladderEffector = effector;
                ladderEffector.surfaceArc = 0;
            }
            IsOnLadder = true;
        }
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
                if (hit.collider.gameObject == collision.gameObject)
                {
                    bool cantPick = PickObject.CheckCeiling(GetComponent<BoxCollider2D>(), collision.gameObject, colliderDimensions1, 0.3f);
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
                    else if (distanceToPickable > distance && !isHolding)
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
        if (collision.tag.Equals("Ladder"))
        {
            if (collision.gameObject.TryGetComponent(out PlatformEffector2D effector))
            {
                effector.surfaceArc = 180;
            }
            ladderEffector = null;
            IsOnLadder = false;
        }
    }
}
