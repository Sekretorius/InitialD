    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public int viewDistance = 20;
    public int layerMask;
    public float offSetX = 0f;
    public float offSetY = 1f;
    public int rayCount = 90;
    public int startAngle = 45;
    public float minDistance = 5f;
    private EnemyMovement movement;
    private bool nearTarget = false;
    private Transform target;

    private void Start()
    {
        movement = GetComponentInParent<EnemyMovement>();
        layerMask = ~LayerMask.GetMask("NPC");
        offSetX += transform.localScale.x / 2;
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > minDistance)
            {   
                movement.SetTarget(null);
            }
        }
        for (int i = 0; i < rayCount; i++)
        {
            float angle = -startAngle + i; 
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * transform.right.x, Mathf.Sin(angle * Mathf.Deg2Rad));
            
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + offSetX * transform.right.x, transform.position.y + offSetY), direction, viewDistance, layerMask);
            float hitDistance = hit.distance == 0 ? viewDistance : hit.distance;
            Debug.DrawRay(new Vector2(transform.position.x + offSetX * transform.right.x, transform.position.y + offSetY), direction * hitDistance, Color.red);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player" && !nearTarget)
                {
                    PlayerControler controler = hit.collider.gameObject.GetComponent<PlayerControler>();
                    if (!controler.isHiden)
                    {
                        target = hit.collider.gameObject.transform;
                        movement.SetTarget(target);
                    }
                }
            }
        }
    }
}
