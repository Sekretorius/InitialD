using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class MovementControler : MonoBehaviour
{
    public float speed = 10f;
    public float JumpTakeOffSpeed = 30f;
    public float movementSmoothing = .05f;
    public float gravityModifier = 6f;
    protected Animator anim;
    protected float minGroungNormalY = 0.65f;
    protected Vector2 targetVelocity;
    protected Vector2 move;
    protected Rigidbody2D rgbd;
    protected bool ground = false;
    protected Vector3 velocity = Vector2.zero;
    protected bool jump = false;
    protected bool isBlocked = false;
    protected Transform obsticle = null;
    protected bool canJumpOver = false;

    public bool isHolding = false;
    public bool canCarrie = true;
    public float inpickableTimmer = 0.15f;
    private float timerToPick;
    protected Transform closestPickable;
    protected float distanceToPickable = -1;

    protected bool canMove = true;

    protected bool nullifiedImput = false;

    protected void Start()
    {
        timerToPick = inpickableTimmer;
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void FixedUpdate()
    {
        if (rgbd != null)
        {
            if (!nullifiedImput)
            {
                ComputeMovement();
                ComputeAnimation();
                Movement();
            }
        }
        else
        {
            if (TryGetComponent(out Rigidbody2D rig))
            {
                rgbd = rig;
            }
        }
        PickTimmer();
        move = Vector2.zero;
        ground = false;
    }
    abstract protected void ComputeMovement();
    abstract protected void ComputeAnimation();
    public void Movement()
    {
        targetVelocity = new Vector2(move.x * speed, rgbd.velocity.y);
        rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (jump && ground)
        {
            targetVelocity = new Vector2(rgbd.velocity.x, JumpTakeOffSpeed);
        }
        else if (!jump)
        {
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                ground = true;
            }
            else if(collision.collider.tag != "Player" && collision.collider.tag != "NPC")
            {
                isBlocked = true;
                obsticle = collision.collider.transform;
            }
            if (collision.collider.tag == "Player" && gameObject.tag == "NPC_Ignored")
            {
                isBlocked = false;
                Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
            }

        }
    }
    private void PickTimmer()
    {
        if(timerToPick <= 0)
        {
            canCarrie = true;
            timerToPick = inpickableTimmer;
        }
        if (!canCarrie)
        {
            timerToPick -= Time.fixedDeltaTime;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > minGroungNormalY)
            {
                ground = true;
            }
            else
            {
                isBlocked = true;
                obsticle = collision.collider.transform;
                if(collision.collider.tag != "Player" && collision.collider.tag != "NPC"){
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
        isBlocked = false;
        obsticle = null;
    }
    protected void Turn(float dir)
    {
        if (dir > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (dir < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
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
        if(collision.TryGetComponent(out PickObject script) && gameObject.tag == "Player")
        {
            bool reachable = false;
            Vector2 targetDirection = collision.transform.position - transform.position;
            Vector2 colliderDimensions1 = script.colliderDimensions;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection);
            if (hit)
            {
                if(hit.collider.gameObject == collision.gameObject)
                {
                    bool cantPick = PickObject.CheckCeiling(GetComponent<CapsuleCollider2D>(), collision.gameObject,colliderDimensions1, 0.3f);
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
                    bool cantPick = PickObject.CheckCeiling(GetComponent<CapsuleCollider2D>(), closestPickable.gameObject, colliderDimensions2, 0.3f);
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
