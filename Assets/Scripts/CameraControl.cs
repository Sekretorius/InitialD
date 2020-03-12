using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int sensitivity = 10;
    public int zoomFOV;
    public GameObject player;
    public Vector3 mousePosition { get; private set; }
    // Start is called before the first frame update
    Vector3 stockPosition;
    float width;
    float height;
    float stockFOV;
    public float zoomSpeed;
    //public float fovDiff;
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
        stockPosition = transform.position;
        stockFOV = Camera.main.fieldOfView;
        // fovDiff = (zoomFOV - stockFOV) / 10;

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = new Vector3(Input.mousePosition.x / width * sensitivity, Input.mousePosition.y / height * sensitivity, 0);
        transform.position = player.transform.position + stockPosition +
            mousePosition;

        if (Input.GetButton("Fire1") && Camera.main.fieldOfView != zoomFOV)
        {
            ZoomIn();
        }

        else if (!Input.GetButton("Fire1") && Camera.main.fieldOfView != stockFOV)
        {
            ZoomOut();
        }
    }

    void ZoomIn()
    {
        if ((Camera.main.fieldOfView + (zoomSpeed * Time.deltaTime)) < zoomFOV)
        {
            Camera.main.fieldOfView += ((zoomSpeed * Time.deltaTime));
            //Debug.Log(Camera.main.fieldOfView);          
        }
        else Camera.main.fieldOfView = zoomFOV;
    }

    void ZoomOut()
    {

        if ((Camera.main.fieldOfView - (zoomSpeed * Time.deltaTime)) > stockFOV)
        {
            Camera.main.fieldOfView -= ((zoomSpeed * Time.deltaTime));
            // Debug.Log(Camera.main.fieldOfView);
        }
        else Camera.main.fieldOfView = stockFOV;
    }
}
