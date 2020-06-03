using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerBean : MonoBehaviour
{
    public GameObject chalice;
    [SerializeField] AudioSource source;
    bool triggeredMusic=false;
    public AudioClip Sound;

    private void Start()
    {
        chalice = GameObject.Find("ConsumeTheChalice");
        source = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chalice == null)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            this.transform.position = new Vector3(this.transform.position.x+0.01f, this.transform.position.y,this.transform.position.z);
            if(!triggeredMusic)
            {
                source.Stop();
                source.clip = Sound;
                source.Play();
                triggeredMusic = true;
            }
        }     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            GameObject.Find("PlayerStats").GetComponent<HealthSystem>().Damage(999);
        }
    }
}
