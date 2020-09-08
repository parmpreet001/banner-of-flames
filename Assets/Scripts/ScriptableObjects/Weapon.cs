using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Weapon : Item
{
    public int dmg;
    public int hitRate;
    public int critRate;
    public int minRange;
    public int maxRange;
    public int minRequirement;
    public WeaponType weaponType;

    public int spdModifier;

    public bool equipped = false;
}
