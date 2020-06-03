using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

    }
    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

}
