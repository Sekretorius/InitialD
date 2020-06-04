using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject main;

    void Start()
    {
        main = GameObject.Find("PLSNODESTROY");
    }

    private void Update()
    {
        if(main == null)
        {
            main = GameObject.Find("PLSNODESTROY");
        }
        main.SetActive(false);
    }


}
