using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObeject : Interactable
{
    private float offsetY = 0.3f;
    private bool pick = false;
    public float throwUpSpeed = 20f;
    public float throwSpeed = 10F;
    public float throwModifier = 10f;
    Rigidbody2D rgbd;
    private new Collider2D collider;


    private Vector3 velocity = Vector3.zero;
    private Vector2 targetVelocity = Vector3.zero;
    public bool isThrowing = false;
    public bool isBeingCarried = false;
    private float ThrowDirection = 0;
    private float minGroungNormalY = 0.65f;
    public bool ground = false;
    private string tag = "";


    protected new void Start()
    {
        base.Start();
        collider = GetComponent<Collider2D>();
        rgbd = GetComponent<Rigidbody2D>();
    }
    protected override void OnEvent()
    {
        
        if (isInteractable && Input.GetKey(KeyCode.E) && interactingObject != null)
        {
            BoxCollider2D interactingCollider = interactingObject.GetComponent<BoxCollider2D>();
            //float objectDeltaY = transform.position.y + transform.localScale.y / 2;
            //float deltaY = interactingObject.position.y - interactingObject.localScale.y / 2;
            float objectOffsetDeltaY = interactingCollider.bounds.center.y + interactingCollider.bounds.size.y / 2 + collider.bounds.size.y /2 + offsetY;
            
            //float diff = objectDeltaY - deltaY;
            //pick = (diff < 0 && Input.GetKey(KeyCode.DownArrow)) || diff >= 0 ? true : false;
            pick = true;

            if (pick && !interactingObject.GetComponent<MovementControler>().isCarring)
            {
                if (TryGetComponent(out MovementControler controller))
                {
                    controller.nullifyMovement(true);
                }

                isThrowing = false;
                isBeingCarried = true;
                interactingObject.GetComponent<MovementControler>().isCarring = true;
                transform.position = new Vector2(interactingObject.position.x, objectOffsetDeltaY);
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.parent = interactingObject;
                rgbd.isKinematic = true;
            }
            ground = false; 
        }
        if (transform.parent != null && !Input.GetKey(KeyCode.E)) {
            isThrowing = true;
            isBeingCarried = false;
            ThrowDirection = interactingObject.right.x;
            tag = interactingObject.gameObject.tag;
            interactingObject.GetComponent<MovementControler>().isCarring = false;
            transform.parent = null;
            rgbd.isKinematic = false;
            Vector2 targetVelocity = new Vector2(rgbd.velocity.x, throwUpSpeed);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, .05f);
        }
        if (isThrowing && !ground)
        {
            Throw();
        }
        if(ground)
        {
            Vector2 targetVelocity = new Vector2(0, 0);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, .05f);
        }
        if (!ground && !isBeingCarried)
        {
            rgbd.velocity += Physics2D.gravity * 6 * Time.fixedDeltaTime;
        }
    }
    private void Throw()
    {
        Vector2 targetVelocity = new Vector2(throwSpeed * ThrowDirection * throwModifier, rgbd.velocity.y);
        rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref velocity, 0);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isThrowing)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > minGroungNormalY)
                {
                    if (collision.gameObject.tag != tag)
                    {
                        ground = true;
                        interactingObject = null;
                        isThrowing = false;
                        if (TryGetComponent(out MovementControler controller))
                        {
                            controller.nullifyMovement(false);
                        }
                    }
                    else
                    {
                        rgbd.velocity = new Vector2(rgbd.velocity.x, 0);
                    }
                }
            }
        }
    }
}
