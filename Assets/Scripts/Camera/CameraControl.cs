using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int sensitivity = 10;
    public int zoomSize;
    public GameObject player;
    public Vector3 mousePosition { get; private set; }
    // Start is called before the first frame update
    Vector3 stockPosition;
    float width;
    float height;
    float stockSize;
    public float zoomSpeed;
    //public float fovDiff;
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
        stockPosition = transform.position;
        stockSize = Camera.main.orthographicSize;
        // fovDiff = (zoomSize - stockSize) / 10;

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = new Vector3(Input.mousePosition.x / width * sensitivity, Input.mousePosition.y / height * sensitivity, 0);
        transform.position = player.transform.position + stockPosition +
            mousePosition;

        if (Input.GetButton("Fire2") && Camera.main.orthographicSize != zoomSize)
        {
            ZoomIn();
        }

        else if (!Input.GetButton("Fire2") && Camera.main.orthographicSize != stockSize)
        {
            ZoomOut();
        }
    }

    void ZoomIn()
    {
        if ((Camera.main.orthographicSize + (zoomSpeed * Time.deltaTime)) < zoomSize)
        {
            Camera.main.orthographicSize += ((zoomSpeed * Time.deltaTime));
            //Debug.Log(Camera.main.fieldOfView);          
        }
        else Camera.main.orthographicSize = zoomSize;
    }

    void ZoomOut()
    {

        if ((Camera.main.orthographicSize - (zoomSpeed * Time.deltaTime)) > stockSize)
        {
            Camera.main.orthographicSize -= ((zoomSpeed * Time.deltaTime));
            // Debug.Log(Camera.main.fieldOfView);
        }
        else Camera.main.orthographicSize = stockSize;
    }
}
