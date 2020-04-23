using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{

    public string Name;
    public string Description;
    public Sprite Icon;
    public string Tag;

    //public class Weapon : Item
    //{
    //    public float Damage { get; set; }
    //    public Weapon( float damage, string name)
    //    {
    //        Damage = damage;
    //        Name = name;
    //    }
    //}

    //public class Key : Item
    //{
    //    public string Color { get; set; }
    //    public Key(string color, string name)
    //    {
    //        Color = color;
    //        Name = name;
    //    }
    //}
}
