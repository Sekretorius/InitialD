using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public  void TakeDamage(int dmg)
    {
        Debug.Log(dmg);
        health -= dmg;
        if (health <= 0)
            Destroy(this.gameObject);
    }
}
