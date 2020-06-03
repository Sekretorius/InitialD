using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameObject control;

    void Start()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = gameObject;
        }
        else if (control != null)
        {
            Destroy(gameObject);
        }
    }
}
