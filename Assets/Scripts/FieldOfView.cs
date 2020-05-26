    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int viewDistance = 5;
    public int layerMask;
    public float offSetX = 0f;
    public float offSetY = 1f;
    public int rayCount = 50;
    public int startAngle = 25;
    public float minDistance = 5f;
    private MovementControler movement;
    private bool nearTarget = false;
    private Transform target;
    private float viewDirection = 1;

    private void Start()
    {
        movement = GetComponentInParent<MovementControler>();
        layerMask = ~LayerMask.GetMask("NPC", "Enemy");
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > minDistance)
            {   
                movement.SetTarget(null, false);
            }
        }
        for (int i = 0; i < rayCount; i++)
        {
            if(movement.facingDirection != 0)
            {
                viewDirection = movement.facingDirection;
            }
            float angle = -startAngle + i; 
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * viewDirection, Mathf.Sin(angle * Mathf.Deg2Rad));
            if (i % 5 == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + offSetX * viewDirection, transform.position.y + offSetY), direction, viewDistance, layerMask);
                //float hitDistance = hit.distance == 0 ? viewDistance : hit.distance;
                //Debug.DrawRay(new Vector2(transform.position.x + offSetX * viewDirection, transform.position.y + offSetY), direction * hitDistance, Color.red);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player") && !nearTarget)
                    {
                        if (hit.collider.TryGetComponent(out PlayerControler controler))
                        {
                            if (!controler.isHiden)
                            {
                                target = hit.collider.gameObject.transform;
                                movement.SetTarget(target, false);
                            }
                        }
                    }
                }
            }
        }
    }
}
