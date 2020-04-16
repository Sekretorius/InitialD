using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
   // [SerializeField] List<Item> items;
    [SerializeField] List<Item> items;
    [SerializeField] int selection=1;
    [SerializeField] ItemSlot[] itemSlots;

    private void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            // scroll up
            if (selection != itemSlots.Length)
                selection++;
            else selection = 1;
            RefreshUI();
        }
        else if (d < 0f)
        {
            // scroll down
            if (selection != 1)
                selection--;
            else selection = itemSlots.Length;
            RefreshUI();
        }
        if (Input.GetButtonDown("Discard"))
            Remove();
    }

    private void OnValidate()
    {
        //  if (itemsParent != null)
    itemSlots = gameObject.GetComponentsInChildren<ItemSlot>();
        RefreshUI();
    }

    void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }

        for (int j=1; j <= itemSlots.Length; j++)
        {
            if (j  == selection)
                itemSlots[j - 1].GetComponent<Transform>().Find("Highlight").GetComponent<Image>().enabled = true;
            else itemSlots[j - 1].GetComponent<Transform>().Find("Highlight").GetComponent<Image>().enabled = false;
        }
    }

    public bool Add(Item item)
    {
        if (items.Count < itemSlots.Length)
        {
            items.Add(item);
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool Remove()
    {
        if (itemSlots[selection - 1].Item != null)
        {
            items.Remove(itemSlots[selection - 1].Item);
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool Spawn()
    {
        if (itemSlots[selection - 1].Item != null)
        {
            items.Remove(itemSlots[selection - 1].Item);
            RefreshUI();
            return true;
        }
        return false;
    }
}
