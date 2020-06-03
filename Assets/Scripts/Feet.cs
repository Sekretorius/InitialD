using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    [SerializeField]AudioClip[] Footstep;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        Footstep = new AudioClip[8];
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        for (int i = 0; i < 8; i++)
        {
            Footstep[i] = (AudioClip)Resources.Load("Footsteps3/" + i);
        }
    }

    public void PlayFoot()
    {
        if(!source)
            source = GameObject.Find("Sound").GetComponent<AudioSource>();

        source.PlayOneShot(Footstep[Random.Range(0, 7)]);
    }

}
