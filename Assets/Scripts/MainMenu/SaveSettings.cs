using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSettings : MonoBehaviour
{
    public TMP_Dropdown list;
    

    public void SetSize()
    {
        int res = list.value;
        Debug.Log(res);
        Resol[] resolutions = {
            new Resol(1920,1080),
            new Resol(1280, 720),
            new Resol(800, 600)
        };
        //Resolution resolution = Screen.currentResolution;
        Resol temp = resolutions[res];
        //if (resolution.height != temp.height)
        //{
            Screen.SetResolution(temp.width,temp.height,false);
            Debug.Log("Resolution changed!");
        //}
    }

    public class Resol
    {
        public int height;
        public int width;

        public Resol(int width, int height)
        {
            this.height = height;
            this.width = width;
        }
    }
}
