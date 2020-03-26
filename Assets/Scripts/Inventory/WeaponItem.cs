using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]

public class WeaponItem : Item
{
    public float Damage;
    public float FireRate;
    public float BulletSpeed;
    public float ChargeSpeed;
}
