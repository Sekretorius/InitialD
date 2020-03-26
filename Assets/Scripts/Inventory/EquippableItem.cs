using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equippable")]

public class EquippableItem : Item
{

    public Type Slot;

    public enum Type // your custom enumeration
    {
        Armor,
        Boots,
        Helmet,
        Glasses
    };

    [Header("Stat Modifiers")]
    public float Health;


    [Header("Speed Modifiers")]
    public float JumpSpeed;
    public float MovementSpeed;
    public float RollSpeed;

    [Header("Others")]
    [Range(0.0f, 100.0f)]

    public float Dankness;
}
