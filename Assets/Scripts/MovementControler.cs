using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementControler : MonoBehaviour
{
    public float speed = 10f;
    public float JumpTakeOffSpeed = 10f;
    public float movementSmoothing = .05f;
    public float gravityModifier = 10f;
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
    

    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        move = Vector2.zero;
        ComputeMovement();
        Movement();
    }
    abstract protected void ComputeMovement();
    public void Movement()
    {
        Turn(move.x);

        if (move.x != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

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
            else if (collision.collider.tag != "Player" && collision.collider.tag != "NPC")
            {
                isBlocked = true;
                obsticle = collision.collider.transform;
                float cordY = transform.position.y - transform.localScale.y / 2;
                float colCordY = collision.transform.position.y + collision.transform.localScale.y / 2;
                float diff = colCordY - cordY;
                if(diff <= transform.localScale.y / 2)
                {
                    canJumpOver = true;
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
    private void Turn(float dir)
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
}
