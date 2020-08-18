using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AllyStats : Stats
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void LevelUp()
    {
        level++;
        double[] rand = new double[7];
        for (int i = 0; i < 7; i++)
            rand[i] = Random.value * 100;

        if (rand[0] <= hpGrowth + classType.hpGrowth)
        {
            maxHP++; hp++;
        }     
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
        
    }

    //If unit gained a level, returns an array of the unit's stats before leveling up. Null otherwise
    public int[] AddExperience(int exp)
    {
        int[] previousStats = { hp, str, mag, spd, skl, def, res, classType.mov };

        experience += exp;
        if(experience >= 100)
        {
            experience -= 100;
            LevelUp();
            return previousStats;
        }
        else
        {
            return null;
        }
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

    public bool CanUseWeapon(Weapon weapon)
    {
        if (classType.skillLevels.weaponLevels[(int)weapon.weaponType] >= weapon.minRequirement &&
    skillLevels.weaponLevels[(int)weapon.weaponType] >= weapon.minRequirement)
        {
            return true;
        }

        return false;
    }

    public void EquipWeapon(int index)
    {
        ((Weapon)inventory[index]).equipped = true;
        if(equippedWeapon)
            equippedWeapon.equipped = false;
        equippedWeapon = ((Weapon)inventory[index]);

        attackMethod = AttackMethod.PHYSICAL;
    }

    public void EquipWeapon(Weapon weapon)
    {
        if(!weapon.equipped)
        {
            weapon.equipped = true;
            if (equippedWeapon)
                equippedWeapon.equipped = false;
            equippedWeapon = weapon;
        }


        attackMethod = AttackMethod.PHYSICAL;
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
            if(inventory[i] != null && inventory[i].GetType() == typeof(Weapon) && CanUseWeapon(i))
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
                if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon) && CanUseWeapon(i))
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

        //Finds index of weapon currently equipped
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon) && ((Weapon)inventory[i]).equipped)
            {
                indexEquippedWeapon = i;
                i = maxInventorySize;
            }
        }

        //Starting from the index of the equipped method, goes through every item in the array after that, and equipps the first weapon if found
        for (int i = indexEquippedWeapon + 1; i <= maxInventorySize-1; i++)
        {
            if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon) && CanUseWeapon(i))
            {
                EquipWeapon(i);
                i = maxInventorySize;
                weaponFound = true;
            }
        }

        //if the weapon has not been found, then search again, but this time go through every item in the array before the index of the equipped weapon
        if (!weaponFound)
        {
            for (int i = 0; i <= maxInventorySize-1; i++)
            {
                if (inventory[i] != null && inventory[i].GetType() == typeof(Weapon) && CanUseWeapon(i))
                {
                    EquipWeapon(i);
                    i = maxInventorySize;
                }
            }
        }
    }

    public void EquipBlackMagic(int index)
    {
        if(equippedBlackMagic)
            equippedBlackMagic.equipped = false;
        equippedBlackMagic = blackMagic[index];
        equippedBlackMagic.equipped = true;

        attackMethod = AttackMethod.OFFENSIVE_MAGIC;
    }

    public void EquipPreviousBlackMagic()
    {
        if(blackMagic.Count > 1)
        {
            int indexOfEquippedMagic = blackMagic.IndexOf(equippedBlackMagic);

            if(indexOfEquippedMagic == 0)
                EquipBlackMagic(blackMagic.Count - 1);
            else
                EquipBlackMagic(indexOfEquippedMagic-1);
        }
    }

    public void EquipNextBlackMagic()
    {
        if (blackMagic.Count > 1)
        {
            int indexOfEquippedMagic = blackMagic.IndexOf(equippedBlackMagic);

            if (indexOfEquippedMagic == blackMagic.Count - 1)
                EquipBlackMagic(0);
            else
                EquipBlackMagic(indexOfEquippedMagic + 1);
        }
    }

    public void EquipWhiteMagic(int index)
    {
        if(equippedWhiteMagic)
            equippedWhiteMagic.equipped = false;
        equippedWhiteMagic = whiteMagic[index];
        equippedWhiteMagic.equipped = true;
    }

    public void LoadStats(int index, SaveData save)
    {
        
        level = save.playerUnits[index].level;
        experience = save.playerUnits[index].experience;

        baseHP = save.playerUnits[index].baseHP;
        baseSTR = save.playerUnits[index].baseSTR;
        baseMAG = save.playerUnits[index].baseMAG;
        baseDEF = save.playerUnits[index].baseDEF;
        baseRES = save.playerUnits[index].baseRES;
        baseSKL = save.playerUnits[index].baseSKL;
        baseSPD = save.playerUnits[index].baseSPD;

        hp = save.playerUnits[index].hp;
        str = save.playerUnits[index].str;
        mag = save.playerUnits[index].mag;
        def = save.playerUnits[index].def;
        res = save.playerUnits[index].res;
        skl = save.playerUnits[index].skl;
        spd = save.playerUnits[index].spd;

        hpGrowth = save.playerUnits[index].hpGrowth;
        strGrowth = save.playerUnits[index].strGrowth;
        magGrowth = save.playerUnits[index].magGrowth;
        defGrowth = save.playerUnits[index].defGrowth;
        resGrowth = save.playerUnits[index].resGrowth;
        sklGrowth = save.playerUnits[index].sklGrowth;
        spdGrowth = save.playerUnits[index].spdGrowth;

        maxHP = save.playerUnits[index].maxHP;

        skillLevels = save.playerUnits[index].skillLevels;

        classType = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Classes/" + save.playerUnits[index].classType + ".asset",
            typeof(ClassType)) as ClassType;

        equippedWeapon = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Weapons/" + save.playerUnits[index].equippedWeapon + ".asset",
            typeof(Weapon)) as Weapon;
        equippedWhiteMagic = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/HealingMagic/" + save.playerUnits[index].equippedWhiteMagic,
            typeof(Magic)) as Magic;
        equippedBlackMagic = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/OffensiveMagic/" + save.playerUnits[index].equippedBlackMagic + ".asset",
            typeof(Magic)) as Magic;

        magicList = new List<Magic>();
        blackMagic = new List<Magic>();
        whiteMagic = new List<Magic>();

        foreach(string spell in save.playerUnits[index].magicList)
        {
            if(AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/OffensiveMagic/" + spell + ".asset",typeof(Magic)))
            {
                magicList.Add(AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/OffensiveMagic/" + spell + ".asset",
                typeof(Magic)) as Magic);
            }
            else if(AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/HealingMagic/" + spell + ".asset",typeof(Magic)))
            {
                magicList.Add(AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/HealingMagic/" + spell + ".asset",
                typeof(Magic)) as Magic);
            }
        }
        foreach(string spell in save.playerUnits[index].blackMagic)
        {
            blackMagic.Add(AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/OffensiveMagic/" + spell + ".asset",
                typeof(Magic)) as Magic);
        }
        foreach(string spell in save.playerUnits[index].whiteMagic)
        {
            whiteMagic.Add(AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/HealingMagic/" + spell + ".asset",
                typeof(Magic)) as Magic);
        }

        attackMethod = save.playerUnits[index].attackMethod;
    }
}
