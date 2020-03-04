using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int sensitivity = 10;
    public GameObject player;
    // Start is called before the first frame update
    Vector3 stockPosition;
    float width;
    float height;

    void Start()
    {
         width = Screen.width;
         height = Screen.height;
         stockPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position+stockPosition + 
            new Vector3(Input.mousePosition.x / width * sensitivity, Input.mousePosition.y / height * sensitivity, 0);
    }
}
