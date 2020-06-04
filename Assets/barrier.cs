using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class barrier : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject BossBarrier;
    [SerializeField] GameObject EndBarrier;
    [SerializeField] bool Fight = false;
    public GameObject checkPoint;
    public GameObject first;
    public GameObject second;

    [SerializeField] AudioSource source;
    public AudioClip Sound;

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("boss");
        BossBarrier = GameObject.Find("Boss constraint");
        EndBarrier = GameObject.Find("End constraint");
        checkPoint = GameObject.Find("Check");
        first = GameObject.Find("FirstCheck");
        second = GameObject.Find("BossCheck");
        //checkPoint.SetActive(false);
        checkPoint.transform.position = first.transform.position;
        // source = GameObject.Find("Music").GetComponent<AudioSource>();
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossBarrier.GetComponent<TilemapRenderer>().enabled = true;
        BossBarrier.GetComponent<TilemapCollider2D>().enabled = true;
        Fight = true;
        //checkPoint.SetActive(true);
        checkPoint.transform.position = second.transform.position;
        if (source.clip != Sound)
            source.Stop();
        if (!source.isPlaying)
        {
            source.clip = Sound;
            source.loop = true;
            source.Play();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Fight && !Boss)
        {
            Destroy(BossBarrier);
            Destroy(EndBarrier);
            Destroy(gameObject);
        }
    }
}
