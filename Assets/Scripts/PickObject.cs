using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickObject : Interactable
{
    public float throwSpeed = 10f;
    public float throwUpSpeed = 20f;
    public float throwModifier = 10f;
    public float spiningForce = 50f;
    public Vector2 colliderDimensions;

    private Rigidbody2D rgbd;
    private new Collider2D collider;
    private Vector3 velocity = Vector3.zero;
    private SpriteRenderer sprite;
    private PlayerControler playerControler;
    private BoxCollider2D interactibleCollider;
    private string ignoreWithTag = "";
    private float offsetY = 0.38f;
    private float additionalForce = 0;
    private float ThrowDirection = 0;
    private float minGroungNormalY = 0.65f;
    private float spriteWidth;
    private float spriteHeight;
    private bool isThrowing = false;
    private bool isBeingCarried = false;
    private bool ground = false;
    private bool collidedHorizontaly = false;
    private bool isBellow = false;
    private bool isBlocked = false;

    protected new void Start()
    {
        base.Start();
        collider = GetComponent<Collider2D>();
        if (CompareTag("NPC"))
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        spriteWidth = sprite.bounds.size.x / 2;
        spriteHeight = sprite.bounds.size.y / 2;
        colliderDimensions = new Vector2(collider.bounds.size.x, collider.bounds.size.y);
        rgbd = GetComponent<Rigidbody2D>();
    }
    protected override void OnEvent()
    {
        if (interactingObject != null)
        {
            playerControler = interactingObject.GetComponent<PlayerControler>();
            interactibleCollider = interactingObject.GetComponent<BoxCollider2D>();
            if (isBeingCarried)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button2) || !Input.GetKey(KeyCode.LeftShift))
                {
                    Dismount();
                    ThrowDirection = interactingObject.right.x;
                    ignoreWithTag = interactingObject.gameObject.tag;
                    additionalForce += interactingObject.GetComponent<Rigidbody2D>().velocity.x;
                    Vector2 targetVelocity = new Vector2(rgbd.velocity.x, throwUpSpeed);
                    rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, .05f);
                    rgbd.AddTorque(spiningForce * -interactingObject.right.x);
                }
                else
                {
                    isBlocked = CheckCeiling(interactibleCollider, gameObject, colliderDimensions, offsetY);
                    if (isBlocked)
                    {
                        Dismount();
                        isThrowing = false;
                    }
                }
            }
            bool isPickable = Reach();
            
            if (isPickable && isBellow)
            {
                isPickable = Input.GetButton("Crouch");
            }
            if (isPickable && (Input.GetButtonDown("Pickup") && interactingObject != null))
            {
                additionalForce = 0;
                if (isPickable && !isBeingCarried)
                {
                    isBlocked = CheckCeiling(interactibleCollider, gameObject, colliderDimensions, offsetY);
                    if (!isBlocked)
                    {
                        float objectDeltaY = interactingObject.position.y + interactibleCollider.bounds.size.y / 2;
                        float objectOffsetDeltaY = objectDeltaY + spriteHeight + offsetY;
                        BecomeCaried(objectOffsetDeltaY);
                    }
                }
            }
        }
        if (isThrowing && !ground)
        {
            Throw();
        }
        if (ground || isBeingCarried)
        {
            Vector2 targetVelocity = new Vector2(0, 0);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, .05f);
        }
        if (!ground && !isBeingCarried)
        {
            rgbd.velocity += Physics2D.gravity * 6 * Time.fixedDeltaTime;
        }
        ground = false;
    }
    public static bool CheckCeiling(Collider2D fromObject, GameObject objectToCheck, Vector2 colliderDimensions, float offset)
    {
        float objectDeltaY = fromObject.transform.position.y + fromObject.bounds.size.y / 2;
        float objectOffsetDeltaY = objectDeltaY + colliderDimensions.y / 2 + offset;
        float offsetDistance = objectOffsetDeltaY - fromObject.transform.position.y;
        Vector2 newColliderDimensions = new Vector2(colliderDimensions.x - 0.2f, colliderDimensions.y);
        Vector2 point = new Vector2(fromObject.transform.position.x, objectOffsetDeltaY);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(point, newColliderDimensions, 0);

        bool isBlocking = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != objectToCheck && collider.gameObject != fromObject.gameObject)
            {
                isBlocking = true;
                break;
            }
        }
        if (!isBlocking)
        {
            bool isInBetween = false;
            RaycastHit2D[] hits = Physics2D.RaycastAll(fromObject.transform.position, Vector2.up, offsetDistance);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit)
                {
                    if (hit.collider.gameObject != objectToCheck && hit.collider.gameObject != fromObject.gameObject)
                    {
                        isInBetween = true;
                        break;
                    }
                }
            }
            return isInBetween;
        }
        return true;
    }
    private void Dismount()
    {
        rgbd.isKinematic = false;
        transform.parent = null;
        isThrowing = true;
        isBeingCarried = false;
        playerControler.isHolding = false;
        playerControler.canCarrie = false;
        Physics2D.IgnoreCollision(collider, interactibleCollider, false);
    }
    private void BecomeCaried(float positionY)
    {
        transform.parent = interactingObject;
        rgbd.isKinematic = true;
        rgbd.AddTorque(0);
        if (TryGetComponent(out MovementControler controller))
        {
            controller.NullifyMovement(true);
        }
        collidedHorizontaly = false;
        isThrowing = false;
        isBeingCarried = true;
        playerControler.isHolding = true;
        transform.position = new Vector2(interactingObject.position.x, positionY);
        transform.eulerAngles = new Vector3(0, 0, 0);
        Physics2D.IgnoreCollision(collider, interactibleCollider);
    }
    public bool Reach()
    {
        if (IsInteractable && interactingObject != null)
        {
            if (interactingObject.TryGetComponent(out PlayerControler controler))
            {
                if (controler.canCarrie && !controler.isHolding)
                {
                    Vector3 interactingObjectOffset = interactibleCollider.bounds.center;

                    Vector2 targetDirection = transform.position - interactingObjectOffset;

                    RaycastHit2D hit = Physics2D.Raycast(interactingObjectOffset, targetDirection);
                    if (hit.collider.gameObject != gameObject && hit.collider.gameObject != interactingObject.gameObject)
                    {
                        return false;
                    }

                    float interactiveBottomY = interactingObject.position.y - interactibleCollider.bounds.size.y / 2;
                    float pickableUpperY = transform.position.y + collider.bounds.size.y / 2;
                    float diff = pickableUpperY - interactiveBottomY - 0.1f;
                    if (diff <= 0)
                    {
                        
                        isBellow = true;
                    }
                    else
                    {
                        isBellow = false;
                    }
                    float distance = Mathf.Abs(interactingObject.position.x - transform.position.x);
                    float width = spriteWidth * 2;

                    if (distance > width)
                    {
                        float objectDirection = (interactingObject.position.x - transform.position.x) < 0 ? -1 : 1;
                        if (objectDirection == interactingObject.right.x)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return controler.PickableClosestToPlayer(transform);
        }
        return false;
    }
    private void Throw()
    {
        Vector2 targetVelocity;
        if (!collidedHorizontaly)
        {
            targetVelocity = new Vector2(throwSpeed * ThrowDirection * throwModifier + additionalForce, rgbd.velocity.y);
        }
        else
        {
            targetVelocity = new Vector2(rgbd.velocity.x, rgbd.velocity.y);
        }
        rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, .05f);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                if (!collision.gameObject.CompareTag(ignoreWithTag) && !isBeingCarried)
                {
                    ground = true;
                    if (isThrowing)
                    {
                        isThrowing = false;
                        collidedHorizontaly = false;
                    }
                    if (TryGetComponent(out MovementControler controller))
                    {
                        controller.NullifyMovement(false);
                    }
                }
                else
                {
                    rgbd.velocity = new Vector2(rgbd.velocity.x, rgbd.velocity.y);
                }
            }
            else
            {
                if (isThrowing && !collision.collider.CompareTag("NPC"))
                {
                    collidedHorizontaly = true;
                }
                else if (isBeingCarried)
                {
                    float targetDirection = (interactingObject.position.x - transform.position.x) > 0 ? -1 : 1;
                    Vector2 interactingVelocity = interactingObject.GetComponent<Rigidbody2D>().velocity;
                    float blockingDirection = (collision.transform.position.x - interactingVelocity.x) > 0 ? -1 : 1;
                    if (targetDirection == blockingDirection)
                    {
                        collidedHorizontaly = true;
                    }
                    else
                    {
                        collidedHorizontaly = false;
                    }
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        ground = false;
        additionalForce = 0;
        if (isBeingCarried && !isThrowing)
        {
            collidedHorizontaly = false;
        }
    }
}
