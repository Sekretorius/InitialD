using System.Collections.Generic;
using UnityEngine;

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
    public bool ground = false;
    protected Vector3 velocity = Vector2.zero;
    protected bool jump = false;
    protected bool IsBlocked = false;
    protected Transform obsticle = null;
    protected bool canJumpOver = false;
    protected bool IsSliding = false;
    protected float slideDirection;
    protected float slideSpeedX;
    protected float minSlopeNomal = 0.9F;

    protected float slopeAngle = 0;
    protected Vector2 groundNormal = Vector2.zero;
    protected bool IsBlockedByCeilling = false;

    protected bool IsJumping = false;
    public bool IsCrawling = false;
    public bool isHolding = false;
    public bool canCarrie = true;
    public float inpickableTimmer = 0.15f;
    protected float timerToPick;
    protected Transform closestPickable;
    protected float distanceToPickable = -1;
    protected bool slope = false;
    protected Vector2 colliderDimensions;
    public float facingDirection;
    protected Vector2 normalToFace;
    protected float angle;
    protected float lostBalanceDirection = 0;

    protected bool canMove = true;

    protected bool nullifiedImput = false;

    public List<ContactPoint2D> collisions = new List<ContactPoint2D>();

    protected void Start()
    {
        timerToPick = inpickableTimmer;
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        colliderDimensions = boxCollider.bounds.size;
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
        if (move.x != 0)
        {
            facingDirection = move.x;
        }
        if (IsBlockedByCeilling && ground)
        {
            IsCrawling = true;
        }
        if (IsSliding && slideSpeedX != 0 && Input.GetAxisRaw("Horizontal") == slideDirection)
        {
            Vector2 slideVelocity = new Vector2(slideSpeedX, rgbd.velocity.y);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, slideVelocity, ref velocity, movementSmoothing);
            targetVelocity = new Vector2(rgbd.velocity.x, rgbd.velocity.y);
            float slopeDirection = groundNormal.x > 0 ? 1 : -1;

            if (slideDirection != slopeDirection || !slope)
            {
                slideSpeedX -= slideDrag * slideDirection - (rgbd.velocity.x * 0.001f);
            }
            else
            {
                slideSpeedX += 0.01f * slideDirection;
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
        if ((!IsSliding && !IsCrawling) || IsJumping)
        {
            Turn(facingDirection, 0);
            rgbd.freezeRotation = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
            targetVelocity = new Vector2(move.x * speed, rgbd.velocity.y);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);
        }
        else if (!IsJumping)
        {
            Turn(facingDirection, transform.eulerAngles.z);
            AlignWithGround();
        }
        if (jump && ground)
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

        if (!ground|| IsSliding)
        {
            rgbd.velocity += Physics2D.gravity * gravityModifier * Time.fixedDeltaTime;
        }
    }
    private void AlignWithGround()
    {
        LayerMask mask = ~LayerMask.GetMask("Player", "NPC", "Enemy");
        RaycastHit2D[] hitResults = new RaycastHit2D[15];
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(mask);
        boxCollider.Cast(-transform.up, contactFilter2D, hitResults, 2f);
        bool nearGround = false;
        Vector2 nearGroundNormal = Vector2.zero;
        foreach(RaycastHit2D result in hitResults)
        {
            if(result && result.collider.gameObject != gameObject)
            {
                if(result.normal.y > 0.25f)
                {
                    nearGround = true;
                    nearGroundNormal = result.normal;
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

            corner1 = RotatePointAroundPivot(corner1, Vector3.zero, bcTransform.eulerAngles);
            corner2 = RotatePointAroundPivot(corner2, Vector3.zero, bcTransform.eulerAngles);

            corner1 = worldPosition + corner1;
            corner2 = worldPosition + corner2;

            RaycastHit2D hitLeft = Physics2D.Raycast(corner1, -Vector2.up, 10f, mask);
            RaycastHit2D hitRight = Physics2D.Raycast(corner2, -Vector2.up, 10f, mask);
            ContactPoint2D[] points = new ContactPoint2D[15];
            int contactCount = boxCollider.GetContacts(points);
            bool canRotateByContacts = true;
            if (hitLeft && hitLeft.normal.y > 0.25f)
            {
                float distanceLeft = Vector2.Distance(corner1, hitLeft.point);
                if (distanceLeft > 2f)
                {
                    canRotateByContacts = false;
                }
            }
            if (hitRight && hitRight.normal.y > 0.25f)
            {
                float distanceRight = Vector2.Distance(corner2, hitRight.point);
                if (distanceRight > 2f)
                {
                    canRotateByContacts = false;
                }
            }
            if(hitRight && hitLeft && canRotateByContacts)
            {
                Vector2 targetVector = hitRight.point - hitLeft.point;
                Vector2 perpendicularVector = Vector2.Perpendicular(targetVector);
                transform.up = perpendicularVector;
            }
            if (contactCount == 1 && !canRotateByContacts)
            {
                transform.up = nearGroundNormal;
            }
        }
        else
        {
            rgbd.freezeRotation = false;
            float angle = Vector2.Angle(transform.up, Vector2.up);
            if(angle > 30)
            {
                transform.eulerAngles = new Vector3(0, 0, 30);
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

    private void CheckSlope()
    {
        RaycastHit2D[] resultsFromBack = new RaycastHit2D[15];
        Vector2 currentColliderDimensions = boxCollider.bounds.size;
        float deltaOffsetY = transform.position.y - currentColliderDimensions.y / 2 * 3;
        float distanceToTarget = Vector2.Distance(transform.position, new Vector2(transform.position.x, deltaOffsetY));
        boxCollider.Cast(-transform.up, resultsFromBack, distanceToTarget, true);
        slope = false;
        foreach (RaycastHit2D result in resultsFromBack)
        {
            if (ground && result)
            {
                if (result.collider.gameObject != gameObject)
                {
                    if (result.normal.y > minGroungNormalY && result.normal.y < minSlopeNomal)
                    {
                        slope = true;
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
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                groundNormal = contact.normal;
                ground = true;
                IsJumping = false;
            }
            else
            {
                IsBlocked = true;
                obsticle = collision.collider.transform;
                if (!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("NPC"))
                {
                    float obsticleHeight = 0;
                    float height = 0;
                    if (obsticle.TryGetComponent(out Collider2D obsticleCollider))
                    {
                        obsticleHeight = obsticleCollider.bounds.size.y;
                    }
                    if (TryGetComponent(out Collider2D collider))
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
        groundNormal = Vector2.zero;
        ground = false;
        IsBlocked = false;
        obsticle = null;
    }
    public void Turn(float dir, float angle)
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
    }
}
