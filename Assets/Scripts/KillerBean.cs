using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerBean : MonoBehaviour
{
    public GameObject chalice;
    [SerializeField] AudioSource soundsource;
    [SerializeField] AudioSource musicsource;

    bool triggeredMusic =false;
    public AudioClip Sound;
    public AudioClip Music;

    private void Start()
    {
        chalice = GameObject.Find("ConsumeTheChalice");
        soundsource = GameObject.Find("Sound").GetComponent<AudioSource>();
        musicsource = GameObject.Find("Music").GetComponent<AudioSource>();

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
                soundsource.Stop();
                soundsource.clip = Sound;
                soundsource.Play();
                musicsource.Stop();
                musicsource.clip = Music;
                musicsource.Play();
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
