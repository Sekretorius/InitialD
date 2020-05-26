using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
	public float speed = 20f;
	public int damage = 40;
	public float lifetime = 1f;
    public float movementSmoothing = 0.5f;
    //public Rigidbody2D rb;
    public GameObject impactEffect;
	//private object gameobject;
	[SerializeField] GameObject player;


	// Use this for initialization
	void Start()
	{
		
		 player = GameObject.FindGameObjectWithTag("Player");
		Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Vector2 destination = Vector2.MoveTowards()
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		//rb.velocity = transform.right * speed;
		//Vector2 targetVelocity = ((target - new Vector2(player.transform.position.x, player.transform.position.y)).normalized * speed);
		Vector2 targetVelocity;
		if ((target.x - player.transform.position.x) > 0)
			targetVelocity = new Vector2(speed,0f);
		else  targetVelocity = new Vector2(- speed,0f);
		

		Vector3 velocity = Vector3.zero;
		rb.velocity = targetVelocity; //Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

		StartCoroutine(BulletTimeOut());
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		if (hitInfo.CompareTag("Enemy"))
		{
           // Transform target = hitInfo.GetComponentInChildren<Transform>();                     
            //print("AT LEAST IT WORKS");
            HealthSystem enemy = hitInfo.GetComponent<HealthSystem>();
			if (enemy != null)
			{
                if(enemy.TryGetComponent(out EnemyMovement controler))
                {
                    controler.SetTarget(player.transform, true);
                }
                enemy.Damage(damage);
			}
			Destroy(gameObject);
		}
		//Instantiate(impactEffect, transform.position, transform.rotation); efektai poggers

	}



    IEnumerator Damage(SpriteRenderer renderer)
    {
        renderer.color = new Color(1, 0, 0, 1);
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("reached");
        renderer.color = new Color(1, 1, 1, 1);
    }

    IEnumerator BulletTimeOut()
	{
		print("pew");
		yield return new WaitForSecondsRealtime(lifetime);
		print("poof");
		Destroy(gameObject);
	}
}
