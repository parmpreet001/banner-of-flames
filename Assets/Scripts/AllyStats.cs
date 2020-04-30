using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStats : Stats
{
    // Start is called before the first frame update
    void Start()
    {
        Init();

        equippedWeapon = ((Weapon)inventory[0]);
    }
}
