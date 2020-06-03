using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class barrier : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject BossBarrier;
    [SerializeField] bool Fight = false;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("boss");
        BossBarrier = GameObject.Find("Boss constraint");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossBarrier.GetComponent<TilemapRenderer>().enabled = true;
        BossBarrier.GetComponent<TilemapCollider2D>().enabled = true;
        Fight = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Fight && !Boss)
        {
            Destroy(BossBarrier);
            Destroy(gameObject);
        }
    }
}
