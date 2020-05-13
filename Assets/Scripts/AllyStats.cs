using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStats : Stats
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void LevelUp(int levels)
    {
        level++;
        double[] rand = new double[7];
        for (int i = 0; i < 7; i++)
            rand[i] = Random.value * 100;

        while (levels > 0)
        {
            if (rand[0] <= hpGrowth + classType.hpGrowth)
                hp++;
            if (rand[1] <= strGrowth + classType.strGrowth)
                str++;
            if (rand[2] <= magGrowth + classType.magGrowth)
                mag++;
            if (rand[3] <= defGrowth + classType.defGrowth)
                def++;
            if (rand[4] <= resGrowth + classType.resGrowth)
                res++;
            if (rand[5] <= sklGrowth + classType.sklGrowth)
                skl++;
            if (rand[6] <= spdGrowth + classType.spdGrowth)
                spd++;
            levels--;
        }
        maxHP = hp;
    }

    public void EquipWeapon(int index)
    {
        ((Weapon)inventory[index]).equipped = true;
        equippedWeapon.equipped = false;
        equippedWeapon = ((Weapon)inventory[index]);
    }
}
