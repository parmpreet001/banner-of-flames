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

    //Checks to see if the unit can equip a weapon, based on the weapons their class can use, and the skill level requirement for the weapon
    public bool CanUseWeapon(int index)
    {
        
        Weapon weapon = ((Weapon)inventory[index]);

        if(classType.skillLevels.weaponLevels[(int)weapon.weaponType] >= weapon.minRequirement &&
            skillLevels.weaponLevels[(int)weapon.weaponType] >= weapon.minRequirement)
        {
            return true;
        }

        return false;
    }

    public void EquipWeapon(int index)
    {
        ((Weapon)inventory[index]).equipped = true;
        equippedWeapon.equipped = false;
        equippedWeapon = ((Weapon)inventory[index]);
    }

    public void EquipPreviousWeapon()
    {
        int indexEquippedWeapon = 0; //index of the weapon currently equipped
        bool weaponFound = false;
        for(int i = 0; i < maxInventorySize; i++)
        {
            if(inventory[i] != null &&  inventory[i].GetType() == typeof(Weapon) && ((Weapon)inventory[i]).equipped)
            {
                indexEquippedWeapon = i;
                i = maxInventorySize;
            }
        }

        for(int i = indexEquippedWeapon-1; i >= 0; i--)
        {
            if(inventory[i] != null && inventory[i].GetType() == typeof(Weapon))
            {
                EquipWeapon(i);
                i = -1;
                weaponFound = true;
            }
        }

        if(!weaponFound)
        {
            for (int i = maxInventorySize-1; i >= 0; i--)
            {
                if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon))
                {
                    EquipWeapon(i);
                    i = -1;
                }
            }
        }
    }

    public void EquipNextWeapon()
    {
        int indexEquippedWeapon = 0; //index of the weapon currently equipped
        bool weaponFound = false;
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon) && ((Weapon)inventory[i]).equipped)
            {
                indexEquippedWeapon = i;
                i = maxInventorySize;
            }
        }

        for (int i = indexEquippedWeapon + 1; i <= maxInventorySize-1; i++)
        {
            if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon))
            {
                EquipWeapon(i);
                i = maxInventorySize;
                weaponFound = true;
            }
        }

        if (!weaponFound)
        {
            for (int i = 0; i <= maxInventorySize-1; i++)
            {
                if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon))
                {
                    EquipWeapon(i);
                    i = maxInventorySize;
                }
            }
        }
    }
}
