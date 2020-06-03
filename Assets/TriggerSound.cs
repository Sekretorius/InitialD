using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public bool Music;
    [Space]
    [SerializeField] AudioSource source;
    public AudioClip Sound;

    void Start()
    {
        if(Music)
            source = GameObject.Find("Music").GetComponent<AudioSource>();
        else
                    source = GameObject.Find("Sound").GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (source.clip != Sound)
            source.Stop();
        if (!source.isPlaying)
        {
            source.clip=Sound;
            source.Play();
        }
        
       
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
