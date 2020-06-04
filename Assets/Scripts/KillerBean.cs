using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerBean : MonoBehaviour
{
    public GameObject chalice;
    [SerializeField] AudioSource soundsource;
    [SerializeField] AudioSource musicsource;

    bool triggeredMusic =false;
    public AudioClip Sound;
    public AudioClip Music;

    public GameObject checkPoint;
    public GameObject otherPoint;

    private GameObject GameManager;

    private void Start()
    {
        chalice = GameObject.Find("ConsumeTheChalice");
        soundsource = GameObject.Find("Sound").GetComponent<AudioSource>();
        musicsource = GameObject.Find("Music").GetComponent<AudioSource>();

        checkPoint = GameObject.Find("Check");
        otherPoint = GameObject.Find("IndianaPoint");

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
            checkPoint.transform.position = otherPoint.transform.position;
            GameManager = GameObject.Find("GameManager");
            if (GameManager.TryGetComponent(out SceneLoader loader))
            {
                loader.SetNewScene(SceneManager.GetActiveScene().name, "ToCheckPoint");
                //health.SetHearts(5);
            }
            Inventory inv = GameObject.FindObjectOfType<Inventory>();
            GameObject.FindObjectOfType<Inventory>().Remove(inv.Exists("Chalice"));
        }
    }
}
