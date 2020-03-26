using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image Image;
    public Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;
            if(_item == null)
            {
                Image.enabled = false;
            }
            else
            {
                Image.sprite = _item.Icon;
                Image.enabled = true;
            }
        }
    }
    private void OnValidate()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
            //Debug.Log("aaaaaaaaaaaaaa");
        }

    }
}
