using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public new string name;
    public int dmg;
    public int accuracy;
    public int minRange;
    public int maxRange;
    public WeaponType weaponType;
}
