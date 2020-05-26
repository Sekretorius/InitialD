using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerBean : MonoBehaviour
{
    public GameObject chalice;

    // Update is called once per frame
    void Update()
    {
        if (chalice == null)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            this.transform.position = new Vector3(this.transform.position.x+0.01f, this.transform.position.y,this.transform.position.z);
        }     
    }
}
