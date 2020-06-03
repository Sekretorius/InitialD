using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy control;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != null)
        {
            Destroy(gameObject);
        }
    }
}
