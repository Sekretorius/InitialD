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
    [SerializeField] GameObject Player;
    public GameObject GunTemplate;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d < 0f)
        {
            // scroll up
            if (selection != itemSlots.Length)
                selection++;
            else selection = 1;
            RefreshUI();
            UpdateGun();
        }
        else if (d > 0f)
        {
            // scroll down
            if (selection != 1)
                selection--;
            else selection = itemSlots.Length;
            RefreshUI();
            UpdateGun();
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



    void UpdateGun()
    {
        for (int i = 1; i <= items.Count; i++)
        {
            if (itemSlots[i - 1].Item.Tag == "Gun" && i != selection)
            {
                Destroy(GameObject.Find("GunTemplate(Clone)"));
                if(Player == null)
                {
                    Player = GameObject.FindGameObjectWithTag("Player");
                }
                if (Player.TryGetComponent(out PlayerControler controler))
                {
                    controler.HoldingGun(false);
                }
            }
        }
        //print(itemSlots[selection-1].Item.Tag);
        if (itemSlots[selection - 1].Item!=null)
            if( itemSlots[selection-1].Item.Tag == "Gun")
        {
                WeaponItem gunItem = (WeaponItem)itemSlots[selection - 1].Item;
           var gun = Instantiate(GunTemplate, Player.transform); //spawn gun
            gun.transform.SetParent(Player.transform); // assign gun to player
            gun.GetComponent<SpriteRenderer>().sprite = itemSlots[selection - 1].GetComponent<Image>().sprite; //assign sprite
            gun.GetComponent<Gun>().damage = gunItem.Damage;
                gun.GetComponent<Gun>().spray = gunItem.Spray;

                if (Player.TryGetComponent(out PlayerControler controler))
                {
                    controler.HoldingGun(true);
                }
         }

    }

    public bool Add(Item item)
    {
        if (items.Count < itemSlots.Length)
        {
            items.Add(item);
            RefreshUI();
            UpdateGun();
            return true;
        }
        return false;
    }

    public int Exists(string str)
    {
        for (int j = 0; j < itemSlots.Length; j++)
        {
            if (itemSlots[j].Item != null)
            {
                if (itemSlots[j].Item.Name == str)
                return j;
            }

        }
        return -1;
    }


    public bool Remove(int item)
    {
        if (item < 0)
            return false;
        if (itemSlots[item].Item != null)
        {
            items.Remove(itemSlots[item].Item);
            RefreshUI();
            UpdateGun();
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
            UpdateGun();
            return true;
        }
        return false;
    }

   /* public bool Spawn()
    {
        if (itemSlots[selection - 1].Item != null)
        {
            items.Remove(itemSlots[selection - 1].Item);
            RefreshUI();
            return true;
        }
        return false;
    }*/
}
