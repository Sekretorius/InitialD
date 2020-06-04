using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public bool Music;
    public bool Loop;
    [Space]
    [SerializeField] AudioSource source;
    public AudioClip Sound;

    void Start()
    {
        if (Music)
            source = GameObject.Find("Music").GetComponent<AudioSource>();
        else
            source = GameObject.Find("Sound").GetComponent<AudioSource>();

        if (source.clip != Sound)
            source.Stop();
        if (!source.isPlaying)
        {
            source.clip = Sound;
            //source.loop = Loop;
            source.Play();         
        }
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            Application.Quit();
        }
    }
}
