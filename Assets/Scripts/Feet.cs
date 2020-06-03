using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    AudioClip[] Footstep;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        for(int i=0; i<8; i++)
        {
            Footstep[i] = (AudioClip)Resources.Load("Footsteps/" + i);
        }
    }

    public void PlayFoot()
    {
       source.PlayOneShot(Footstep[Random.Range(0, 7)]);
    }

}
