using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysics : MonoBehaviour
{
    public float minGroundNormalY = .65f;
    public float gravityModifier = 10f;
    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    public float mass = 1000;
    public float frictionModifier = 10f;
    public bool canSlip = true;
    protected Vector2 aditionalforce;
    protected Vector2 SlipForce;
    protected float minInteractForce = 300f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }
    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
        if (groundNormal.x != 0 && grounded && canSlip)
        {
            SlipForce += new Vector2(groundNormal.y, -groundNormal.x) * mass / frictionModifier * Time.fixedDeltaTime;
        }
        if (targetVelocity.x == 0 && velocity.x != 0)
        {
            velocity.x -= velocity.x * Time.fixedDeltaTime;
        } else if (targetVelocity.x == 0 && aditionalforce.x == 0 && SlipForce.x == 0)
        {
            velocity.x = 0;
        }

        velocity.x = targetVelocity.x + aditionalforce.x + SlipForce.x;
        aditionalforce.x = 0;

        if (targetVelocity.x == 0) SlipForce.x -= SlipForce.x * frictionModifier * Time.fixedDeltaTime;
        else if (Mathf.Abs(SlipForce.x) > 1) SlipForce.x -= SlipForce.x * 0.5f;
        else SlipForce.x = 0;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Interact(move);
        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }
    public void Prepare()
    {
        
    }

    public void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                    }
                }
                float projection = Vector2.Dot(velocity, currentNormal);
                
                if (projection < 0)
                {
                    velocity -= projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position += move.normalized * distance;
    }
    private void Interact(Vector2 move)
    {
        float distance = move.magnitude;
        int count = rb2d.Cast(move, contactFilter, hitBuffer, distance);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++)
        {
            hitBufferList.Add(hitBuffer[i]);
        }
        foreach (RaycastHit2D hit in hitBufferList)
        {
            if (hit != false)
            {
                ObjectPhysics physics = null;
                hit.collider.TryGetComponent(out physics);
                if (physics != null)
                {
                    
                    float diff = (mass - physics.mass);
                    if (diff <= 0) diff = minInteractForce;
                    physics.aditionalforce = move * diff * Time.fixedDeltaTime;
                    physics.SlipForce = SlipForce;
                    SlipForce.x *= 0.5f;
                }
            }
        }
    }
}