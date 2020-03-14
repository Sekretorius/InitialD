using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlerV2 : MonoBehaviour
{
    public Animator anim;
    private float minGroungNormalY = 0.65f;
    private Vector2 targetVelocity;
    private Vector2 move;
    private Rigidbody2D rgbd;
    private bool ground = false;
    public float gravityModifier = 10f;
    private Vector3 velocity = Vector2.zero;
    private float movementSmoothing = .05f;
    private bool jump = false;
    public float speed = 10f;
    public float JumpTakeOffSpeed = 10f;

    void Start()
    {
        rgbd = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
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

        if (Input.GetKey(KeyCode.Space))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
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
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        ground = false;
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
