using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    //  [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;
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
    }
}
