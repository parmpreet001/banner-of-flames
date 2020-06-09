using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealingMagic : Magic
{
    public bool areaOfEffect;
    public int heal;

    public int GetHealAmount(int mag)
    {
        return heal + (mag / 2);
    }
}
