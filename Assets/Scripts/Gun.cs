﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public Transform firePoint;
    public GameObject bulletPrefab;
	public int damage;
    private GameObject player;
	private void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        bulletPrefab.GetComponent<Bullet>().damage = damage;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
            if (player.TryGetComponent(out PlayerControler controler))
            {
                Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float targetDirection = (target.x - player.transform.position.x) > 0 ? 1 : -1;
                controler.Turn(targetDirection);
                if (controler.Shoot())
                {
                    if(targetDirection > 0)
                    {
                        firePoint = player.transform.Find("ShootPointRight");
                    }
                    else
                    {
                        firePoint = player.transform.Find("ShootPointLeft");
                    }
                    Shoot();
                }
            }
        }
	}

	void Shoot()
	{
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
	}
}
