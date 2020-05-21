using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	public float speed = 20f;
	public int damage = 40;
	public float lifetime = 1f;
    public float movementSmoothing = 0.5f;
    //public Rigidbody2D rb;
    public GameObject impactEffect;
	//private object gameobject;
	[SerializeField] GameObject player;
	[SerializeField] GameObject owner;


	// Use this for initialization
	void Start()
	{
		owner = gameObject;
		 player = GameObject.FindGameObjectWithTag("Player");
		Vector2 target = (player.transform.position);
		//Vector2 destination = Vector2.MoveTowards()
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
        Vector2 targetVelocity = ((target - new Vector2(owner.transform.position.x, owner.transform.position.y + Random.Range(-1f,1f) ) ).normalized * speed);
        Vector3 velocity = Vector3.zero;
		rb.velocity = targetVelocity; //Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

		StartCoroutine(BulletTimeOut());
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		if (hitInfo.CompareTag("Player"))
		{
			// Transform target = hitInfo.GetComponentInChildren<Transform>();                     
			//print("AT LEAST IT WORKS");
			GameObject obj = GameObject.Find("PlayerStats");
							HealthSystem health = obj.GetComponent<HealthSystem>();

			if (player != null)
			{
				health.Damage(damage);
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
