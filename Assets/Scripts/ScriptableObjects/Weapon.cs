using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : Item
{
    public int dmg;
    public int accuracy;
    public int critRate;
    public int minRange;
    public int maxRange;
    public int minRequirement;
    public WeaponType weaponType;

    public bool equipped = false;
}
