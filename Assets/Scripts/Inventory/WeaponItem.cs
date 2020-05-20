using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]

public class WeaponItem : Item
{
    public int Damage;
    public float FireRate;
    public float BulletSpeed;
    public float ChargeSpeed;
    public int Spray;
}
